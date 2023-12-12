using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day12.txt"
$inputLines = Get-Content $inputFile
function Get-Day12_1 {
    part_1 $inputLines
}

function Get-Day12_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $lines
    | ForEach-Object { arrangementCountWithRepeat $_ 1 }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function part_2 {
    param (
        [string[]] $lines
    )

    $lines
    | ForEach-Object { arrangementCountWithRepeat $_ 5 }
    | Measure-Object -Sum
    | Select-Object -ExpandProperty Sum
}

function arrangementCount ([string] $line) {
    $m = [regex]::Match($line, '^(?<SpringsPattern>[#.?]+)(?:[ ,](?<Number>\d+))+$')
    if (-not $m.Success) {
        throw "Invalid line: $line"
    }

    $springsPattern = $m.Groups['SpringsPattern'].Value
    [int[]]$brokenCounts = $m.Groups['Number'].Captures.Value
    [string]$brokenCountsString = $brokenCounts -join ","

    $m = [regex]::Matches($springsPattern, '\?')
    if (-not $m.Success) {
        throw "Invalid springs pattern: $springsPattern"
    }

    [int[]]$indexes = $m.Index
    [char[]]$chars = $springsPattern

    [int] $result = 0

    $n = 1 -shl $indexes.Count
    for ($i = 0; $i -lt $n; $i++) {
        for ($j = 0; $j -lt $indexes.Count; $j++) {
            $chars[$indexes[$j]] = $i -band (1 -shl $j) ? "#" : "."
        }

        $s = -join $chars
        $m = [regex]::Matches($s, '#+')
        if ($m.Success) {
            [string]$counts = [int[]]$m.Length -join ","
            if ($counts -eq $brokenCountsString) {
                ++$result
            }
        }
    }

    $result
}

function arrangementCountWithRepeat ([string] $line, [int] $repeat) {
    $m = [regex]::Match($line, '^(?<Springs>[#.?]+)(?:[ ,](?<Number>\d+))+$')
    if (-not $m.Success) {
        throw "Invalid line: $line"
    }

    $springs = $m.Groups['Springs'].Value
    $springs = 1..$repeat | ForEach-Object { , $springs }
    $springs = ($springs -join "?") + "."
    [string]$springs = $springs -replace '\.\.+', '.'

    [int[]]$counts = $m.Groups['Number'].Captures.Value
    $counts = 1..$repeat | ForEach-Object { $counts }

    $states = [Stack[pscustomobject]]::new()
    $states.Push(
        [PSCustomObject]@{
            Springs = $springs
            Counts  = $counts
        }
    )

    [int64] $result = 0
    while ($states.Count -ne 0) {
        $state = $states.Pop()

        $springs = $state.Springs
        $counts = $state.Counts
        if ($counts.Count -eq 0 -and ($springs.Length -eq 0 -or $springs -cmatch '^[.?]*$')) {
            ++$result
            continue
        }
        if ($counts.Count -eq 0 -or $springs.Length -eq 0) {
            continue
        }

        if ($springs[0] -cmatch '[.?]') {
            $states.Push(
                [PSCustomObject]@{
                    Springs = $springs.Substring(1)
                    Counts  = $counts
                }
            )
        }

        $count = $counts[0]

        $search = "#" * $count + "."
        if ($search.Length -gt $springs.Length) {
            continue
        }

        $pattern = $springs.Substring(0, $search.Length)
        if ($search -clike $pattern) {
            $n = $counts.Count - 1
            $counts = $n -ge 1 ? $counts[1..$n] : @()
            $states.Push(
                [PSCustomObject]@{
                    Springs = $springs.Substring($search.Length)
                    Counts  = $counts
                }
            )
        }
    }

    $result
}

# Get-Day12_1 # 8419
# Get-Day12_2 # 840988812853
