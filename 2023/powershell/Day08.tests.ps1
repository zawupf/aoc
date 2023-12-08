BeforeAll {
    . "./Day08.ps1"
}

Describe 'Test Day08.ps1' {
    It "Part 1" { Get-Day08_1 | Should -BeExactly 20093 }
    It "Part 2" { Get-Day08_2 | Should -BeExactly 22103062509257 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 2
                in       = @"
RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)
"@ -split "`n"
            }
            @{
                expected = 6
                in       = @"
LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 6
                in       = @"
LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)
"@ -split "`n"
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
