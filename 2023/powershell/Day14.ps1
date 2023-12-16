using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day14.txt"
$inputLines = Get-Content $inputFile
function Get-Day14_1 {
    part_1 $inputLines
}

function Get-Day14_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    [char[][]]$grid = $lines | ForEach-Object { , [char[]]$_ }
    tiltNorth $grid
    load $grid
}

function part_2 {
    param (
        [string[]] $lines
    )

    [char[][]]$grid = $lines | ForEach-Object { , [char[]]$_ }
    $state = [ordered]@{}
    $state[(key $grid)] = load $grid
    while ($true) {
        tiltNorth $grid
        tiltWest $grid
        tiltSouth $grid
        tiltEast $grid
        $l = load $grid
        $k = key $grid
        if ($state.Contains($k)) {
            [string[]]$keys = $state.Keys
            $offset = $keys.IndexOf($k)
            $repeat = $keys.Count - $offset
            break
        }
        $state[$k] = $l
    }

    $n = (1000000000 - $offset) % $repeat
    # Write-Host "first, second: $first, $second"
    $state.Values[$offset + $n]
}

function dump ([char[][]] $grid) {
    $h = $grid.Count
    $s = for ($y = 0; $y -lt $h; $y++) {
        -join $grid[$y]
    }
    $s = $s -join "`n"
    Write-Host "$s`n"
}

function key ([char[][]] $grid) {
    $h = $grid.Count
    $s = for ($y = 0; $y -lt $h; $y++) {
        -join $grid[$y]
    }
    -join $s
}

function load ([char[][]] $grid) {
    $w = $grid[0].Count
    $h = $grid.Count

    @(
        for ($x = 0; $x -lt $w; $x += 1) {
            for ($y = 0; $y -lt $h; $y += 1) {
                $c = $grid[$y][$x]
                if ($c -eq 'O') {
                    $h - $y
                }
            }
        }
    )
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function tiltNorth ([char[][]] $grid) {
    $w = $grid[0].Count
    $h = $grid.Count

    for ($x = 0; $x -lt $w; $x += 1) {
        $i = -1
        for ($y = 0; $y -lt $h; $y += 1) {
            $c = $grid[$y][$x]
            if ($c -eq '#') {
                $i = -1
                continue
            }

            if ($c -eq '.') {
                $i = $i -eq -1 ? $y : $i
            }
            elseif ($i -ge 0) {
                $grid[$i++][$x] = 'O'
                $grid[$y][$x] = '.'
            }
        }
    }
}

function tiltSouth ([char[][]] $grid) {
    $w = $grid[0].Count
    $h = $grid.Count

    for ($x = 0; $x -lt $w; $x += 1) {
        $i = -1
        for ($y = $h - 1; $y -ge 0; $y -= 1) {
            $c = $grid[$y][$x]
            if ($c -eq '#') {
                $i = -1
                continue
            }

            if ($c -eq '.') {
                $i = $i -eq -1 ? $y : $i
            }
            elseif ($i -ge 0) {
                $grid[$i--][$x] = 'O'
                $grid[$y][$x] = '.'
            }
        }
    }
}

function tiltWest ([char[][]] $grid) {
    $w = $grid[0].Count
    $h = $grid.Count

    for ($y = 0; $y -lt $h; $y += 1) {
        $i = -1
        for ($x = 0; $x -lt $w; $x += 1) {
            $c = $grid[$y][$x]
            if ($c -eq '#') {
                $i = -1
                continue
            }

            if ($c -eq '.') {
                $i = $i -eq -1 ? $x : $i
            }
            elseif ($i -ge 0) {
                $grid[$y][$i++] = 'O'
                $grid[$y][$x] = '.'
            }
        }
    }
}

function tiltEast ([char[][]] $grid) {
    $w = $grid[0].Count
    $h = $grid.Count

    for ($y = 0; $y -lt $h; $y += 1) {
        $i = -1
        for ($x = $w - 1; $x -ge 0; $x -= 1) {
            $c = $grid[$y][$x]
            if ($c -eq '#') {
                $i = -1
                continue
            }

            if ($c -eq '.') {
                $i = $i -eq -1 ? $x : $i
            }
            elseif ($i -ge 0) {
                $grid[$y][$i--] = 'O'
                $grid[$y][$x] = '.'
            }
        }
    }
}

# Get-Day14_1 # 105623
# Get-Day14_2 # 98029
