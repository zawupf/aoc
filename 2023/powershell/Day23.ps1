using namespace System
using namespace System.Collections.Generic
using namespace System.Collections.Immutable

$inputFile = "$PSScriptRoot\..\_inputs\Day23.txt"
$inputLines = Get-Content $inputFile

function Get-Day23_1 {
    part_1 $inputLines
}

function Get-Day23_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $g = [Graph]::new($lines)
    $map = $g.BuildMap()
    $cache = @{}

    $stack = [Stack[psobject]]::new()
    $stack.Push([PSCustomObject]@{
            Node    = $g.Start
            Visited = [ImmutableHashSet]::Create($g.Start)
            Length  = 0
        })

    $result = 0
    while ($stack.Count) {
        $current = $stack.Pop()

        if ($current.Node -eq $g.End) {
            $result = [math]::Max($result, $current.Length)
            continue
        }

        if ($cache[$current.Node] -gt $current.Length) {
            continue
        }

        $cache[$current.Node] = $current.Length
        $map[$current.Node]
        | Where-Object { -not $current.Visited.Contains($_.End) }
        | ForEach-Object {
            $next = [PSCustomObject]@{
                Node    = $_.End
                Visited = $current.Visited.Add($_.End)
                Length  = $current.Length + $_.Length
            }
            $stack.Push($next)
        }
    }

    $result
}

function part_2 {
    param (
        [string[]] $lines
    )

    $g = [Graph]::new($lines)
    $g.Paths += $g.Paths.Swapped()
    $map = $g.BuildMap()

    $stack = [Stack[psobject]]::new()
    $stack.Push([PSCustomObject]@{
            Node    = $g.Start
            Visited = [ImmutableHashSet]::Create($g.Start)
            Length  = 0
        })

    $result = 0
    while ($stack.Count) {
        $current = $stack.Pop()

        if ($current.Node -eq $g.End) {
            $result = [math]::Max($result, $current.Length)
            continue
        }

        $map[$current.Node]
        | Where-Object { -not $current.Visited.Contains($_.End) }
        | ForEach-Object {
            $next = [PSCustomObject]@{
                Node    = $_.End
                Visited = $current.Visited.Add($_.End)
                Length  = $current.Length + $_.Length
            }
            $stack.Push($next)
        }
    }

    $result
}

class Graph {
    [Node] $Start
    [Node] $End
    [Path[]] $Paths

    Graph([string[]] $lines) {
        $width = $lines[0].Length
        $height = $lines.Count

        $nodes = [HashSet[Node]]::new()
        $start_ = [Node]::new(1, 0)
        [void]$nodes.Add($start_)
        $end_ = [Node]::new($width - 2, $height - 1)
        [void]$nodes.Add($end_)

        $paths_ = [HashSet[Path]]::new()

        function mapGet ([Node] $p) { $lines[$p.y][$p.x] }

        $stack = [Stack[psobject]]::new()
        $stack.Push([PSCustomObject]@{
                Start  = $start_
                End    = $start_
                Length = 0
                Prev   = $null
            })

        while ($stack.Count) {
            $p = $stack.Pop()

            if ($p.End -eq $end_) {
                $_added = $paths_.Add([Path]$p)
                continue
            }

            $appendCount = 0
            $newCount = 0
            $next = @($p.End.IncX() , $p.End.DecX() , $p.End.IncY() , $p.End.DecY())
            | ForEach-Object {
                $isPrev = $p.Prev -eq $_
                $invalid = $_.X -lt 0 -or $_.X -ge $width -or $_.Y -lt 0 -or $_.Y -ge $height
                if ($isPrev -or $invalid) {
                    return
                }

                $n = mapGet $_
                if ($n -eq '#') {
                    return
                }

                switch ($n) {
                    '.' { ++$appendCount }
                    Default { ++$newCount }
                }

                if ($n -eq '.') {
                    [PSCustomObject]@{
                        Start  = $p.Start
                        End    = $_
                        Length = $p.Length + 1
                        Prev   = $p.End
                    }
                }
                elseif (($n -eq '<' -and $_.X -lt $p.End.X) -or
                        ($n -eq '>' -and $_.X -gt $p.End.X) -or
                        ($n -eq '^' -and $_.Y -lt $p.End.Y) -or
                        ($n -eq 'v' -and $_.Y -gt $p.End.Y)) {
                    [PSCustomObject]@{
                        Start  = $p.End
                        End    = $_
                        Length = 1
                        Prev   = $p.End
                    }
                }
            }

            if ($appendCount -gt 1 -or ($appendCount -and $newCount)) {
                throw "PANIC!"
            }

            $newStart = $false
            if ($newCount) {
                $_added = $paths_.Add([Path]$p)
                if (-not $_added) {
                    throw "But, why???"
                }
                $nextStart = ($next | Select-Object -First 1).Start
                $newStart = $nodes.Add($nextStart)
            }

            if ($appendCount -or $newStart) {
                $next | ForEach-Object {
                    $stack.Push($_)
                }
            }
        }

        $this.Start = $start_
        $this.End = $end_
        $this.Paths = $paths_
    }

    [hashtable] BuildMap() {
        $nodes = [HashSet[Node]]::new()
        $this.Paths | ForEach-Object {
            [void]$nodes.Add($_.Start)
            [void]$nodes.Add($_.End)
        }

        $map = @{}
        $nodes | ForEach-Object {
            $node = $_
            $paths_ = $this.Paths | Where-Object Start -EQ $node
            if ($null -ne $paths_) {
                $map[$node] = $paths_
            }
        }

        return $map
    }
}

class Node : Tuple[int, int] {
    Node([int]$x, [int]$y) : base($x, $y) {}

    static  Node() {
        "Node" | Update-TypeData -MemberName X -MemberType AliasProperty -Value Item1 -Force
        "Node" | Update-TypeData -MemberName Y -MemberType AliasProperty -Value Item2 -Force
    }

    [Node] IncX() { return [Node]::new($this.X + 1, $this.Y) }
    [Node] DecX() { return [Node]::new($this.X - 1, $this.Y) }
    [Node] IncY() { return [Node]::new($this.X, $this.Y + 1) }
    [Node] DecY() { return [Node]::new($this.X, $this.Y - 1) }
}

class Path : Tuple[psobject, psobject, int] {
    Path([Node]$start, [Node]$end, [int]$length) : base($start, $end, $length) {}
    Path([hashtable]$h) : base($h.Start, $h.End, $h.Length) {}
    Path([psobject]$h) : base($h.Start, $h.End, $h.Length) {}

    static  Path() {
        "Path" | Update-TypeData -MemberName Start -MemberType AliasProperty -Value Item1 -Force
        "Path" | Update-TypeData -MemberName End -MemberType AliasProperty -Value Item2 -Force
        "Path" | Update-TypeData -MemberName Length -MemberType AliasProperty -Value Item3 -Force
    }

    [Path] Swapped() {
        return [Path]::new($this.End, $this.Start, $this.Length)
    }
}

# Get-Day23_1 # 2238
# Get-Day23_2 # 6398
