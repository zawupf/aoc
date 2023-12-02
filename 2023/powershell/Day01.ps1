$inputFile = "$PSScriptRoot\..\_inputs\Day01.txt"
$inputLines = Get-Content $inputFile

function Get-Day01_1 ([string[]] $Lines = $inputLines) {
    getCalibrationSum '\d' $Lines
}

function Get-Day01_2  ($Lines = $inputLines) {
    getCalibrationSum '\d|one|two|three|four|five|six|seven|eight|nine' $Lines
}

function getCalibrationSum {
    param (
        [string] $DigitPattern,
        [string[]] $Lines
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

    $values = foreach ($line in $Lines) {
        $a = toNumber ([regex]::Match($line, ".*?($DigitPattern)").Groups[1].Value)
        $b = toNumber ([regex]::Match($line, ".*($DigitPattern)").Groups[1].Value)
        [int]"$a$b"
    }

    ($values | Measure-Object -Sum).Sum
}

# Get-Day01_1 # 54990
# Get-Day01_2 # 54473
