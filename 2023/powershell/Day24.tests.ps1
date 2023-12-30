BeforeAll {
    . "./Day24.ps1"
}

Describe 'Test Day24.ps1' {
    It "Part 1" { Get-Day24_1 | Should -BeExactly 16727 }
    # It "Part 2" { Get-Day24_2 | Should -BeExactly 143219569011526 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 2
                area     = @(7, 27)
                in       = @"
19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{ expected = 6536; in = $testData1[0].in }
        )
    }

    It "test part 1" -ForEach $testData1 {
        part_1 $in $area[0] $area[1] | Should -BeExactly $expected
    }

    # It "test  part 2" -ForEach $testData2 {
    #     part_2 $in $area[0] $area[1] | Should -BeExactly $expected
    # }
}
