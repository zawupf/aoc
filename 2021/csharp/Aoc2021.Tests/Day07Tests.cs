namespace Aoc2021.Tests;

public class Day07Tests
{
    private static readonly List<int> input = "16,1,2,0,4,2,7,1,2,14"
        .Split(',').Select(Utils.ParseInt).ToList();

    [Fact]
    public void FindPositionWorks()
    {
        Assert.Equal((2, 37), Day07.FindBestPostion(input));
        Assert.Equal((5, 168), Day07.FindBestCrabPostion(input));
    }

    [Fact]
    public void Stars()
    {
        Day07 run = new();
        Assert.Equal("348996", run.Result1());
        Assert.Equal("98231647", run.Result2());
    }
}
