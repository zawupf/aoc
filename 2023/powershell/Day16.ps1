using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day16.txt"
$inputLines = Get-Content $inputFile
function Get-Day16_1 {
    part_1 $inputLines
}

function Get-Day16_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    [char[][]]$cave = $lines | ForEach-Object { , [char[]]$_ }
    fire ([Beam]@{ x = 0; y = 0; d = [Direction]::Right }) $cave
}

function part_2 {
    param (
        [string[]] $lines
    )

    [char[][]]$cave = $lines | ForEach-Object { , [char[]]$_ }
    $w = $cave[0].Count
    $h = $cave.Count
    @(
        1..$w | ForEach-Object {
            fire ([Beam]@{ x = $_; y = 0; d = [Direction]::Down }) $cave
            fire ([Beam]@{ x = $_; y = $h - 1; d = [Direction]::Up }) $cave
        }
        1..$h | ForEach-Object {
            fire ([Beam]@{ x = 0; y = $_; d = [Direction]::Right }) $cave
            fire ([Beam]@{ x = $w - 1; y = $_; d = [Direction]::Left }) $cave
        }
    )
    | Measure-Object -Maximum
    | Select-Object -ExpandProperty Maximum
}

function fire ([Beam] $beam, [char[][]] $cave) {
    $w = $cave[0].Count
    $h = $cave.Count

    [Direction[][]]$state = 1..$h | ForEach-Object {
        , @(1..$w | ForEach-Object { [Direction]::None })
    }

    $beams = [Stack[Beam]]::new()
    $beams.Push($beam)
    while ($beams.Count) {
        [Beam]$beam = $beams.Pop()
        $x = $beam.x
        $y = $beam.y
        if ($x -lt 0 -or $x -ge $w -or $y -lt 0 -or $y -ge $h) {
            continue
        }

        $d = $beam.d
        $s = $state[$y][$x]

        if ($s -band $d) {
            continue
        }
        $s = $s -bor $d

        $c = $cave[$y][$x]
        switch ($c) {
            '\' {
                switch ($d) {
                    ([Direction]::Right) { $beams.Push($beam.Down()); $s = $s -bor [Direction]::Up }
                    ([Direction]::Left) { $beams.Push($beam.Up()); $s = $s -bor [Direction]::Down }
                    ([Direction]::Down) { $beams.Push($beam.Right()); $s = $s -bor [Direction]::Left }
                    ([Direction]::Up) { $beams.Push($beam.Left()); $s = $s -bor [Direction]::Right }
                    Default { throw "Invalid direction: $_" }
                }
            }
            '/' {
                switch ($d) {
                    ([Direction]::Right) { $beams.Push($beam.Up()); $s = $s -bor [Direction]::Down }
                    ([Direction]::Left) { $beams.Push($beam.Down()); $s = $s -bor [Direction]::Up }
                    ([Direction]::Down) { $beams.Push($beam.Left()); $s = $s -bor [Direction]::Right }
                    ([Direction]::Up) { $beams.Push($beam.Right()); $s = $s -bor [Direction]::Left }
                    Default { throw "Invalid direction: $_" }
                }
            }
            '|' {
                switch ($d) {
                    ([Direction]::Right) { $beams.Push($beam.Up()); $beams.Push($beam.Down()); $s = $s -bor [Direction]::Left }
                    ([Direction]::Left) { $beams.Push($beam.Up()); $beams.Push($beam.Down()); $s = $s -bor [Direction]::Right }
                    ([Direction]::Down) { $beams.Push($beam.Next()); $s = $s -bor [Direction]::Up }
                    ([Direction]::Up) { $beams.Push($beam.Next()); $s = $s -bor [Direction]::Down }
                    Default { throw "Invalid direction: $_" }
                }
            }
            '-' {
                switch ($d) {
                    ([Direction]::Right) { $beams.Push($beam.Next()); $s = $s -bor [Direction]::Left }
                    ([Direction]::Left) { $beams.Push($beam.Next()); $s = $s -bor [Direction]::Right }
                    ([Direction]::Down) { $beams.Push($beam.Left()); $beams.Push($beam.Right()); $s = $s -bor [Direction]::Up }
                    ([Direction]::Up) { $beams.Push($beam.Left()); $beams.Push($beam.Right()); $s = $s -bor [Direction]::Down }
                    Default { throw "Invalid direction: $_" }
                }
            }
            '.' {
                $beams.Push($beam.Next())
                switch ($d) {
                    ([Direction]::Right) { $s = $s -bor [Direction]::Left }
                    ([Direction]::Left) { $s = $s -bor [Direction]::Right }
                    ([Direction]::Down) { $s = $s -bor [Direction]::Up }
                    ([Direction]::Up) { $s = $s -bor [Direction]::Down }
                    Default { throw "Invalid direction: $_" }
                }
            }
            Default { throw "Invalid cave: $_" }
        }

        $state[$y][$x] = $s
    }

    $state
    | ForEach-Object { $_ }
    | Measure-Object -Sum { $_ -eq [Direction]::None ? 0 : 1 }
    | Select-Object -ExpandProperty Sum
}
enum Direction {
    None = 0
    Right = 1
    Left = 2
    Up = 4
    Down = 8
}

class Beam {
    [int] $x
    [int] $y
    [Direction] $d

    [Beam] Up() {
        return [Beam]@{x = $this.x; y = $this.y - 1; d = [Direction]::Up }
    }

    [Beam] Down() {
        return [Beam]@{x = $this.x; y = $this.y + 1; d = [Direction]::Down }
    }

    [Beam] Left() {
        return [Beam]@{x = $this.x - 1; y = $this.y; d = [Direction]::Left }
    }

    [Beam] Right() {
        return [Beam]@{x = $this.x + 1; y = $this.y; d = [Direction]::Right }
    }

    [Beam] Next() {
        $dx = switch ($this.d) {
            ([Direction]::Left) { -1 }
            ([Direction]::Right) { 1 }
            Default { 0 }
        }
        $dy = switch ($this.d) {
            ([Direction]::Up) { -1 }
            ([Direction]::Down) { 1 }
            Default { 0 }
        }
        return [Beam]@{
            x = $this.x + $dx
            y = $this.y + $dy
            d = $this.d
        }
    }
}

# Get-Day16_1 # 8112
# Get-Day16_2 # 8314
