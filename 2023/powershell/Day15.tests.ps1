BeforeAll {
    . "./Day15.ps1"
}

Describe 'Test Day15.ps1' {
    It "Part 1" { Get-Day15_1 | Should -BeExactly 514025 }
    It "Part 2" { Get-Day15_2 | Should -BeExactly 244461 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 52
                in       = "HASH"
            }
            @{
                expected = 1320
                in       = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7"
            }
        )

        $testData2 = @(
            @{
                expected = 145
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
