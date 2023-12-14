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

    $lines = transpose $lines
    $len = $lines[0].Length

    $lines
    | ForEach-Object {
        $m = [regex]::Matches($_, '(?:\.|(?<rock>O))+')
        $m | ForEach-Object {
            $startIndex = $_.Index
            $rockCount = $_.Groups['rock'].Captures.Count
            $n = $startIndex + $rockCount
            for ($i = $startIndex; $i -lt $n; $i++) {
                $load = $len - $i
                $load
            }
        }
    }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function part_2 {
    param (
        [string[]] $lines
    )

}

function transpose ([string[]] $lines) {
    $h = $lines.Count
    $w = $lines[0].Length
    $result = for ($x = 0; $x -lt $w; $x++) {
        $line = for ($y = 0; $y -lt $h; $y++) {
            $lines[$y][$x]
        }
        -join $line
    }
    $result
}

# Get-Day14_1 # 105623
# Get-Day14_2 # 840988812853
