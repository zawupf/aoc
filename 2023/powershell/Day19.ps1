using namespace System.Collections.Generic

$inputFile = "$PSScriptRoot\..\_inputs\Day19.txt"
$inputContent = Get-Content $inputFile -Raw
function Get-Day19_1 {
    part_1 $inputContent
}

function Get-Day19_2 {
    part_2 $inputContent
}

function part_1 {
    param (
        [string] $content
    )

    function sum ($p) { $p.x + $p.m + $p.a + $p.s }

    $data = $content -split "`n`n"
    $workflows = parseWorkflows ($data[0].Trim() -split "`n")
    $parts = parseParts ($data[1].Trim() -split "`n")

    $result = 0
    foreach ($part in $parts) {
        $workflow = $workflows['in']
        while ($workflow) {
            for ($i = 0; $i -lt $workflow.Rules.Count; $i++) {
                $rule = $workflow.Rules[$i]
                $prop = $rule.Property
                if ($prop) {
                    $propValue = $part.$prop
                    $predicate = switch ($rule.Operator) {
                        '<' { $propValue -lt $rule.Value }
                        '>' { $propValue -gt $rule.Value }
                        Default { throw "Invalid operator: $_" }
                    }
                    if ($predicate) {
                        switch ($rule.NextWorkflow) {
                            'R' {  }
                            'A' { $result += sum $part }
                            Default {}
                        }
                        $workflow = $workflows[$rule.NextWorkflow]
                        break
                    }
                }
                else {
                    switch ($rule.NextWorkflow) {
                        'R' {  }
                        'A' { $result += sum $part }
                        Default {}
                    }
                    $workflow = $workflows[$rule.NextWorkflow]
                    break
                }
            }
        }
    }

    $result
}

function part_2 {
    param (
        [string] $content
    )
    function product ($p) {
        ($p.x2 - $p.x1 + 1) *
        ($p.m2 - $p.m1 + 1) *
        ($p.a2 - $p.a1 + 1) *
        ($p.s2 - $p.s1 + 1)
    }

    $data = $content -split "`n`n"
    $workflows = parseWorkflows ($data[0].Trim() -split "`n")

    $states = [Stack[psobject]]::new()
    $states.Push([PSCustomObject]@{
            Parts     = [PSCustomObject]@{
                x1 = 1; m1 = 1; a1 = 1; s1 = 1
                x2 = 4000; m2 = 4000; a2 = 4000; s2 = 4000
            }
            Workflow  = $workflows['in']
            RuleIndex = 0
        })

    [int64]$result = 0
    while ($states.Count) {
        $s = $states.Pop()
        $rule = $s.Workflow.Rules[$s.RuleIndex]
        $prop = $rule.Property
        if ($prop) {
            $p1 = "$($prop)1"
            $p2 = "$($prop)2"
            $a = $s.Parts.$p1
            $b = $s.Parts.$p2
            $m = $rule.Value
            switch ($rule.Operator) {
                '<' {
                    if ($b -ge $m) {
                        $parts = $s.Parts.psobject.Copy()
                        $parts.$p1 = $m
                        $states.Push([PSCustomObject]@{
                                Parts     = $parts
                                Workflow  = $s.Workflow
                                RuleIndex = $s.RuleIndex + 1
                            })
                    }
                    if ($a -lt $m) {
                        $s.Parts.$p2 = $m - 1
                        $s.Workflow = $workflows[$rule.NextWorkflow]
                        $s.RuleIndex = 0
                        switch ($rule.NextWorkflow) {
                            'R' {  }
                            'A' { $result += product $s.Parts }
                            Default { $states.Push($s) }
                        }

                    }
                }
                '>' {
                    if ($a -le $m) {
                        $parts = $s.Parts.psobject.Copy()
                        $parts.$p2 = $m
                        $states.Push([PSCustomObject]@{
                                Parts     = $parts
                                Workflow  = $s.Workflow
                                RuleIndex = $s.RuleIndex + 1
                            })
                    }
                    if ($b -gt $m) {
                        $s.Parts.$p1 = $m + 1
                        $s.Workflow = $workflows[$rule.NextWorkflow]
                        $s.RuleIndex = 0
                        switch ($rule.NextWorkflow) {
                            'R' {  }
                            'A' { $result += product $s.Parts }
                            Default { $states.Push($s) }
                        }

                    }
                }
                Default { throw "Invalid operator: $_" }
            }
        }
        else {
            switch ($rule.NextWorkflow) {
                'R' {  }
                'A' { $result += product $s.Parts }
                Default {
                    $s.Workflow = $workflows[$rule.NextWorkflow]
                    $s.RuleIndex = 0
                    $states.Push($s)
                }
            }
        }
    }

    $result
}

function parseWorkflows ([string[]] $lines) {
    # ex{x>10:one,m<20:two,a>30:R,A}
    $map = @{}
    switch -Regex ($lines) {
        '^(?<Name>\w+)\{(?<Rules>.+)\}$' {
            $name = $Matches.Name
            $rules = $Matches.Rules -split ','
            $map[$name] = [PSCustomObject]@{
                Name  = $name
                Rules = switch -Regex ($rules) {
                    '^(?<Property>[xmas])(?<Operator>[<>])(?<Value>\d+):(?<NextWorkflow>\w+)$' {
                        [PSCustomObject]@{
                            Property     = $Matches.Property
                            Operator     = $Matches.Operator
                            Value        = $Matches.Value -as [int]
                            NextWorkflow = $Matches.NextWorkflow
                        }
                    }
                    '^\w+$' {
                        [PSCustomObject]@{
                            NextWorkflow = $Matches[0]
                        }
                    }
                    Default { throw "Invalid rule: $_" }
                }
            }
        }
        Default { throw "Invalid workflow: $_" }
    }
    $map
}

function parseParts ([string[]] $lines) {
    # {x=787,m=2655,a=1222,s=2876}
    $lines.Trim('{', '}') | ForEach-Object {
        $props = [ordered]@{ x = 0; m = 0; a = 0; s = 0 }
        switch -Regex ($_ -split ',') {
            '^(?<Name>[xmas])=(?<Value>\d+)$' {
                $props[$Matches.Name] = $Matches.Value -as [int]
            }
            Default { throw "Invalid part property: $_" }
        }
        [PSCustomObject]$props
    }
}

# Get-Day19_1 # 509597
# Get-Day19_2 # 143219569011526
