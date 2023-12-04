BeforeAll {
    . "./Day03.ps1"
}

Describe 'Test Day03.ps1' {
    It "Part 1" { Get-Day03_1 | Should -BeExactly 557705 }
    It "Part 2" { Get-Day03_2 | Should -BeExactly 84266818 }

    BeforeDiscovery {
        $input1 = @"
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
"@ -split "`n"

        $input2 = $input1
    }

    It "test part 1" -ForEach @(, $input1) {
        part_1 $_ | Should -BeExactly 4361
    }

    It "test  part 2" -ForEach @(, $input2) {
        part_2 $_ | Should -BeExactly 467835
    }
}
