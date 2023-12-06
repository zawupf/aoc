BeforeAll {
    . "./Day06.ps1"
}

Describe 'Test Day06.ps1' {
    It "Part 1" { Get-Day06_1 | Should -BeExactly 861300 }
    It "Part 2" { Get-Day06_2 | Should -BeExactly 28101347 }

    BeforeDiscovery {
        $input1 = @"
Time:      7  15   30
Distance:  9  40  200
"@ -split "`n"

        $input2 = $input1
    }

    It "test part 1" -ForEach @(, $input1) {
        part_1 $_ | Should -BeExactly 288
    }

    It "test  part 2" -ForEach @(, $input2) {
        part_2 $_ | Should -BeExactly 71503
    }
}
