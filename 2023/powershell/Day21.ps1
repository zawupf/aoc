using namespace System
using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day21.txt"
$inputLines = Get-Content $inputFile
function Get-Day21_1 {
    part_1 $inputLines 64
}

function Get-Day21_2 {
    part_2 $inputLines 26501365
}

function part_1 {
    param (
        [string[]] $lines,
        [int] $steps
    )

    $width = $lines[0].Length
    $height = $lines.Count

    $coords = [HashSet[Tuple[int, int]]]::new()
    $rocks = [HashSet[Tuple[int, int]]]::new()

    for ($y = 0; $y -lt $height; $y++) {
        $line = $lines[$y]
        for ($x = 0; $x -lt $width; $x++) {
            if ($line[$x] -eq '#') {
                [void]$rocks.Add([Tuple[int, int]]::new($x, $y))
            }
            elseif ($line[$x] -eq 'S') {
                [void]$coords.Add([Tuple[int, int]]::new($x, $y))
            }
        }
    }

    while ($steps--) {
        $coords_ = [HashSet[Tuple[int, int]]]::new()
        foreach ($c in $coords) {
            for ($dx = -1; $dx -le 1; $dx += 2) {
                $x = $c.Item1 + $dx
                if ($x -lt 0 -or $x -ge $width) { continue }
                $c_ = [Tuple[int, int]]::new($x, $c.Item2)
                if (-not $rocks.Contains($c_)) {
                    [void]$coords_.Add($c_)
                }
            }
            for ($dy = -1; $dy -le 1; $dy += 2) {
                $y = $c.Item2 + $dy
                if ($y -lt 0 -or $y -ge $height) { continue }
                $c_ = [Tuple[int, int]]::new($c.Item1, $y)
                if (-not $rocks.Contains($c_)) {
                    [void]$coords_.Add($c_)
                }
            }
        }
        $coords = $coords_
    }

    $result = $coords.Count
    $result
}

function part_2 {
    param (
        [string[]] $lines,
        [int] $steps
    )

    $size = $lines.Count
    if ($size -ne $lines[0].Length) {
        throw "Input must be a square"
    }

    $coords = [HashSet[Tuple[int, int]]]::new()
    $rocks = [HashSet[Tuple[int, int]]]::new()
    $deltas = [List[Tuple[int, int]]]::new()
    $deltas.Add([Tuple[int, int]]::new(1, 0))

    for ($y = 0; $y -lt $size; $y++) {
        $line = $lines[$y]
        for ($x = 0; $x -lt $size; $x++) {
            if ($line[$x] -eq '#') {
                [void]$rocks.Add([Tuple[int, int]]::new($x, $y))
            }
            elseif ($line[$x] -eq 'S') {
                [void]$coords.Add([Tuple[int, int]]::new($x, $y))
            }
        }
    }

    while ($steps--) {
        $coords_ = [HashSet[Tuple[int, int]]]::new()
        foreach ($c in $coords) {
            for ($dx = -1; $dx -le 1; $dx += 2) {
                $x = $c.Item1 + $dx
                $rx = $x % $size
                if ($rx -lt 0) { $rx += $size }
                $ry = $c.Item2 % $size
                if ($ry -lt 0) { $ry += $size }
                $c_ = [Tuple[int, int]]::new($rx, $ry)
                if (-not $rocks.Contains($c_)) {
                    [void]$coords_.Add([Tuple[int, int]]::new($x, $c.Item2))
                }
            }
            for ($dy = -1; $dy -le 1; $dy += 2) {
                $y = $c.Item2 + $dy
                $ry = $y % $size
                if ($ry -lt 0) { $ry += $size }
                $rx = $c.Item1 % $size
                if ($rx -lt 0) { $rx += $size }
                $c_ = [Tuple[int, int]]::new($rx, $ry)
                if (-not $rocks.Contains($c_)) {
                    [void]$coords_.Add([Tuple[int, int]]::new($c.Item1, $y))
                }
            }
        }
        $deltas.Add([Tuple[int, int]]::new($coords_.Count, $coords_.Count - $coords.Count))
        $coords = $coords_
    }

    $result = $coords.Count
    $result
}

# Get-Day21_1 # 3782
# Get-Day21_2 # 143219569011526
