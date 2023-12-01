$inputFile = "$PSScriptRoot\..\_inputs\Day01.txt"
$lines = Get-Content $inputFile

function Get-Day01_1 {
    getCalibrationSum '\d'
}

function Get-Day01_2 {
    getCalibrationSum '\d|one|two|three|four|five|six|seven|eight|nine'
}

function getCalibrationSum {
    param (
        [Parameter(Mandatory)]
        [string]
        $DigitPattern
    )

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

    $values = foreach ($line in $lines) {
        $a = toNumber ([regex]::Match($line, ".*?($DigitPattern)").Groups[1].Value)
        $b = toNumber ([regex]::Match($line, ".*($DigitPattern)").Groups[1].Value)
        [int]"$a$b"
    }

    ($values | Measure-Object -Sum).Sum
}

Get-Day01_1 # 54990
Get-Day01_2 # 54473
