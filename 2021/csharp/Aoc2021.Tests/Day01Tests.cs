namespace Aoc2021.Tests;

public class Day01Tests
{
    [Fact]
    public void CountIncreasedSeaDepths()
    {
        int[] seaDepths = { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 };
        Assert.Equal(7, Day01.CountIncreasedSeaDepths(seaDepths));
    }

    [Fact]
    public void CountIncreasedSeaDepthWindows()
    {
        int[] seaDepths = { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 };
        Assert.Equal(5, Day01.CountIncreasedSeaDepthWindows(seaDepths));
    }

    [Fact]
    public void Stars()
    {
        Day01 run = new();
        Assert.Equal("1583", run.Result1());
        Assert.Equal("1627", run.Result2());
    }
}
