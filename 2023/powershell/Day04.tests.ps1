BeforeAll {
    . "./Day04.ps1"
}

Describe 'Test Day04.ps1' {
    It "Part 1" { Get-Day04_1 | Should -BeExactly 21138 }
    It "Part 2" { Get-Day04_2 | Should -BeExactly 7185540 }

    BeforeDiscovery {
        $input1 = @"
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
"@ -split "`n"

        $input2 = $input1
    }

    It "test part 1" -ForEach @(, $input1) {
        part_1 $_ | Should -BeExactly 13
    }

    It "test  part 2" -ForEach @(, $input2) {
        part_2 $_ | Should -BeExactly 30
    }
}
