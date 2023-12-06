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

    $m = $time / 2
    $d = ($time - $m) * $m
    if ($d -le $distance) { throw "Oink" }

    [int64]$a = 0
    $b = $m
    do {
        [int64]$i = [math]::Floor(($a + $b) / 2)
        $d = ($time - $i) * $i
        if ($d -gt $distance) {
            $b = $i
        }
        else {
            $a = $i
        }
    } while ( $b - $a -gt 1 )
    $first = $b

    [int64]$a = $m
    $b = $time
    do {
        [int64]$i = [math]::Floor(($a + $b) / 2)
        $d = ($time - $i) * $i
        if ($d -gt $distance) {
            $a = $i
        }
        else {
            $b = $i
        }
    } while ( $b - $a -gt 1 )
    $last = $b

    $last - $first
}

# Get-Day06_1 # 861300
# Get-Day06_2 # 28101347
