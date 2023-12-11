BeforeAll {
    . "./Day11.ps1"
}

Describe 'Test Day11.ps1' {
    It "Part 1" { Get-Day11_1 | Should -BeExactly 10494813 }
    It "Part 2" { Get-Day11_2 | Should -BeExactly 840988812853 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 374
                in       = @"
...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....
"@ -split "`n"
            }
        )

        $testData2 = @(
            # @{
            #     expected = 1
            #     in       = $testData1[0].in
            # }
        )
    }

    It "test part 1" -ForEach $testData1 {
        part_1 $in | Should -BeExactly $expected
    }

    # It "test  part 2" -ForEach $testData2 {
    #     part_2 $in | Should -BeExactly $expected
    # }
}
