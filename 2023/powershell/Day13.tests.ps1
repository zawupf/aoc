BeforeAll {
    . "./Day13.ps1"
}

Describe 'Test Day13.ps1' {
    # It "Part 1" { Get-Day13_1 | Should -BeExactly 8419 }
    # It "Part 2" { Get-Day13_2 | Should -BeExactly 840988812853 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 21
                in       = @"
???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 525152
                in       = $testData1[0].in
            }
        )
    }

    # It "test part 1" -ForEach $testData1 {
    #     part_1 $in | Should -BeExactly $expected
    # }

    # It "test  part 2" -ForEach $testData2 {
    #     part_2 $in | Should -BeExactly $expected
    # }
}
