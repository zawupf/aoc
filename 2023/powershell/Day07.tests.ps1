BeforeAll {
    . "./Day07.ps1"
}

Describe 'Test Day07.ps1' {
    It "Part 1" { Get-Day07_1 | Should -BeExactly 249748283 }
    It "Part 2" { Get-Day07_2 | Should -BeExactly 248029057 }

    BeforeDiscovery {
        $input1 = @"
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
"@ -split "`n"

        $input2 = $input1
    }

    It "test part 1" -ForEach @(, $input1) {
        part_1 $_ | Should -BeExactly 6440
    }

    It "test  part 2" -ForEach @(, $input2) {
        part_2 $_ | Should -BeExactly 5905
    }
}
