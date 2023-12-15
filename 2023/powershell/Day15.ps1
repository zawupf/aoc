using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day15.txt"
$inputLines = Get-Content $inputFile
function Get-Day15_1 {
    part_1 $inputLines
}

function Get-Day15_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    ($lines -join ",").Trim(",") -split ","
    | ForEach-Object { Get-Hash $_ }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function part_2 {
    param (
        [string[]] $lines
    )

    [ordered[]]$boxes = 0..255 | ForEach-Object { [ordered]@{} }
    ($lines -join ",").Trim(",") -split ","
    | ForEach-Object {
        switch -Regex ($_) {
            '(?<Label>.+?)=(?<FocalLength>\d+)' {
                $label = $Matches.Label
                $focalLength = $Matches.FocalLength -as [int]
                $i = Get-Hash $label
                $boxes[$i][$label] = $focalLength
            }
            '(?<Label>.+?)-' {
                $label = $Matches.Label
                $i = Get-Hash $label
                $boxes[$i].Remove($label)
            }
            Default { throw "Invalid input: $_" }
        }
    }

    1..256 | ForEach-Object {
        $box = $_
        $focalLengths = $boxes[$box - 1].Values
        1..$focalLengths.Count | ForEach-Object {
            $slot = $_
            $box * $slot * $focalLengths[$slot - 1]
        }
    }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function Get-Hash ([string] $value) {
    $hash = 0
    [char[]]$value | ForEach-Object { $hash = (($hash + [char]$_) * 17) -band 255 }
    $hash
}

# Get-Day15_1 # 514025
# Get-Day15_2 # 244461
