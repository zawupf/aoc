BeforeAll {
    . "./Day10.ps1"
}

Describe 'Test Day10.ps1' {
    It "Part 1" { Get-Day10_1 | Should -BeExactly 6697 }
    It "Part 2" { Get-Day10_2 | Should -BeExactly 423 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 4
                in       = @"
.-L|F7.
.7S-7|.
.L|7||.
.-L-J|.
.L|-JF.
"@ -split "`n"
            }
            @{
                expected = 8
                in       = @"
.7-F7-.
..FJ|7.
.SJLL7.
.|F--J.
.LJ.LJ.
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{
                expected = 1
                in       = $testData1[0].in
            }
            @{
                expected = 1
                in       = $testData1[1].in
            }
            @{
                expected = 4
                in       = @"
...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........
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
