namespace Aoc2021.Tests;

public class Day03Tests
{
    private static readonly List<string> report = new()
    {
        "00100",
        "11110",
        "10110",
        "10111",
        "10101",
        "01111",
        "00111",
        "11100",
        "10000",
        "11001",
        "00010",
        "01010",
    };

    [Fact]
    public void Rating1Works()
    {
        Assert.Equal(new(22, 9), Day03.PowerConsumtionRate.FromReport(report));
    }

    [Fact]
    public void Rating2Works()
    {
        Assert.Equal(new(23, 10), Day03.LifeSupportRate.FromReport(report));
    }

    [Fact]
    public void Stars()
    {
        Day03 run = new();
        Assert.Equal("1071734", run.Result1());
        Assert.Equal("6124992", run.Result2());
    }
}
