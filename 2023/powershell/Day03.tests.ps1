BeforeAll {
    . "./Day03.ps1"
}

Describe 'Test Day03.ps1' {
    It "Part 1" { Get-Day03_1 | Should -BeExactly 557705 }
    It "Part 2" { Get-Day03_2 | Should -BeExactly 84266818 }

    It "test part 1" {
        part_1 $input1 | Should -BeExactly 4361
    }

    It "test  part 2" {
        part_2 $input2 | Should -BeExactly 467835
    }
}

$script:input1 = @"
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

$script:input2 = $input1
