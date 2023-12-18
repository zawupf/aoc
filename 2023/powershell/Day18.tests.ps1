BeforeAll {
    . "./Day18.ps1"
}

Describe 'Test Day18.ps1' {
    It "Part 1" { Get-Day18_1 | Should -BeExactly 70253 }
    # It "Part 2" { Get-Day18_2 | Should -BeExactly 1268 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 62
                in       = @"
R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 952408144115
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
