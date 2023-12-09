BeforeAll {
    . "./Day09.ps1"
}

Describe 'Test Day09.ps1' {
    It "Part 1" { Get-Day09_1 | Should -BeExactly 1953784198 }
    It "Part 2" { Get-Day09_2 | Should -BeExactly 957 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 114
                in       = @"
0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 2
                in       = $testData1[0].in
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
