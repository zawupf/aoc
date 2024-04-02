using namespace System
using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day25.txt"
$inputLines = Get-Content $inputFile

function Get-Day25_1 {
    part_1 $inputLines
}

function Get-Day25_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $graph = @{}

    $lines | ForEach-Object {
        $m = [regex]::Match($_, '(?<Left>\w+):( (?<Right>\w+))+')
        if (-not $m.Success) {
            throw "Invalid line: $_"
        }

        $left = $m.Groups['Left'].Value
        [string[]] $rights = $m.Groups['Right'].Captures.Value
        foreach ($right in $rights) {
            $graph[$left] += , $right
            $graph[$right] += , $left
        }
    }

    0
}

function part_2 {
    param (
        [string[]] $lines
    )

    0
}

# Get-Day25_1 # 3782
# Get-Day25_2 # 143219569011526
