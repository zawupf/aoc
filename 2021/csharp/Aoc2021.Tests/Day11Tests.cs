namespace Aoc2021.Tests;

public class Day11Tests
{
    private static readonly List<string> input = @"
        5483143223
        2745854711
        5264556173
        6141336146
        6357385478
        4167524645
        2176841721
        6882881134
        4846848554
        5283751526
    "
    .Lines();

    [Fact]
    public void TotalFlashCount100Works()
    {
        Assert.Equal(1656, Day11.TotalFlashCount100(input));
    }

    [Fact]
    public void FirstStepFullFlashWorks()
    {
        Assert.Equal(195, Day11.FirstStepFullFlash(input));
    }

    [Fact]
    public void Stars()
    {
        Day11 run = new();
        Assert.Equal("1725", run.Result1());
        Assert.Equal("308", run.Result2());
    }
}
