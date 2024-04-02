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
    # $deltas.Add([Tuple[int, int]]::new(1, 0))
    $stcoords = [HashSet[Tuple[int, int]]]::new()

    for ($y = 0; $y -lt $size; $y++) {
        $line = $lines[$y]
        for ($x = 0; $x -lt $size; $x++) {
            if ($line[$x] -eq '#') {
                [void]$rocks.Add([Tuple[int, int]]::new($x, $y))
            }
            elseif ($line[$x] -eq 'S') {
                [void]$coords.Add([Tuple[int, int]]::new($x, $y))
                # [void]$stcoords.Add([Tuple[int, int]]::new($x, $y))
            }
        }
    }

    while ($steps--) {
        $coords_ = [HashSet[Tuple[int, int]]]::new()
        $stcoords_ = [HashSet[Tuple[int, int]]]::new()
        foreach ($c in $coords) {
            for ($dx = -1; $dx -le 1; $dx += 2) {
                $x = $c.Item1 + $dx
                $y = $c.Item2
                $rx = $x % $size
                if ($rx -lt 0) { $rx += $size }
                $ry = $y % $size
                if ($ry -lt 0) { $ry += $size }
                $c_ = [Tuple[int, int]]::new($rx, $ry)
                if (-not $rocks.Contains($c_)) {
                    [void]$coords_.Add([Tuple[int, int]]::new($x, $y))
                    if (($x + 2 * $size) -ge 0 -and ($x + 2 * $size) -lt $size -and $y -ge 0 -and $y -lt $size) {
                        [void]$stcoords_.Add([Tuple[int, int]]::new($x, $y))
                    }
                }
            }
            for ($dy = -1; $dy -le 1; $dy += 2) {
                $x = $c.Item1
                $y = $c.Item2 + $dy
                $ry = $y % $size
                if ($ry -lt 0) { $ry += $size }
                $rx = $x % $size
                if ($rx -lt 0) { $rx += $size }
                $c_ = [Tuple[int, int]]::new($rx, $ry)
                if (-not $rocks.Contains($c_)) {
                    [void]$coords_.Add([Tuple[int, int]]::new($x, $y))
                    if (($x + 2 * $size) -ge 0 -and ($x + 2 * $size) -lt $size -and $y -ge 0 -and $y -lt $size) {
                        [void]$stcoords_.Add([Tuple[int, int]]::new($x, $y))
                    }
                }
            }
        }
        $deltas.Add([Tuple[int, int]]::new($stcoords_.Count, $stcoords_.Count - $stcoords.Count))
        $coords = $coords_
        $stcoords = $stcoords_
    }

    # 0,0-tile: 14th 42,39,...
    # -1s,0-tile: 22th(6) 42,39,...
    # -2s,0-tile: 37th(21) 42,39,...
    # +1s,0-tile: 26th(8) 42,39,...
    # +2s,0-tile: 39th(21) 42,39,...

    $result = $coords.Count
    $result
}

# Get-Day21_1 # 3782
# Get-Day21_2 # 143219569011526
