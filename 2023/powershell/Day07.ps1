using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day07.txt"
$inputLines = Get-Content $inputFile

function Get-Day07_1 {
    part_1 $inputLines
}

function Get-Day07_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    [Player[]] $players = $lines
    | ForEach-Object { [Player]::Parse($_) }
    | Sort-Object Sort1, Sort2

    $result = 0
    for ($i = 0; $i -lt $players.Count; $i++) {
        $result += ($i + 1) * $players[$i].Bid
    }
    $result
}

function part_2 {
    param (
        [string[]] $lines
    )

    [Player[]] $players = $lines
    | ForEach-Object { [Player]::ParseWithJoker($_) }
    | Sort-Object Sort1, Sort2

    $result = 0
    for ($i = 0; $i -lt $players.Count; $i++) {
        $result += ($i + 1) * $players[$i].Bid
    }
    $result
}

class Player {
    [string] $Hand
    [int64] $Bid
    [string] $Sort1
    [string] $Sort2

    [Player] static Parse([string] $value) {
        if ($value -match '(?<Hand>.{5})\s+(?<Bid>\d+)') {
            function getSort1 ([string] $hand) {
                $cards = @{}
                [char[]]$hand | ForEach-Object { ++$cards[$_] }
                -join ($cards.Values | Sort-Object -Descending)
            }

            function getSort2 ([string] $hand) {
                -join ([char[]]$hand | ForEach-Object { '{0:d2}' -f "23456789TJQKA".IndexOf($_) })
            }

            return [Player]@{
                Hand  = $Matches.Hand
                Bid   = $Matches.Bid
                Sort1 = getSort1 $Matches.Hand
                Sort2 = getSort2 $Matches.Hand
            }
        }
        throw "Invalid player input: $value"
    }

    [Player] static ParseWithJoker([string] $value) {
        if ($value -match '(?<Hand>.{5})\s+(?<Bid>\d+)') {
            function getSort1 ([string] $hand) {
                $cards = @{}
                [char[]]$hand | ForEach-Object { ++$cards[$_] }
                -join ($cards.Values | Sort-Object -Descending)
            }

            function getSort2 ([string] $hand) {
                -join ([char[]]$hand | ForEach-Object { '{0:d2}' -f "J23456789TQKA".IndexOf($_) })
            }

            if (-not $Matches.Hand.Contains("J")) {
                return [Player]@{
                    Hand  = $Matches.Hand
                    Bid   = $Matches.Bid
                    Sort1 = getSort1 $Matches.Hand
                    Sort2 = getSort2 $Matches.Hand
                }
            }
            else {
                $usedCards = [char[]]($Matches.Hand -replace "J", "") | Sort-Object -Unique
                if ($usedCards.Count -eq 0) {
                    $usedCards = [char[]]"A"
                }
                $players = $usedCards
                | ForEach-Object {
                    $h = $Matches.Hand -replace "J", $_
                    [Player]@{
                        Hand  = $h
                        Bid   = $Matches.Bid
                        Sort1 = getSort1 $h
                        Sort2 = getSort2 $Matches.Hand
                    }
                }
                | Sort-Object Sort1, Sort2
                return $players[-1]
            }
        }
        throw "Invalid player input: $value"
    }
}

# Get-Day07_1 # 249748283
# Get-Day07_2 # 248029057
