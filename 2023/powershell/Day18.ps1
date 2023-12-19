using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day18.txt"
$inputLines = Get-Content $inputFile
function Get-Day18_1 {
    part_1 $inputLines
}

function Get-Day18_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $instructions = switch -Regex ($lines) {
        '^(?<Direction>[LRUD]) (?<Length>\d+) \(#[0-9a-f]{6}\)$' {
            [Instruction]@{
                Length    = $Matches.Length
                Direction = $Matches.Direction
            }
        }
        Default { throw "Invalid line: $_" }
    }

    area $instructions
}

function part_2 {
    param (
        [string[]] $lines
    )

    $instructions = switch -Regex ($lines) {
        '^[LRUD] \d+ \(#(?<Length>[0-9a-f]{5})(?<Direction>[0-3])\)$' {
            [Instruction]@{
                Length    = "0x$($Matches.Length)"
                Direction = switch ($Matches.Direction) {
                    '0' { 'R' }
                    '1' { 'D' }
                    '2' { 'L' }
                    '3' { 'U' }
                    Default { throw "Invalid direction code: $_" }
                }
            }
        }
        Default { throw "Invalid line: $_" }
    }

    area $instructions
}

function area ([Instruction[]] $instructions) {
    [int64] $result = 0

    $x = $y = 0
    $rowMap = @{}
    $colMap = @{}
    $instructions | ForEach-Object {
        $instruction = $_
        switch ($instruction.Direction) {
            'L' { $x_ = $x; $x -= $instruction.Length; $rowMap[$y] += , [Row]::new($y, $x, $x_) }
            'R' { $x_ = $x; $x += $instruction.Length; $rowMap[$y] += , [Row]::new($y, $x, $x_) }
            'U' { $y_ = $y; $y -= $instruction.Length; $colMap[$x] += , [Col]::new($x, $y, $y_) }
            'D' { $y_ = $y; $y += $instruction.Length; $colMap[$x] += , [Col]::new($x, $y, $y_) }
            Default { throw "Invalid direction: $_" }
        }
    }

    $rowKeys = $rowMap.Keys | Sort-Object
    $cols = $colMap.Values | ForEach-Object { $_ }

    function isCrossing ([Row] $r) {
        $cs = $colMap[$r.a]
        $c = $cs | Where-Object { $_.a -eq $r.y -or $_.b -eq $r.y }
        $a = $c.a -eq $r.y ? 1 : -1

        $cs = $colMap[$r.b]
        $c = $cs | Where-Object { $_.a -eq $r.y -or $_.b -eq $r.y }
        $b = $c.a -eq $r.y ? 1 : -1

        $a * $b -eq -1
    }

    for ($i = 0; $i -lt $rowKeys.Count - 1; $i++) {
        $y = [int64]$rowKeys[$i] + 1
        $h = [int64]$rowKeys[$i + 1] - $y
        if ($h -eq 0) {
            continue
        }

        $ranges = $cols
        | Where-Object { $_.a -lt $y -and $_.b -gt $y }
        | ForEach-Object { [Range]::new($_.x, $_.x) }
        | Sort-Object a

        [int64]$subresult = 0
        $inside = $false
        for ($j = 0; $j -lt $ranges.Count; $j++) {
            $r = $ranges[$j]
            $subresult += $r.b - $r.a + 1

            $inside = -not $inside
            if ($inside) {
                $subresult += $ranges[$j + 1].a - $r.b - 1
            }
        }

        $result += $h * $subresult
    }

    for ($i = 0; $i -lt $rowKeys.Count; $i++) {
        $y = $rowKeys[$i]

        $rows = $rowMap[$rowKeys[$i]]
        $ranges = $cols
        | Where-Object { $_.a -lt $y -and $_.b -gt $y }
        | ForEach-Object { [Range]::new($_.x, $_.x) }

        $ranges = ($rows + $ranges) | Sort-Object a

        [int64]$subresult = 0
        $inside = $false
        for ($j = 0; $j -lt $ranges.Count; $j++) {
            $r = $ranges[$j]
            $subresult += $r.b - $r.a + 1

            if ($r.a -eq $r.b -or (isCrossing $r)) {
                $inside = -not $inside
            }

            if ($inside) {
                $subresult += $ranges[$j + 1].a - $r.b - 1
            }
        }

        $result += $subresult
    }

    $result
}

class Instruction {
    [string] $Direction
    [int64] $Length
}

class Range {
    [int64] $a
    [int64] $b

    Range([int64] $a_, [int64] $b_) {
        $this.a = [math]::Min($a_, $b_)
        $this.b = [math]::Max($a_, $b_)
    }
}

class Row : Range {
    [int64]$y

    Row([int64] $y_, [int64] $a_, [int64] $b_) : base($a_, $b_) {
        $this.y = $y_
    }
}

class Col : Range {
    [int64]$x

    Col([int64] $x_, [int64] $a_, [int64] $b_) : base($a_, $b_) {
        $this.x = $x_
    }
}

# Get-Day18_1 # 70253
# Get-Day18_2 # 131265059885080
