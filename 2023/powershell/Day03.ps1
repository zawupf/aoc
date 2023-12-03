$inputFile = "$PSScriptRoot\..\_inputs\Day03.txt"
$inputLines = Get-Content $inputFile

function Get-Day03_1 {
    part_1 $inputLines
}

function Get-Day03_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $Lines
    )

    $height = $Lines.Count
    $width = $Lines[0].Length

    function testSymbol ($s) {
        $s -match '[^.0-9]'
    }

    function testAdjacentSymbols ($match, $lineIndex) {
        $a = [math]::Max($match.Index - 1, 0)
        $b = [math]::Min($match.Index + $match.Length, $width - 1)

        $y = $lineIndex - 1
        if ($y -ge 0) {
            $s = $Lines[$y].Substring($a, $b - $a + 1)
            if (testSymbol $s) {
                return $true
            }
        }

        $y = $lineIndex + 1
        if ($y -lt $height) {
            $s = $Lines[$y].Substring($a, $b - $a + 1)
            if (testSymbol $s) {
                return $true
            }
        }

        $i = $match.Index - 1
        if ($i -ge 0 -and (testSymbol $Lines[$lineIndex][$i])) {
            return $true
        }

        $i = $match.Index + $match.Length
        if ($i -lt $width -and (testSymbol $Lines[$lineIndex][$i])) {
            return $true
        }
        return $false
    }

    $sum = 0
    for ($y = 0; $y -lt $height; $y++) {
        $line = $Lines[$y]
        foreach ($match in [regex]::Matches($line, '\d+')) {
            if (testAdjacentSymbols $match $y) {
                $sum += [int]$match.Value
            }
        }
    }
    $sum
}

function part_2 {
    param (
        [string[]] $Lines
    )

    $height = $Lines.Count
    $width = $Lines[0].Length

    $gears = @{}
    for ($y = 0; $y -lt $height; $y++) {
        $line = $Lines[$y]
        foreach ($match in [regex]::Matches($line, '\d+')) {
            $a = [math]::Max($match.Index - 1, 0)
            $b = [math]::Min($match.Index + $match.Length, $width - 1)
            $cy = $y - 1
            if ($cy -ge 0) {
                for ($x = $a; $x -le $b; $x++) {
                    if ($Lines[$cy][$x] -eq "*") {
                        $gears["$x,$cy"] += , [int]$match.Value
                    }
                }
            }
            $cy = $y + 1
            if ($cy -lt $height) {
                for ($x = $a; $x -le $b; $x++) {
                    if ($Lines[$cy][$x] -eq "*") {
                        $gears["$x,$cy"] += , [int]$match.Value
                    }
                }
            }
            $x = $match.Index - 1
            if ($x -ge 0 -and $Lines[$y][$x] -eq '*') {
                $gears["$x,$y"] += , [int]$match.Value
            }
            $x = $match.Index + $match.Length
            if ($x -lt $width -and $Lines[$y][$x] -eq '*') {
                $gears["$x,$y"] += , [int]$match.Value
            }
        }
    }

    $sum = 0
    foreach ($gear in $gears.Keys) {
        $values = $gears[$gear]
        switch ($values.Count) {
            1 {  }
            2 { $sum += $values[0] * $values[1] }
            Default { throw "Too many values for gear: $gear" }
        }
    }
    $sum
}

# Get-Day03_1 # 557705
# Get-Day03_2 # 84266818
