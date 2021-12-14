namespace Aoc2021.Tests;

public class Day14Tests
{
    private static readonly List<string> input = @"
        NNCB

        CH -> B
        HH -> N
        CB -> H
        NH -> C
        HB -> C
        HC -> B
        HN -> C
        NN -> C
        BH -> H
        NC -> B
        NB -> B
        BN -> B
        BB -> N
        BC -> B
        CC -> N
        CN -> C
    "
    .Trim()
    .Split('\n')
    .Select(line => line.Trim())
    .ToList();

    [Fact]
    public void MaxMinDiffWorks()
    {
        Assert.Equal(1588L, Day14.MaxMinDiff(10, input));
        Assert.Equal(2188189693529L, Day14.MaxMinDiff(40, input));
    }

    [Fact]
    public void Stars()
    {
        Day14 run = new();
        Assert.Equal("3143", run.Result1());
        Assert.Equal("4110215602456", run.Result2());
    }
}
