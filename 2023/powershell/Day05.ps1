using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day05.txt"
$inputData = Get-Content $inputFile -Raw

function Get-Day05_1 {
    part_1 $inputData
}

function Get-Day05_2 {
    part_2 $inputData
}

function part_1 {
    param (
        [string] $content
    )

    $chunks = $content.Trim() -split "`n`n"

    $m = [regex]::Match($chunks[0], 'seeds:(?:\s+(?<seeds>\d+))+')
    if (-not $m.Success) { throw "Invalid seed: $_" }
    [Int64[]]$seeds = $m.Groups["seeds"].Captures.Value

    $n = $chunks.Count - 1
    $maps = $chunks[1..$n] | ForEach-Object { [Map]::Parse($_) }

    $locations = foreach ($seed in $seeds) {
        $id = $seed
        foreach ($map in $maps) {
            $id = $map.Convert($id)
        }
        $id
    }

    ($locations | Measure-Object -Minimum).Minimum
}

function part_2 {
    param (
        [string] $content
    )

    $chunks = $content.Trim() -split "`n`n"

    $m = [regex]::Match($chunks[0], 'seeds:(?<range>\s+\d+\s+\d+)+')
    if (-not $m.Success) { throw "Invalid seed ranges: $_" }
    [Range[]]$seeds = $m.Groups["range"].Captures.Value.Trim() | ForEach-Object {
        $values = -split $_
        [Range]@{
            Start  = $values[0]
            Length = $values[1]
        }
    }

    $n = $chunks.Count - 1
    $maps = $chunks[1..$n] | ForEach-Object { [Map]::Parse($_) }

    $ids = $seeds
    foreach ($map in $maps) {
        $ids = $map.ConvertRanges($ids)
    }

    ($ids | Measure-Object -Minimum -Property Start).Minimum
}

class Range {
    [Int64] $Start
    [Int64] $Length

    [Int64] Stop() {
        return $this.Start + $this.Length - 1
    }
}

class RangeMapping {
    [Int64] $DestinationStart
    [Int64] $SourceStart
    [Int64] $Length

    [Int64] SourceEnd() {
        return $this.SourceStart + $this.Length - 1
    }

    [bool] Contains([Int64] $source) {
        return $source -ge $this.SourceStart -and $source -lt ($this.SourceStart + $this.Length)
    }

    [Int64] Convert([Int64] $source) {
        return $this.DestinationStart + ($source - $this.SourceStart)
    }

    [Range] ConvertRange([Range] $source) {
        return [Range]@{
            Start  = $this.DestinationStart + ($source.Start - $this.SourceStart)
            Length = $source.Length
        }
    }

    [RangeMapping] static Parse([string] $data) {
        $m = [regex]::Match($data, '(\d+)\s+(\d+)\s+(\d+)')
        if (-not $m.Success) { throw "Invalid range mapping: $_" }

        return [RangeMapping]@{
            DestinationStart = $m.Groups[1].Value
            SourceStart      = $m.Groups[2].Value
            Length           = $m.Groups[3].Value
        }
    }
}

class Map {
    [string] $Name
    [RangeMapping[]] $Ranges

    [Int64] Convert([Int64] $source) {
        foreach ($range in $this.Ranges) {
            if ($range.Contains($source)) {
                return $range.Convert($source)
            }
        }
        return $source
    }

    [Range[]] ConvertRanges([Range[]] $idRanges) {
        $done = @()
        $todo = $idRanges
        foreach ($rm in $this.Ranges) {
            if ($todo.Count -eq 0) {
                break
            }

            $nextTodo = @()
            foreach ($r in $todo) {
                $IdStart = $r.Start
                $IdEnd = $r.Stop()
                $MapStart = $rm.SourceStart
                $MapEnd = $rm.SourceEnd()

                switch ($true) {
                    # Fully out of map range
                    ($IdStart -gt $MapEnd -or $IdEnd -lt $MapStart) {
                        $nextTodo += $r
                        continue
                    }

                    # Fully in map range
                    ($IdStart -ge $MapStart -and $IdEnd -le $MapEnd) {
                        $done += $rm.ConvertRange($r)
                        continue
                    }

                    # Partial/full overlap/overflow: Take left outer range
                    ($IdStart -lt $MapStart) {
                        $nextTodo += [Range]@{
                            Start  = $IdStart
                            Length = $MapStart - $IdStart
                        }
                    }

                    # Partial/full overlap/overflow: Take right outer range
                    ($IdEnd -gt $MapEnd) {
                        $nextTodo += [Range]@{
                            Start  = $MapEnd + 1
                            Length = $IdEnd - $MapEnd
                        }
                    }

                    # Full overflow: Take map range
                    ($IdStart -lt $MapStart -and $IdEnd -gt $MapEnd) {
                        $done += $rm.ConvertRange([Range]@{
                                Start  = $rm.SourceStart
                                Length = $rm.Length
                            })
                        continue
                    }

                    # Partial overlap: Take left inner range
                    ($rm.Contains($IdEnd)) {
                        $done += $rm.ConvertRange([Range]@{
                                Start  = $MapStart
                                Length = $IdEnd - $MapStart + 1
                            })
                        continue
                    }

                    # Partial overlap: Take right inner range
                    ($rm.Contains($IdStart)) {
                        $done += $rm.ConvertRange([Range]@{
                                Start  = $IdStart
                                Length = $MapEnd - $IdStart + 1
                            })
                        continue
                    }

                    Default { throw "PANIC!" }
                }

            }
            $todo = $nextTodo
        }
        return $done + $todo
    }

    [Map] static Parse([string] $data) {
        $lines = $data -split "`n"
        $n = $lines.Count - 1

        $m = [regex]::Match($lines[0], '\s*(?<name>.+) map:')
        if (-not $m.Success) { throw "Invalid map header: $_" }

        return [Map]@{
            Name   = $m.Groups["name"].Value
            Ranges = $lines[1..$n] | ForEach-Object { [RangeMapping]::Parse($_) }
        }
    }
}

# Get-Day05_1 # 175622908
# Get-Day05_2 # 5200543
