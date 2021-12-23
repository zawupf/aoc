namespace Aoc2021.Tests;

public class Day21Tests
{
    private static readonly List<string> input = @"
        Player 1 starting position: 4
        Player 2 starting position: 8
    "
    .Trim()
    .Split('\n')
    .Select(line => line.Trim())
    .ToList();

    [Fact]
    public void DeterministicDieWorks()
    {
        Day21.DeterministicDie die = new();
        Assert.Equal(1, die.Roll());
        Assert.Equal(2, die.Roll());
        Assert.Equal(3, die.Roll());
        Assert.Equal(3, die.RollCount);

        Assert.Equal((97 * 98 / 2) - 6, die.Roll(94));
        Assert.Equal(97, die.RollCount);

        Assert.Equal(98, die.Roll());
        Assert.Equal(99, die.Roll());
        Assert.Equal(100, die.Roll());
        Assert.Equal(100, die.RollCount);

        Assert.Equal(1, die.Roll());
        Assert.Equal(2, die.Roll());
        Assert.Equal(3, die.Roll());
        Assert.Equal(103, die.RollCount);
    }

    [Fact]
    public void WarmupGameWorks()
    {
        Assert.Equal(
            new int[] { 10, 3, 14, 9, 20, 16, 26, 22 },
            Day21.WarmupGame
                .Init(input)
                .Play()
                .Select(game => game.CurrentPlayer.Score)
                .Take(8)
        );
        Assert.Equal(
            new int[] { 742, 990, 745, 1000 },
            Day21.WarmupGame
                .Init(input)
                .Play()
                .Select(game => game.CurrentPlayer.Score)
                .TakeLast(4)
        );
        Assert.Equal(
            739785,
            Day21.WarmupGame
                .Init(input)
                .Play()
                .Last()
                .Result1
        );
    }

    [Fact]
    public void DiracGameWorks()
    {
        Assert.Equal(
            444356092776315L,
            Day21.DiracGame
                .Init(input)
                .Play()
                .Last()
                .MasterOfTheMultiverseCount
        );
    }

    [Fact]
    public void Stars()
    {
        Day21 run = new();
        Assert.Equal("926610", run.Result1());
        Assert.Equal("146854918035875", run.Result2());
    }
}
