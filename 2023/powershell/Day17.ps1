using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day17.txt"
$inputLines = Get-Content $inputFile
function Get-Day17_1 {
    part_1 $inputLines
}

function Get-Day17_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    stalkAround $lines 0 3
}

function part_2 {
    param (
        [string[]] $lines
    )

    stalkAround $lines 3 10
}

function stalkAround {
    param (
        [string[]] $lines,
        [int] $noStopLimit,
        [int] $wobbleLimit
    )

    [int[][]]$grid = $lines | ForEach-Object { , ($_ -as [char[]] -as [string[]] -as [int[]]) }
    $w = $grid[0].Count
    $h = $grid.Count

    [int[][][]]$heatlossState = 1..$h | ForEach-Object {
        , @(1..$w | ForEach-Object { , @([int]::MaxValue, [int]::MaxValue) })
    }

    [Stack[Cursor]]$cursors = [Stack[Cursor]]::new()
    $cursors.Push([Cursor]@{ x = 0; y = 0; dx = 0; dy = 1; heatloss = 0 })
    $cursors.Push([Cursor]@{ x = 0; y = 0; dx = 1; dy = 0; heatloss = 0 })

    while ($cursors.Count) {
        $c = $cursors.Pop()
        $j = $c.dx ? 0 : 1

        for ($i = 0; $i -lt $wobbleLimit; $i++) {
            $c = $c.Next($grid, $w, $h)
            if ($null -eq $c) {
                break
            }

            if ($i -lt $noStopLimit) {
                continue
            }

            if ($c.heatloss -ge $heatlossState[-1][-1][$j]) {
                break
            }

            if ($c.heatloss -ge $heatlossState[$c.y][$c.x][$j]) {
                continue
            }

            $heatlossState[$c.y][$c.x][$j] = $c.heatloss

            if ($c.x -eq ($w - 1) -and $c.y -eq ($h - 1)) {
                break
            }

            if ($c.dx) {
                $cursors.Push($c.WithDY(-1))
                $cursors.Push($c.WithDY(1))
            }
            else {
                $cursors.Push($c.WithDX(-1))
                $cursors.Push($c.WithDX(1))
            }
        }
    }

    ($heatlossState[-1][-1] | Measure-Object -Minimum).Minimum
}

class Cursor {
    [int] $x
    [int] $y
    [int] $dx
    [int] $dy
    [int] $heatloss

    [Cursor] Next([int[][]] $grid, [int] $w, [int] $h) {
        $x_ = $this.x + $this.dx
        $y_ = $this.y + $this.dy
        if ($x_ -ge 0 -and $x_ -lt $w -and
            $y_ -ge 0 -and $y_ -lt $h) {
            return [Cursor]@{
                x = $x_; y = $y_
                dx = $this.dx; dy = $this.dy
                heatloss = $this.heatloss + $grid[$y_][$x_]
            }
        }
        return $null
    }

    [Cursor] WithDX([int] $dx_) {
        return [Cursor]@{
            x = $this.x; y = $this.y
            dx = $dx_; dy = 0
            heatloss = $this.heatloss
        }
    }

    [Cursor] WithDY([int] $dy_) {
        return [Cursor]@{
            x = $this.x; y = $this.y
            dx = 0; dy = $dy_
            heatloss = $this.heatloss
        }
    }
}

# Get-Day17_1 # 1128
# Get-Day17_2 # 1268
