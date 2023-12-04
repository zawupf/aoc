BeforeAll {
    . "./Day01.ps1"
}

Describe 'Test Day01.ps1' {
    It "Part 1" { Get-Day01_1 | Should -BeExactly 54990 }
    It "Part 2" { Get-Day01_2 | Should -BeExactly 54473 }

    BeforeDiscovery {
        $input1 = @"
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
"@ -split "`n"

        $input2 = @"
two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen
"@ -split "`n"
    }

    It "test part 1" -ForEach @(, $input1) {
        Get-Day01_1 $_ | Should -BeExactly 142
    }

    It "test  part 2" -ForEach @(, $input2) {
        Get-Day01_2 $_ | Should -BeExactly 281
    }
}
