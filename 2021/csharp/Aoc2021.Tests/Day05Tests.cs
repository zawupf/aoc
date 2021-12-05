namespace Aoc2021.Tests;

public class Day05Tests
{
    private static readonly List<string> input = @"
        0,9 -> 5,9
        8,0 -> 0,8
        9,4 -> 3,4
        2,2 -> 2,1
        7,0 -> 7,4
        6,4 -> 2,0
        0,9 -> 2,9
        3,4 -> 1,4
        0,0 -> 8,8
        5,5 -> 8,2
    "
    .Trim()
    .Split('\n')
    .Select(line => line.Trim())
    .ToList();

    [Fact]
    public void InputDataIsCorrect()
    {
        Assert.Equal(10, input.Count);
    }

    [Fact]
    public void StraightLinesOnlyWorks()
    {
        Assert.Equal(5, Day05.CountMostDangerousPoints(input, false));
    }

    [Fact]
    public void AllLinesWorks()
    {
        Assert.Equal(12, Day05.CountMostDangerousPoints(input, true));
    }

    [Fact]
    public void Stars()
    {
        Day05 run = new();
        Assert.Equal("8622", run.Result1());
        Assert.Equal("22037", run.Result2());
    }
}
