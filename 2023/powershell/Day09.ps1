using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day09.txt"
$inputLines = Get-Content $inputFile

function Get-Day09_1 {
    part_1 $inputLines
}

function Get-Day09_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $lines
    | ForEach-Object {
        [int[]]$numbers = -split $_
        resume $numbers
    }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function part_2 {
    param (
        [string[]] $lines
    )

    $lines
    | ForEach-Object {
        [int[]]$numbers = -split $_
        resume $numbers -First
    }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function resume ([int[]] $numbers, [switch] $first = $false) {
    function diffs ([int[]] $numbers) {
        1..($numbers.Count - 1) | ForEach-Object { $numbers[$_] - $numbers[$_ - 1] }
    }

    function isDone ([int[]] $numbers) {
        for ($i = 0; $i -lt $numbers.Count; $i++) {
            if ($numbers[$i] -ne 0) {
                return $false
            }
        }
        return $true
    }

    $result = @(, $numbers)
    while (-not (isDone $numbers)) {
        $numbers = diffs $numbers
        $result += , $numbers
    }

    if ($first) {
        $result = $result | ForEach-Object { $_[0] }
        $acc = 0
        for ($i = $result.Count - 1; $i -ge 0; $i--) {
            $acc = $result[$i] - $acc
        }
        $acc
    }
    else {
        $result
        | ForEach-Object { $_[-1] }
        | Measure-Object -Sum
        | Select-Object -ExpandProperty Sum
    }
}

# Get-Day09_1 # 1953784198
# Get-Day09_2 # 957
