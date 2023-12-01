$inputFile = "$PSScriptRoot\..\_inputs\Day01.txt"

function Get-Day01_1 {
    $values = switch -Regex -File ($inputFile) {
        '(\d).*(\d)' { [int]"$($Matches.1)$($Matches.2)"; continue }
        '\d' { [int]"$($Matches.0)$($Matches.0)"; continue }
        Default { "> $_" }
    }
    ($values | Measure-Object -Sum).Sum
}

function Get-Day01_2 {
    function toNumber ([string]$value) {
        switch ($value) {
            'one' { '1' }
            'two' { '2' }
            'three' { '3' }
            'four' { '4' }
            'five' { '5' }
            'six' { '6' }
            'seven' { '7' }
            'eight' { '8' }
            'nine' { '9' }
            Default { $value }
        }
    }
    $digitPattern = '\d|one|two|three|four|five|six|seven|eight|nine'
    $values = switch -Regex -File ($inputFile) {
        "($digitPattern).*($digitPattern)" { [int]"$(toNumber $Matches.1)$(toNumber $Matches.2)"; continue }
        "$digitPattern" { [int]"$(toNumber $Matches.0)$(toNumber $Matches.0)"; continue }
        Default { "> $_" }
    }
    ($values | Measure-Object -Sum).Sum
}

Get-Day01_1 # 54990
Get-Day01_2 # 54473
