using namespace System
using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day22.txt"
$inputLines = Get-Content $inputFile

function Get-Day22_1 {
    part_1 $inputLines
}

function Get-Day22_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $bricks = [Brick]::Parse($lines) | Sort-Object Z
    $a = analyze $bricks
    $supportingBricks = $a.Item1
    $graph = $a.Item2

    $topBricksCount = $bricks.Count - $graph.Count

    [HashSet[int]] $candidateBricks = $supportingBricks
    | Where-Object Count -GE 2
    | ForEach-Object { $_ }
    | Sort-Object -Unique

    [HashSet[int]] $requiredBricks = $supportingBricks
    | Where-Object Count -EQ 1
    | ForEach-Object { $_ }
    | Sort-Object -Unique

    $candidateBricks.ExceptWith([HashSet[int]]$requiredBricks)

    $candidateBricks.Count + $topBricksCount
}

function part_2 {
    param (
        [string[]] $lines
    )

    $bricks = [Brick]::Parse($lines) | Sort-Object Z
    $a = analyze $bricks
    $supportingBricks = $a.Item1
    $graph = $a.Item2

    $result = 0
    foreach ($brick in $graph.Keys) {
        $moved = [HashSet[int]]::new()
        [void]$moved.Add($brick)
        $nexts = [PriorityQueue[int, int]]::new()
        $nextBricks = [int[]]$graph[$brick]
        $nexts.EnqueueRange($nextBricks, $bricks[$nextBricks[0]].Z)
        while ($nexts.Count) {
            $next = $nexts.Dequeue()
            $supporting = $supportingBricks[$next]
            if ($supporting.IsSubsetOf($moved)) {
                [void]$moved.Add($next)
                $nextBricks = [int[]]$graph[$next]
                if ($null -ne $nextBricks) {
                    $nexts.EnqueueRange($nextBricks, $bricks[$nextBricks[0]].Z)
                }
            }
        }
        $result += $moved.Count - 1
    }
    $result
}

function analyze ([Brick[]] $bricks) {
    $supportingBricks = [HashSet[int][]]::new($bricks.Count)
    $graph = @{}
    $terrain = @{}

    for ($i = 0; $i -lt $bricks.Count; $i++) {
        $brick = $bricks[$i]
        $surfaceItems = $brick.Surface()

        $terrainItems = $surfaceItems | ForEach-Object {
            $info = $terrain[$_]
            $null -ne $info ? $info : [PSCustomObject]@{
                BrickIndex = $null
                Height     = 0
            }
        }

        $h = ($terrainItems | Measure-Object -Maximum Height).Maximum
        $terrainItems = $terrainItems
        | Where-Object Height -EQ $h
        | Sort-Object -Unique BrickIndex
        $supportingBricks[$i] = @($terrainItems | Select-Object -ExpandProperty BrickIndex)
        $supportingBricks[$i] | ForEach-Object { $graph[$_] += , $i }
        $brick.Z = ++$h
        $info = [PSCustomObject]@{
            BrickIndex = $i
            Height     = $h + $brick.DZ
        }
        $surfaceItems | ForEach-Object { $terrain[$_] = $info }
    }

    [Tuple]::Create($supportingBricks, $graph)
}

class Brick {
    [int] $X
    [int] $Y
    [int] $Z
    [int] $DX
    [int] $DY
    [int] $DZ

    [string[]] Surface() {
        $tuples = $this.X..($this.X + $this.DX) | ForEach-Object {
            $x_ = $_
            $this.Y..($this.Y + $this.DY) | ForEach-Object {
                $y_ = $_
                , "$x_,$y_"
            }
        }
        return $tuples
    }

    [string[]] Volume() {
        $tuples = $this.X..($this.X + $this.DX) | ForEach-Object {
            $x_ = $_
            $this.Y..($this.Y + $this.DY) | ForEach-Object {
                $y_ = $_
                $this.Z..($this.Z + $this.DZ) | ForEach-Object {
                    $z_ = $_
                    , "$x_,$y_,$z_"
                }
            }
        }
        return $tuples
    }

    [string] ToString() {
        $o = "$($this.X),$($this.Y),$($this.Z)"
        $d = switch ($true) {
            (!!$this.DX) { "dx=$($this.DX)" }
            (!!$this.DY) { "dy=$($this.DY)" }
            (!!$this.DZ) { "dz=$($this.DZ)" }
            Default { "d=0" }
        }
        return "$o $d"
    }

    [Brick[]] static Parse([string[]] $line) {
        return $line | ForEach-Object { [Brick]::Parse($_) }
    }

    [Brick] static Parse([string] $line) {
        if ($line -notmatch '^(?<x1>\d+),(?<y1>\d+),(?<z1>\d+)~(?<x2>\d+),(?<y2>\d+),(?<z2>\d+)$') {
            throw "Invalid brick: $line"
        }

        $brick = [Brick]@{
            X  = [math]::Min($Matches.x1, $Matches.x2)
            DX = [math]::Abs($Matches.x2 - $Matches.x1)
            Y  = [math]::Min($Matches.y1, $Matches.y2)
            DY = [math]::Abs($Matches.y2 - $Matches.y1)
            Z  = [math]::Min($Matches.z1, $Matches.z2)
            DZ = [math]::Abs($Matches.z2 - $Matches.z1)
        }
        if ($brick.DX -and $brick.DY -or
            $brick.DX -and $brick.DZ -or
            $brick.DY -and $brick.DZ ) {
            throw "Invalid brick: $line"
        }
        return $brick
    }
}

# Get-Day22_1 # 503
# Get-Day22_2 # 98431
