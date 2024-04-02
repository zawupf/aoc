BeforeAll {
    . "./Day25.ps1"
}

Describe 'Test Day25.ps1' {
    It "Part 1" { Get-Day25_1 | Should -BeExactly 3782 }
    It "Part 2" { Get-Day25_2 | Should -BeExactly 143219569011526 }

    BeforeDiscovery {
        $testData1 = @(
            @{
                expected = 54
                in       = @"
jqt: rhn xhk nvd
rsh: frs pzl lsr
xhk: hfx
cmg: qnr nvd lhk bvb
rhn: xhk bvb hfx
bvb: xhk hfx
pzl: lsr hfx nvd
qnr: nvd
ntq: jqt hfx bvb xhk
nvd: lhk
lsr: lhk
rzs: qnr cmg lsr rsh
frs: qnr lhk lsr
"@ -split "`n"
            }
        )

        $testData2 = @(
            @{ expected = 6536; in = $testData1[0].in }
        )
    }

    It "test part 1" -ForEach $testData1 {
        part_1 $in | Should -BeExactly $expected
    }

    It "test  part 2" -ForEach $testData2 {
        part_2 $in | Should -BeExactly $expected
    }
}
