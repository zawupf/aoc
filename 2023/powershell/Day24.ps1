using namespace System
using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day24.txt"
$inputLines = Get-Content $inputFile

function Get-Day24_1 {
    part_1 $inputLines 200000000000000 400000000000000
}

function Get-Day24_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines,
        [int64] $aMin,
        [int64] $aMax
    )

    $particles = switch -Regex ($lines) {
        '^(?<PX>-?\d+), +(?<PY>-?\d+), +(?<PZ>-?\d+) +@ +(?<VX>-?\d+), +(?<VY>-?\d+), +(?<VZ>-?\d+)$' {
            [Line]::new(
                [Vec]::new($Matches.PX, $Matches.PY, $Matches.PZ),
                [Vec]::new($Matches.VX, $Matches.VY, $Matches.VZ)
            )
        }
        Default { throw "Invalid line: $_" }
    }

    $points = for ($i = 0; $i -lt $particles.Count - 1; $i++) {
        for ($j = $i + 1; $j -lt $particles.Count; $j++) {
            $p1 = $particles[$i]
            $p2 = $particles[$j]

            # Calculate the denominator
            $denom = $p1.V.X * $p2.V.Y - $p1.V.Y * $p2.V.X

            # Check if the lines are parallel (or identical)
            if ($denom -eq 0) {
                continue
            }

            # Calculate s and t
            $s = (($p2.P.X - $p1.P.X) * $p2.V.Y - ($p2.P.Y - $p1.P.Y) * $p2.V.X) / $denom
            $t = (($p2.P.X - $p1.P.X) * $p1.V.Y - ($p2.P.Y - $p1.P.Y) * $p1.V.X) / $denom
            if ($s -lt 0.0 -or $t -lt 0.0) {
                continue
            }

            , @(($p1.P.X + $s * $p1.V.X), ($p1.P.Y + $s * $p1.V.Y))
        }
    }

    $result = $points | Where-Object {
        $x, $y = $_
        $x -ge $aMin -and $x -le $aMax -and $y -ge $aMin -and $y -le $aMax
    }
    $result.Count
}

function part_2 {
    param (
        [string[]] $lines
    )

    0
}

class Vec : Tuple[Int64, Int64, Int64] {
    Vec([Int64]$x, [Int64]$y, [Int64]$z) : base($x, $y, $z) {}

    static  Vec() {
        "Vec" | Update-TypeData -MemberName X -MemberType AliasProperty -Value Item1 -Force
        "Vec" | Update-TypeData -MemberName Y -MemberType AliasProperty -Value Item2 -Force
        "Vec" | Update-TypeData -MemberName Z -MemberType AliasProperty -Value Item3 -Force
    }
}

class Line : Tuple[psobject, psobject] {
    Line([Vec]$p, [Vec]$v) : base($p, $v) {}

    static  Line() {
        "Line" | Update-TypeData -MemberName P -MemberType AliasProperty -Value Item1 -Force
        "Line" | Update-TypeData -MemberName V -MemberType AliasProperty -Value Item2 -Force
    }
}

# Get-Day24_1 # 16727
# Get-Day24_2 # 143219569011526
