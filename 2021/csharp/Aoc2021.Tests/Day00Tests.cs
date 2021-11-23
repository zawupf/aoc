namespace Aoc2021.Tests;

public class Day00Tests
{
    [Theory]
    [InlineData(2, 12)]
    [InlineData(2, 14)]
    [InlineData(654, 1969)]
    [InlineData(33583, 100756)]
    public void RequiredFuelWorks(int fuel, int mass)
    {
        Assert.Equal(fuel, Day00.RequiredFuel(mass));
    }

    [Theory]
    [InlineData(2, 14)]
    [InlineData(966, 1969)]
    [InlineData(50346, 100756)]
    public void RequiredTotalFuelWorks(int fuel, int mass)
    {
        Assert.Equal(fuel, Day00.RequiredTotalFuel(mass));
    }

    [Fact]
    public void Stars()
    {
        Day00 run = new();
        Assert.Equal("3421505", run.Result1());
        Assert.Equal("5129386", run.Result2());
    }
}
