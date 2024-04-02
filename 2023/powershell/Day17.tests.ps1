BeforeAll {
    . "./Day17.ps1"
}

Describe 'Test Day17.ps1' {
    It "Part 1" { Get-Day17_1 | Should -BeExactly 1128 } # ~25min
    It "Part 2" { Get-Day17_2 | Should -BeExactly 1268 } # ~30min

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 102
                in       = @"
2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 94
                in       = $testData1[0].in
            }
            @{
                expected = 71
                in       = @"
111111111111
999999999991
999999999991
999999999991
999999999991
"@ -split "`n"
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
