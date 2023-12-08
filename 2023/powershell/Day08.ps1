using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day08.txt"
$inputLines = Get-Content $inputFile

function Get-Day08_1 {
    part_1 $inputLines
}

function Get-Day08_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $instructions = $lines[0]

    $map = @{}
    $lines[2..($lines.Count - 1)] | ForEach-Object {
        $node = [Node]::Parse($_)
        $map[$node.Name] = $node
    }

    $count = 0
    $n = $instructions.Length
    $i = 0
    $node = $map["AAA"]
    while ($node.Name -ne "ZZZ") {
        $node = switch ($instructions[$i]) {
            'L' { $map[$node.Left] }
            'R' { $map[$node.Right] }
            Default { throw "Invalid instruction: $_" }
        }
        ++$count
        if (++$i -eq $n) {
            $i = 0
        }
    }

    $count
}

function part_2 {
    param (
        [string[]] $lines
    )

    $instructions = $lines[0]

    $map = @{}
    $nodes = @()
    $lines[2..($lines.Count - 1)] | ForEach-Object {
        $node = [Node]::Parse($_)
        $map[$node.Name] = $node
        if ($node.Name[2] -eq "A") {
            $nodes += $node
        }
    }

    function countStepsToEnd ([string] $startNodeName) {
        $count = 0
        $n = $instructions.Length
        $i = 0
        $node = $map[$startNodeName]

        while ($node.Name[2] -ne "Z") {
            $node = switch ($instructions[$i]) {
                'L' { $map[$node.Left] }
                'R' { $map[$node.Right] }
                Default { throw "Invalid instruction: $_" }
            }

            ++$count
            if (++$i -eq $n) {
                $i = 0
            }
        }

        $count
    }

    $numbers = $nodes | ForEach-Object { countStepsToEnd $_.Name }

    function Get-GCD ([int64]$a, [int64]$b) {
        while ($b -ne 0) {
            $temp = $b
            $b = $a % $b
            $a = $temp
        }
        return $a
    }

    function Get-LCM ([int64]$a, [int64]$b) {
        return [Math]::Abs($a * $b) / (Get-GCD $a $b)
    }

    function Get-LCMArray ([int64[]]$numbers) {
        $lcm = 1
        foreach ($number in $numbers) {
            $lcm = Get-LCM $lcm $number
        }
        return $lcm
    }

    $result = Get-LCMArray $numbers
    $result
}

class Node {
    [string] $Name
    [string] $Left
    [string] $Right

    [Node] static Parse([string] $value) {
        if ($value -match '^(?<Name>\w{3}) = \((?<Left>\w{3}), (?<Right>\w{3})\)$') {
            return [Node]($Matches | Select-Object Name, Left, Right)
        }
        throw "Invalid input: $value"
    }
}

# Get-Day08_1 # 20093
# Get-Day08_2 # 22103062509257
