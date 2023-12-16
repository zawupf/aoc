BeforeAll {
    . "./Day16.ps1"
}

Describe 'Test Day16.ps1' {
    It "Part 1" { Get-Day16_1 | Should -BeExactly 8112 }
    # It "Part 2" { Get-Day16_2 | Should -BeExactly 8314 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 46
                in       = @"
.|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 51
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
