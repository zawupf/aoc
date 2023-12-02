$inputFile = "$PSScriptRoot\..\_inputs\Day02.txt"
$inputLines = Get-Content $inputFile

function Get-Day02_1 {
    part_1 $inputLines ([CubeSet]::Parse("12 red cubes, 13 green cubes, and 14 blue cubes"))
}

function Get-Day02_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $Lines,
        [CubeSet] $Bag
    )

    $Lines
    | ForEach-Object { [Game]::Parse($_) }
    | Where-Object { $_.IsPossibleWithBag($Bag) }
    | Measure-Object -Sum -Property ID
    | Select-Object -ExpandProperty Sum
}

function part_2 {
    param (
        [string[]] $Lines
    )

    $games = $Lines | ForEach-Object { [Game]::Parse($_) }
    ($games.MinBag().Power() | Measure-Object -Sum).Sum
}

class CubeSet {
    [int]$RedCount
    [int]$GreenCount
    [int]$BlueCount

    [CubeSet] static Parse([string] $value) {
        $cubeSet = [CubeSet]::new()
        $colors = $value -split ","
        switch -Regex ($colors) {
            '(?<Num>\d+)\s+red' { $cubeSet.RedCount = $Matches.Num }
            '(?<Num>\d+)\s+green' { $cubeSet.GreenCount = $Matches.Num }
            '(?<Num>\d+)\s+blue' { $cubeSet.BlueCount = $Matches.Num }
            Default { throw "Invalid CubeSet: $value" }
        }
        return $cubeSet
    }

    [bool] IsSubSet([CubeSet] $cs) {
        return (
            $cs.RedCount -le $this.RedCount -and
            $cs.GreenCount -le $this.GreenCount -and
            $cs.BlueCount -le $this.BlueCount
        )
    }

    [int] Power() {
        return $this.RedCount * $this.GreenCount * $this.BlueCount
    }
}

class Game {
    [int] $ID
    [CubeSet[]] $CubeSets

    [Game] static Parse([string] $value) {
        $game = [Game]::new()
        switch -Regex ($value) {
            'Game\s(?<GameId>\d+):\s+(?<CubeSets>.+)' {
                $game.ID = $Matches.GameId
                $game.CubeSets = ($Matches.CubeSets -split ';')
                | ForEach-Object { [CubeSet]::Parse($_) }
            }
            Default { throw "Invalid Game: $value" }
        }
        return $game
    }

    [bool] IsPossibleWithBag([CubeSet] $Bag) {
        foreach ($cs in $this.CubeSets) {
            if (-not $Bag.IsSubSet($cs)) {
                return $false
            }
        }
        return $true
    }

    [CubeSet] MinBag() {
        $bag = [CubeSet]::new()
        foreach ($cs in $this.CubeSets) {
            $bag.RedCount = [math]::Max($bag.RedCount, $cs.RedCount)
            $bag.GreenCount = [math]::Max($bag.GreenCount, $cs.GreenCount)
            $bag.BlueCount = [math]::Max($bag.BlueCount, $cs.BlueCount)
        }
        return $bag
    }
}

# Get-Day02_1 # 2204
# Get-Day02_2 # 71036
