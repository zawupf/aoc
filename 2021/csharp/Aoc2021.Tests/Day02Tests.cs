namespace Aoc2021.Tests;

public class Day02Tests
{
    private static readonly List<Day02.Command> commands = Day02.ParseCommands(new[]
    {
        "forward 5",
        "down 5",
        "forward 8",
        "up 3",
        "down 8",
        "forward 2",
    });

    [Fact]
    public void MovingPart1Works()
    {
        Assert.Equal(new(15, 10, 0), Day02.MoveFirstTry(new(0, 0, 0), commands));
    }

    [Fact]
    public void MovingPart2Works()
    {
        Assert.Equal(new(15, 60, 0), Day02.MoveSecondTry(new(0, 0, 0), commands) with { Aim = 0 });
    }

    [Fact]
    public void Stars()
    {
        Day02 run = new();
        Assert.Equal("1507611", run.Result1());
        Assert.Equal("1880593125", run.Result2());
    }
}
