BeforeAll {
    . "./Day20.ps1"
}

Describe 'Test Day20.ps1' {
    It "Part 1" { Get-Day20_1 | Should -BeExactly 817896682 }
    It "Part 2" { Get-Day20_2 | Should -BeExactly 143219569011526 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 32000000
                in       = @"
broadcaster -> a, b, c
%a -> b
%b -> c
%c -> inv
&inv -> a
"@ -split "`n"
            }
            @{
                expected = 11687500
                in       = @"
broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> rx
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 167409079868000
                in       = $testData1[1].in
            }
        )
    }

    It "test part 1" -ForEach $testData1 {
        part_1 $in | Should -BeExactly $expected
    }

    It "test  part 2" -ForEach $testData2 {
        part_2 $in | Should -BeExactly $expected
    }
}
