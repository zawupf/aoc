BeforeAll {
    . "./Day14.ps1"
}

Describe 'Test Day14.ps1' {
    It "Part 1" { Get-Day14_1 | Should -BeExactly 105623 }
    It "Part 2" { Get-Day14_2 | Should -BeExactly 98029 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 136
                In       = @"
O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 64
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
