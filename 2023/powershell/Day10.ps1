using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day10.txt"
$inputLines = Get-Content $inputFile

function Get-Day10_1 {
    part_1 $inputLines
}

function Get-Day10_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $grid = [Grid]::new($lines)
    $ways = $grid.Walk()
    $ways.Count - 1
}

function part_2 {
    param (
        [string[]] $lines
    )

    $grid = [Grid]::new($lines)
    $ways = $grid.Walk()

    $set = [HashSet[string]]::new()
    $way = $ways | ForEach-Object { $_ }
    $way | ForEach-Object {
        $k = $_.Key()
        [void]$set.Add($k)
    }

    [string[]]$dirs = @()
    $p = [Pos]::new($grid.start.x, $grid.start.y - 1)
    if ("|7F".Contains($grid.Get($p))) {
        $dirs += "N"
    }
    $p = [Pos]::new($grid.start.x, $grid.start.y + 1)
    if ("|LJ".Contains($grid.Get($p))) {
        $dirs += "S"
    }
    $p = [Pos]::new($grid.start.x - 1, $grid.start.y)
    if ("-FL".Contains($grid.Get($p))) {
        $dirs += "W"
    }
    $p = [Pos]::new($grid.start.x + 1, $grid.start.y)
    if ("-7J".Contains($grid.Get($p))) {
        $dirs += "E"
    }

    $startPipe = switch (-join $dirs) {
        "NS" { "|" }
        "NW" { "J" }
        "NE" { "L" }
        "SW" { "7" }
        "SE" { "F" }
        "WE" { "-" }
        Default { throw "Invlid dirs: $_" }
    }

    $lines = for ($y = 0; $y -lt $grid.height; $y++) {
        $line = $grid.lines[$y] -replace "S", $startPipe
        $line = for ($x = 0; $x -lt $grid.width; $x++) {
            if ($set.Contains([Pos]::new($x, $y).Key())) {
                $line[$x]
            }
            else {
                "."
            }
        }
        $line = -join $line
        $line
    }

    $count = 0
    for ($y = 0; $y -lt $lines.Count; $y++) {
        $inner = $false
        $line = $lines[$y]
        $line = $line -replace 'L-*J|F-*7', ''
        $line = $line -replace 'L-*7|F-*J', '|'
        for ($x = 0; $x -lt $line.Length; $x++) {
            switch ($line[$x]) {
                '.' { $count += $inner ? 1 : 0 }
                '|' { $inner = -not $inner }
                Default { throw "Invalid char: $_" }
            }
        }
    }
    $count
}

class Pos {
    [int] $x
    [int] $y

    Pos($x, $y) {
        $this.x = $x
        $this.y = $y
    }

    [string] Key() {
        $x_ = $this.x
        $y_ = $this.y
        return "$x_,$y_"
    }

    [Pos] North() {
        return [Pos]::new($this.x, $this.y - 1)
    }

    [Pos] South() {
        return [Pos]::new($this.x, $this.y + 1)
    }

    [Pos] West() {
        return [Pos]::new($this.x - 1, $this.y)
    }

    [Pos] East() {
        return [Pos]::new($this.x + 1, $this.y)
    }

    [bool] static Equal([Pos] $p1, [Pos] $p2) {
        return $p1.x -eq $p2.x -and $p1.y -eq $p2.y
    }
}

class Grid {
    [string[]] $lines
    [int] $width
    [int] $height
    [Pos] $start

    Grid([string[]] $lines) {
        $this.lines = $lines
        $this.width = $lines[0].Length
        $this.height = $lines.Count
        $this.start = & {
            for ($sy = 0; $sy -lt $lines.Count; $sy++) {
                $sx = $lines[$sy].IndexOf("S")
                if ($sx -ge 0) {
                    break
                }
            }
            [Pos]::new($sx, $sy)
        }
    }

    [string] Get ([Pos] $p) {
        if ($p.x -ge 0 -and $p.x -lt $this.width -and $p.y -ge 0 -and $p.y -lt $this.height) {
            return $this.lines[$p.y][$p.x]
        }
        else {
            return "."
        }
    }

    [pscustomobject[][]] Walk() {
        [pscustomobject[]]$starts = @($this.start, $this.start)
        [pscustomobject[]]$dirs = @()
        $p = [Pos]::new($this.start.x, $this.start.y - 1)
        if ("|7F".Contains($this.Get($p))) {
            $dirs += $p
        }
        $p = [Pos]::new($this.start.x, $this.start.y + 1)
        if ("|LJ".Contains($this.Get($p))) {
            $dirs += $p
        }
        $p = [Pos]::new($this.start.x - 1, $this.start.y)
        if ("-FL".Contains($this.Get($p))) {
            $dirs += $p
        }
        $p = [Pos]::new($this.start.x + 1, $this.start.y)
        if ("-7J".Contains($this.Get($p))) {
            $dirs += $p
        }

        function findDirs ([Pos] $p) {
            switch ($this.Get($p)) {
                '|' { @($p.South(), $p.North()) }
                '-' { @($p.West(), $p.East()) }
                'L' { @($p.North(), $p.East()) }
                'J' { @($p.North(), $p.West()) }
                '7' { @($p.South(), $p.West()) }
                'F' { @($p.South(), $p.East()) }
                Default { throw "Invalid pipe: $_" }
            }
        }

        $ways = @($starts, $dirs)
        do {
            $p = $ways[-1][0]
            $dirs = findDirs $p
            $p0 = if ([Pos]::Equal($dirs[0], $ways[-2][0])) { $dirs[1] } else { $dirs[0] }

            $p = $ways[-1][1]
            $dirs = findDirs $p
            $p1 = if ([Pos]::Equal($dirs[0], $ways[-2][1])) { $dirs[1] } else { $dirs[0] }

            $ways += , @($p0, $p1)
        } until (
            [Pos]::Equal($ways[-1][0], $ways[-1][1])
        )

        return $ways
    }
}

# Get-Day10_1 # 6697
# Get-Day10_2 # 423
