namespace Aoc2021;

public class Day01 : IDay
{
    public override string Day { get; } = nameof(Day01)[3..];

    public override string Result1()
    {
        int[] seaDepths = InputLines.Select((line) => ParseInt(line)).ToArray();
        int result = CountIncreasedSeaDepths(seaDepths);
        return result.ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        int[] seaDepths = InputLines.Select((line) => ParseInt(line)).ToArray();
        int result = CountIncreasedSeaDepthWindows(seaDepths);
        return result.ToString(CultureInfo.InvariantCulture);
    }

    private static int SmartCompareCount(int offset, IEnumerable<int> sequence)
    {
        return sequence
            .Zip(sequence.Skip(offset))
            .Count((pair) => pair.Second > pair.First);
    }

    public static int CountIncreasedSeaDepths(IEnumerable<int> seaDepths)
    {
        return SmartCompareCount(1, seaDepths);
    }

    public static int CountIncreasedSeaDepthWindows(IEnumerable<int> seaDepths)
    {
        return SmartCompareCount(3, seaDepths);
    }
}
