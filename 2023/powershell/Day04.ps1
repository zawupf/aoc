using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day04.txt"
$inputLines = Get-Content $inputFile

function Get-Day04_1 {
    part_1 $inputLines
}

function Get-Day04_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $Lines
    )

    $Lines
    | ForEach-Object { [Card]::Parse($_) }
    | Measure-Object -Sum {
        $n = $_.OwnedWinningNumbers().Count
        $n ? 1 -shl ($n - 1) : 0
    }
    | Select-Object -ExpandProperty Sum
}

function part_2 {
    param (
        [string[]] $Lines
    )

    $cards = $Lines | ForEach-Object { [Card]::Parse($_) }
    for ($i = 0; $i -lt $cards.Count; $i++) {
        $card = $cards[$i]
        $n = $card.OwnedWinningNumbers().Count
        $m = [math]::Min($i + $n, $cards.Count - 1)
        for ($j = $i + 1; $j -le $m; $j++) {
            $cards[$j].Count += $card.Count
        }
    }

    ($cards | Measure-Object -Sum -Property Count).Sum
}

class Card {
    [int] $ID
    [HashSet[int]] $WinningNumbers
    [HashSet[int]] $OwnedNumbers
    [int] $Count

    [int[]] OwnedWinningNumbers() {
        $result = [HashSet[int]]::new($this.WinningNumbers)
        $result.IntersectWith($this.OwnedNumbers)
        return $result
    }

    [Card] static Parse([string] $line) {
        $m = [regex]::Match($line, 'Card\s+(?<ID>\d+):(?:\s+(?<Winning>\d+))+\s+\|(?:\s+(?<Owned>\d+))+')
        if (-not $m.Success) {
            throw "Invalid card input: $line"
        }

        return [Card]@{
            ID             = $m.Groups["ID"].Value
            WinningNumbers = [HashSet[int]]$m.Groups["Winning"].Captures.Value
            OwnedNumbers   = [HashSet[int]]$m.Groups["Owned"].Captures.Value
            Count          = 1
        }
    }
}

# part_1 (@"
# Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
# Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
# Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
# Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
# Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
# Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
# "@ -split "`n")

# Get-Day04_1 # 21138
# Get-Day04_2 # 7185540
