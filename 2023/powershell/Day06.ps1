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
    for ($i = 0; $i -lt $n; $i++) {
        $result *= winCount $times[$i] $distances[$i]
    }
    $result
}

function part_2 {
    param (
        [string[]] $lines
    )

    [int64]$time = -join [regex]::Matches($lines[0], '\d+').Value
    [int64]$distance = -join [regex]::Matches($lines[1], '\d+').Value

    winCount $time $distance
}

function winCount ([int64]$time, [int64]$distance) {
    function binaryFindSwitchTime([int64]$a, [int64]$b, [int64]$time, [scriptblock]$condition) {
        do {
            [int64]$i = ($a + $b) / 2
            if (&$condition (($time - $i) * $i)) {
                $b = $i
            }
            else {
                $a = $i
            }
        } while ( $b - $a -gt 1 )
        $b
    }

    $first = binaryFindSwitchTime 0 ($time / 2) $time { param($d) $d -gt $distance }
    $last = binaryFindSwitchTime ($time / 2) $time $time { param($d) $d -le $distance }

    $last - $first
}

# Get-Day06_1 # 861300
# Get-Day06_2 # 28101347
