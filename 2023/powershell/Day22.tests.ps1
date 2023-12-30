BeforeAll {
    . "./Day22.ps1"
}

Describe 'Test Day22.ps1' {
    It "Part 1" { Get-Day22_1 | Should -BeExactly 503 }
    It "Part 2" { Get-Day22_2 | Should -BeExactly 98431 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 5
                in       = @"
1,0,1~1,2,1
0,0,2~2,0,2
0,2,3~2,2,3
0,0,4~0,2,4
2,0,5~2,2,5
0,1,6~2,1,6
1,1,8~1,1,9
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{ expected = 7; in = $testData1[0].in }
        )
    }

    It "test part 1" -ForEach $testData1 {
        part_1 $in | Should -BeExactly $expected
    }

    It "test  part 2" -ForEach $testData2 {
        part_2 $in | Should -BeExactly $expected
    }
}
