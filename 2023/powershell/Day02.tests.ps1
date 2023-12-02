BeforeAll {
    . "./Day02.ps1"
}

Describe 'Test Day02.ps1' {
    It "Part 1" { Get-Day02_1 | Should -BeExactly 2204 }
    It "Part 2" { Get-Day02_2 | Should -BeExactly 71036 }

    It "test part 1" {
        $bag = [CubeSet]::Parse("12 red cubes, 13 green cubes, and 14 blue cubes")
        part_1 $input1 $bag | Should -BeExactly 8
    }

    It "test  part 2" {
        part_2 $input2 | Should -BeExactly 2286
    }
}

$script:input1 = @"
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
"@ -split "`n"

$script:input2 = $input1
