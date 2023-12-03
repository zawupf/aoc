$inputFile = "$PSScriptRoot\..\_inputs\Day03.txt"
$inputLines = Get-Content $inputFile

function Get-Day03_1 {
    part_1 $inputLines
}

function Get-Day03_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $Lines
    )

    $sum = 0
    for ($y = 0; $y -lt $Lines.Count; $y++) {
        foreach ($match in [regex]::Matches($Lines[$y], '\d+')) {
            $symbols = -join (adjacentPositions $match.Index $y $match.Length $Lines | ForEach-Object { $Lines[$_.y][$_.x] })
            if ($symbols -match '[^.\d]') {
                $sum += [int]$match.Value
            }
        }
    }
    $sum
}

function part_2 {
    param (
        [string[]] $Lines
    )

    $gears = @{}
    for ($y = 0; $y -lt $Lines.Count; $y++) {
        foreach ($match in [regex]::Matches($Lines[$y], '\d+')) {
            adjacentPositions $match.Index $y $match.Length $Lines
            | Where-Object { $Lines[$_.y][$_.x] -EQ "*" }
            | ForEach-Object { $gears["$($_.x),$($_.y)"] += , [int]$match.Value }
        }
    }

    $gears.Values
    | Measure-Object -Sum { $_.Count -eq 2 ? $_[0] * $_[1]:0 }
    | Select-Object -ExpandProperty Sum
}

function adjacentPositions ($x, $y, $length, $lines) {
    @(
        ($x - 1)..($x + $length) | ForEach-Object { [PSCustomObject]@{ x = $_; y = $y - 1 } }
        ($x - 1)..($x + $length) | ForEach-Object { [PSCustomObject]@{ x = $_; y = $y + 1 } }
        [PSCustomObject]@{ x = $x - 1; y = $y }
        [PSCustomObject]@{ x = $x + $length; y = $y }
    )
    | Where-Object { $_.x -ge 0 -and $_.x -lt $lines[0].Length -and $_.y -ge 0 -and $_.y -lt $lines.Count }
}

# Get-Day03_1 # 557705
# Get-Day03_2 # 84266818
