namespace Aoc2021.Tests;

public class Day09Tests
{
    private static readonly List<string> input = @"
        2199943210
        3987894921
        9856789892
        8767896789
        9899965678
    "
    .Lines();

    [Fact]
    public void RiskLevelSumWorks()
    {
        Assert.Equal(15, Day09.RiskLevelSum(input));
    }

    [Fact]
    public void BasinProductWorks()
    {
        Assert.Equal(1134, Day09.BasinProduct(input));
    }

    [Fact]
    public void Stars()
    {
        Day09 run = new();
        Assert.Equal("448", run.Result1());
        Assert.Equal("1417248", run.Result2());
    }
}
