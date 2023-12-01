BeforeAll {
    . "./Day01.ps1"
}

Describe 'Test Day01.ps1' {
    It "Part 1" { Get-Day01_1 | Should -BeExactly 54990 }
    It "Part 2" { Get-Day01_2 | Should -BeExactly 54473 }

    It "test part 1" {
        Get-Day01_1 $input1 | Should -BeExactly 142
    }

    It "test  part 2" {
        Get-Day01_2 $input2 | Should -BeExactly 281
    }
}

$script:input1 = -split @"
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
"@

$script:input2 = -split @"
two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen
"@
