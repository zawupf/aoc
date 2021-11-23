namespace Aoc2021.Tests;

public class Day01Tests
{
    [Fact]
    public void FindTwoEntriesWithSumWorks()
    {
        int[] expenseReport = { 1721, 979, 366, 299, 675, 1456 };
        Assert.Equal((1721, 299), Day01.FindTwoEntriesWithSum(2020, expenseReport));
    }

    [Fact]
    public void FindThreeEntriesWithSumWorks()
    {
        int[] expenseReport = { 1721, 979, 366, 299, 675, 1456 };
        Assert.Equal((979, 366, 675), Day01.FindThreeEntriesWithSum(2020, expenseReport));
    }

    [Fact]
    public void Stars()
    {
        Day01 run = new();
        Assert.Equal("1018944", run.Result1());
        Assert.Equal("8446464", run.Result2());
    }
}
