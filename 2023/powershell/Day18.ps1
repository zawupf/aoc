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

    $grid = digOutline $lines
    digInner $grid
    countHoles $grid
}

function part_2 {
    param (
        [string[]] $lines
    )

    stalkAround $lines 3 10
}

function digOutline ([string[]] $lines) {
    $x = $y = $minx = $miny = $maxx = $maxy = 0

    $digs = switch -Regex ($lines) {
        '(?<Direction>[LRUD]) (?<Length>\d+) \((?<Color>#[0-9a-f]{6})\)' {
            $dig = $Matches | Select-Object Direction, Length, Color
            $dig

            switch ($dig.Direction) {
                'L' { $x -= $dig.Length }
                'R' { $x += $dig.Length }
                'U' { $y -= $dig.Length }
                'D' { $y += $dig.Length }
                Default {}
            }

            $minx = [math]::Min($minx, $x)
            $maxx = [math]::Max($maxx, $x)
            $miny = [math]::Min($miny, $y)
            $maxy = [math]::Max($maxy, $y)
        }
        Default { throw "Invalid line: $_" }
    }

    $width = $maxx - $minx + 1
    $height = $maxy - $miny + 1
    [string[][]]$grid = 1..$height | ForEach-Object { , @(@(".") * $width) }

    $x = - $minx
    $y = - $miny
    $grid[$y][$x] = '#'

    $digs | ForEach-Object {
        $dig = $_
        switch ($dig.Direction) {
            'R' { 1..$dig.Length | ForEach-Object { $grid[$y][++$x] = $dig.Color } }
            'L' { 1..$dig.Length | ForEach-Object { $grid[$y][--$x] = $dig.Color } }
            'D' { 1..$dig.Length | ForEach-Object { $grid[++$y][$x] = $dig.Color } }
            'U' { 1..$dig.Length | ForEach-Object { $grid[--$y][$x] = $dig.Color } }
            Default { throw "Invalid direction: $_" }
        }
    }

    $grid
}

function digInner ([string[][]] $grid) {
    $w = $grid[0].Count
    $h = $grid.Count
    [string[][]]$innerGrid = 1..$h | ForEach-Object { , @(@(".") * $w) }

    for ($y = 1; $y -le ($h - 2); $y++) {
        $inside = $false
        $row = $grid[$y]
        $innerRow = $innerGrid[$y]
        for ($x = 0; $x -lt $w; $x++) {
            if ($row[$x][0] -eq '.') {
                if ($inside) {
                    $innerRow[$x] = '#'
                }
            }
            else {
                $x_ = $x
                while ($x_ -lt $w -and $row[++$x_] -ne '.') {}
                --$x_
                $corners = -join @(
                    $grid[$y - 1][$x].Substring(0, 1)
                    $grid[$y - 1][$x_].Substring(0, 1)
                    $grid[$y + 1][$x].Substring(0, 1)
                    $grid[$y + 1][$x_].Substring(0, 1)
                )
                switch ($corners) {
                    '####' { $inside = -not $inside }
                    '#..#' { $inside = -not $inside }
                    '.##.' { $inside = -not $inside }
                    '##..' {}
                    '..##' {}
                    Default { throw "Invalid corners: $_" }
                }
                $x = $x_
            }
        }
    }

    for ($y = 0; $y -lt $h; $y++) {
        for ($x = 0; $x -lt $w; $x++) {
            if ($innerGrid[$y][$x] -ne '.') {
                $grid[$y][$x] = $innerGrid[$y][$x]
            }
        }
    }
}

function countHoles ([string[][]] $grid) {
    $grid
    | ForEach-Object { $_ }
    | Measure-Object -Sum { $_.StartsWith('#') ? 1 : 0 }
    | Select-Object -ExpandProperty Sum
}

function dump ([string[][]] $grid) {
    Write-Host ""
    $grid | ForEach-Object { Write-Host ( -join ($_.Substring(0, 1))) }
}

# Get-Day18_1 # 70253
# Get-Day18_2 # 1268
