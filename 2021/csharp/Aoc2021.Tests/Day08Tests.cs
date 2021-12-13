namespace Aoc2021.Tests;

public class Day08Tests
{
    private static readonly List<string> input = @"
        be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
        edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
        fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
        fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
        aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
        fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
        dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
        bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
        egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
        gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce
    "
    .Trim()
    .Split('\n')
    .Select(line => line.Trim())
    .ToList();

    [Fact]
    public void CountUniqueOutputDigitsWorks()
    {
        Assert.Equal(26, Day08.CountUniqueOutputDigits(input));
    }

    [Fact]
    public void DecodeSumWorks()
    {
        Assert.Equal(5353, Day08.DecodeSum(new string[] { "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf" }));
        Assert.Equal(61229, Day08.DecodeSum(input));
    }

    [Fact]
    public void Stars()
    {
        Day08 run = new();
        Assert.Equal("534", run.Result1());
        Assert.Equal("1070188", run.Result2());
    }
}