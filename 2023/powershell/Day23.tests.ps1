BeforeAll {
    . "./Day23.ps1"
}

Describe 'Test Day23.ps1' {
    It "Part 1" { Get-Day23_1 | Should -BeExactly 2238 }
    It "Part 2" { Get-Day23_2 | Should -BeExactly 6398 } # hours later...

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 94
                in       = @"
#.#####################
#.......#########...###
#######.#########.#.###
###.....#.>.>.###.#.###
###v#####.#v#.###.#.###
###.>...#.#.#.....#...#
###v###.#.#.#########.#
###...#.#.#.......#...#
#####.#.#.#######.#.###
#.....#.#.#.......#...#
#.#####.#.#.#########v#
#.#...#...#...###...>.#
#.#.#v#######v###.###v#
#...#.>.#...>.>.#.###.#
#####v#.#.###v#.#.###.#
#.....#...#...#.#.#...#
#.#########.###.#.#.###
#...###...#...#...#.###
###.###.#.###v#####v###
#...#...#.#.>.>.#.>.###
#.###.###.#.###.#.#v###
#.....###...###...#...#
#####################.#
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{ expected = 154; in = $testData1[0].in }
        )
    }

    It "test part 1" -ForEach $testData1 {
        part_1 $in | Should -BeExactly $expected
    }

    It "test  part 2" -ForEach $testData2 {
        part_2 $in | Should -BeExactly $expected
    }
}
