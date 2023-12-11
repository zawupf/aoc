using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day11.txt"
$inputLines = Get-Content $inputFile
function Get-Day11_1 {
    part_1 $inputLines
}

function Get-Day11_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $universe = [Universe]::Parse($lines)
    $universe.DistanceSum(2)
}

function part_2 {
    param (
        [string[]] $lines
    )

    $universe = [Universe]::Parse($lines)
    $universe.DistanceSum(1000000)
}

class Pos {
    [int64] $X
    [int64] $Y

    Pos($x, $y) {
        $this.X = $x
        $this.Y = $y
    }
}

class Universe {
    [Pos[]] $Galaxies
    [HashSet[int64]] $OccupiedRows
    [HashSet[int64]] $OccupiedColumns

    [Universe] static Parse([string[]] $lines) {
        $universe = [Universe]@{
            OccupiedRows    = [HashSet[int64]]::new()
            OccupiedColumns = [HashSet[int64]]::new()
        }

        $universe.Galaxies = for ($y = 0; $y -lt $lines.Count; $y++) {
            $line = $lines[$y]
            for ($x = 0; $x -lt $line.Length; $x++) {
                if ($line[$x] -eq "#") {
                    $p = [Pos]::new($x, $y)
                    [void]$universe.OccupiedRows.Add($p.Y)
                    [void]$universe.OccupiedColumns.Add($p.X)
                    $p
                }
            }
        }

        return $universe
    }

    [int64] DistanceSum([int64] $expansion) {
        $n = $this.Galaxies.Count
        [int64]$sumDist = 0
        for ($i = 0; $i -lt $n - 1; $i++) {
            for ($j = $i + 1; $j -lt $n; $j++) {
                $d = $this.Distance($i, $j, $expansion)
                $sumDist += $d
            }
        }
        return $sumDist
    }

    [int64] Distance([int64] $i1, [int64] $i2, [int64] $expansion) {
        [Pos]$g1 = $this.Galaxies[$i1]
        [Pos]$g2 = $this.Galaxies[$i2]

        $a = [math]::Min($g1.X, $g2.X)
        $b = [math]::Max($g1.X, $g2.X)
        $dx = 0
        for ($i = $a + 1; $i -le $b; $i++) {
            $dx += $this.OccupiedColumns.Contains($i) ? 1 : $expansion
        }

        $a = [math]::Min($g1.Y, $g2.Y)
        $b = [math]::Max($g1.Y, $g2.Y)
        $dy = 0
        for ($i = $a + 1; $i -le $b; $i++) {
            $dy += $this.OccupiedRows.Contains($i) ? 1 : $expansion
        }

        $d = $dx + $dy
        return $d
    }
}

# Get-Day11_1 # 10494813
# Get-Day11_2 # 840988812853
