using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day20.txt"
$inputLines = Get-Content $inputFile
function Get-Day20_1 {
    part_1 $inputLines
}

function Get-Day20_2 {
    part_2 $inputLines
}

function part_1 {
    param (
        [string[]] $lines
    )

    $modules = parseModules($lines)
    $events = [Queue[Event]]::new()

    [int64[]]$pulseCount = @(0, 0)

    $n = 1000
    while ($n--) {
        $events.Enqueue([Event]@{
                Sender   = "button"
                Receiver = "broadcaster"
                Pulse    = 0
            })

        while ($events.Count) {
            $e = $events.Dequeue()
            $pulseCount[$e.Pulse]++

            $m = $modules[$e.Receiver]
            if (-not $m) {
                continue
            }

            $es = $m.Handle($e)
            if (-not $es) {
                continue
            }

            $es | ForEach-Object { $events.Enqueue($_) }
        }
    }

    $pulseCount[0] * $pulseCount[1]
}

function part_2 {
    param (
        [string[]] $lines
    )

    $modules = parseModules($lines)
    $events = [Queue[Event]]::new()

    [int64]$buttonCount = 0
    [int64]$rxLowCount = 0

    while ($rxLowCount -eq 0) {
        $events.Enqueue([Event]@{
                Sender   = "button"
                Receiver = "broadcaster"
                Pulse    = 0
            })

        while ($events.Count) {
            $e = $events.Dequeue()
            if ($e.Sender -eq "button") {
                ++$buttonCount
            }
            elseif ($e.Receiver -eq "rx" -and -not $e.Pulse) {
                ++$rxLowCount
            }

            $m = $modules[$e.Receiver]
            if (-not $m) {
                continue
            }

            $es = $m.Handle($e)
            if (-not $es) {
                continue
            }

            $es | ForEach-Object { $events.Enqueue($_) }
        }
    }

    $buttonCount
}

function parseModules ([string[]] $lines) {
    $modules = switch -Regex ($lines) {
        '^(?<Prefix>[%&]?)(?<Name>.*) -> (?<Receivers>.*)$' {
            $m = $Matches | Select-Object Prefix, Name, Receivers
            switch ($m.Prefix) {
                '%' { [FlipFlop]::new($m.Name, $m.Receivers -split ", ") }
                '&' { [Conjunction]::new($m.Name, $m.Receivers -split ", ") }
                '' {
                    if ($m.Name -eq "broadcaster") {
                        [Broadcaster]::new($m.Receivers -split ", ")
                    }
                    else {
                        [Module]::new($m.Name, $m.Receivers -split ", ")
                    }
                }
                Default { throw "Invalid prefix: $_" }
            }
        }
        Default { throw "Invalid module: $_" }
    }

    foreach ($m in $modules) {
        if ($m -is [Conjunction]) {
            foreach ($m_ in $modules) {
                if ($m_.Receivers -contains $m.Name) {
                    $m.Inputs[$m_.Name] = 0
                }
            }
        }
    }

    $map = [ordered]@{}
    $modules | ForEach-Object { $map[$_.Name] = $_ }
    $map
}

class Event {
    [string] $Sender
    [string] $Receiver
    [int] $Pulse
}

class Module {
    [string] $Name
    [string[]] $Receivers

    Module([string] $name_, [string[]] $receivers_) {
        $this.Name = $name_
        $this.Receivers = $receivers_
    }

    [Event[]] Handle([Event] $e) {
        throw "Not implemented in derived module"
    }

    [Event[]] Emit([int] $pulse) {
        return $this.Receivers | ForEach-Object {
            [Event]@{
                Sender   = $this.Name
                Receiver = $_
                Pulse    = $pulse
            }
        }
    }
}

class Broadcaster : Module {
    Broadcaster([string[]] $receivers_) : base("broadcaster", $receivers_) {}

    [Event[]] Handle([Event] $e) {
        return $this.Emit($e.Pulse)
    }
}

class FlipFlop : Module {
    [bool] $IsOn

    FlipFlop([string] $name_, [string[]] $receivers_) : base($name_, $receivers_) {
        $this.IsOn = $false
    }

    [Event[]] Handle([Event] $e) {
        if ($e.Pulse) {
            return $null
        }

        $this.IsOn = -not $this.IsOn
        return $this.Emit([int]$this.IsOn)
    }
}

class Conjunction : Module {
    [hashtable] $Inputs

    Conjunction([string] $name_, [string[]] $receivers_) : base($name_, $receivers_) {
        $this.Inputs = [ordered]@{}
    }

    [Event[]] Handle([Event] $e) {
        $this.Inputs[$e.Sender] = $e.Pulse
        return $this.Emit(-not ($e.Pulse -and $this.IsAllHigh()))
    }

    [bool] IsAllHigh() {
        foreach ($pulse in $this.Inputs.Values) {
            if (-not $pulse) {
                return $false
            }
        }
        return $true
    }
}

# Get-Day20_1 # 817896682
# Get-Day20_2 # 143219569011526
