using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day13.txt"
$inputLines = Get-Content $inputFile
function Get-Day13_1 {
    part_1 $inputLines
}

function Get-Day13_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $lines
    | ForEach-Object { $_.Length }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function part_2 {
    param (
        [string[]] $lines
    )

    $lines
    | ForEach-Object { $_.Length }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

# Get-Day13_1 # 8419
# Get-Day13_2 # 840988812853
