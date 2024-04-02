BeforeAll {
    . "./Day21.ps1"
}

Describe 'Test Day21.ps1' {
    It "Part 1" { Get-Day21_1 | Should -BeExactly 3782 }
    It "Part 2" { Get-Day21_2 | Should -BeExactly 143219569011526 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 16
                steps    = 6
                in       = @"
...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........
"@ -split "`n"
            }
        )

        $testData2 = @(
            # @{ expected = 16; steps = 6; in = $testData1[0].in }
            # @{ expected = 50; steps = 10; in = $testData1[0].in }
            # @{ expected = 1594; steps = 50; in = $testData1[0].in }
            @{ expected = 6536; steps = 100; in = $testData1[0].in }
            # @{ expected = 167004; steps = 500; in = $testData1[0].in }
            # @{ expected = 668697; steps = 1000; in = $testData1[0].in }
            # @{ expected = 16733044; steps = 5000; in = $testData1[0].in }
        )
    }

    It "test part 1" -ForEach $testData1 {
        part_1 $in $steps | Should -BeExactly $expected
    }

    It "test  part 2" -ForEach $testData2 {
        part_2 $in $steps | Should -BeExactly $expected
    }
}
