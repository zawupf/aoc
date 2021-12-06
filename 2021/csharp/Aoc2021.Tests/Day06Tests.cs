namespace Aoc2021.Tests;

public class Day06Tests
{
    private static readonly List<long> input = "3,4,3,1,2"
        .Split(',').Select(Utils.ParseLong).ToList();

    [Fact]
    public void SimulateWorks()
    {
        Assert.Equal(26, Day06.SimulateFishPopulation(input).ElementAt(18));
        Assert.Equal(5934, Day06.SimulateFishPopulation(input).ElementAt(80));
        Assert.Equal(26984457539, Day06.SimulateFishPopulation(input).ElementAt(256));
    }

    [Fact]
    public void Stars()
    {
        Day06 run = new();
        Assert.Equal("350605", run.Result1());
        Assert.Equal("1592778185024", run.Result2());
    }
}
