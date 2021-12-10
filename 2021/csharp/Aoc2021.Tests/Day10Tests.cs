namespace Aoc2021.Tests;

public class Day10Tests
{
    private static readonly List<string> input = @"
        [({(<(())[]>[[{[]{<()<>>
        [(()[<>])]({[<{<<[]>>(
        {([(<{}[<>[]}>{[]{[(<()>
        (((({<>}<{<{<>}{[]{[]{}
        [[<[([]))<([[{}[[()]]]
        [{[{({}]{}}([{[{{{}}([]
        {<[[]]>}<{[{[{[]{()[[[]
        [<(<(<(<{}))><([]([]()
        <{([([[(<>()){}]>(<<{{
        <{([{{}}[<[[[<>{}]]]>[]]
    "
    .Trim()
    .Split('\n')
    .Select(line => line.Trim())
    .ToList();

    [Fact]
    public void TotalSyntaxErrorScoreWorks()
    {
        Assert.Equal(26397, Day10.TotalSyntaxErrorScore(input));
    }

    [Fact]
    public void MiddleCompletionScoreWorks()
    {
        Assert.Equal(288957, Day10.MiddleCompletionScore(input));
    }

    [Fact]
    public void Stars()
    {
        Day10 run = new();
        Assert.Equal("294195", run.Result1());
        Assert.Equal("3490802734", run.Result2());
    }
}
