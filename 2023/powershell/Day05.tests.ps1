BeforeAll {
    . "./Day05.ps1"
}

Describe 'Test Day05.ps1' {
    It "Part 1" { Get-Day05_1 | Should -BeExactly 175622908 }
    It "Part 2" { Get-Day05_2 | Should -BeExactly 5200543 }

    BeforeDiscovery {
        $input1 = @"
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
"@

        $input2 = $input1
    }

    It "test part 1" -ForEach @(, $input1) {
        part_1 $_ | Should -BeExactly 35
    }

    It "test  part 2" -ForEach @(, $input2) {
        part_2 $_ | Should -BeExactly 46
    }
}
