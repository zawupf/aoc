using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day06.txt"
$inputLines = Get-Content $inputFile

function Get-Day06_1 {
    part_1 $inputLines
}

function Get-Day06_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    [int[]]$times = [regex]::Matches($lines[0], '\d+').Value
    [int[]]$distances = [regex]::Matches($lines[1], '\d+').Value

    $n = $times.Count
    if ($distances.Count -ne $n) { throw '$times and $distances must have same size' }

    $result = 1
    0..--$n
    | ForEach-Object {
        $time = $times[$_]
        $distance = $distances[$_]

        $wins = 0..$time
        | ForEach-Object {
            $timeHolding = $_
            $timeRacing = $time - $timeHolding
            $speed = $timeHolding
            $dist = $timeRacing * $speed
            $dist
        }
        | Where-Object { $_ -gt $distance }
        $result *= $wins.Count
    }
    $result
}

function part_2 {
    param (
        [string[]] $lines
    )

    [int64]$time = -join [regex]::Matches($lines[0], '\d+').Value
    [int64]$distance = -join [regex]::Matches($lines[1], '\d+').Value

    $wins = 0..$time
    | ForEach-Object {
        $timeHolding = $_
        $timeRacing = $time - $timeHolding
        $speed = $timeHolding
        $dist = $timeRacing * $speed
        $dist
    }
    | Where-Object { $_ -gt $distance }
    $wins.Count
}

# Get-Day06_1 # 861300
# Get-Day06_2 # 28101347
