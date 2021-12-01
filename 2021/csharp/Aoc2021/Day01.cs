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

    public static int CountIncreasedSeaDepths(IEnumerable<int> seaDepths)
    {
        int count = 0;
        int previousSeaDepth = seaDepths.First();
        (int count, int previousSeaDepth) result = seaDepths.Skip(1).Aggregate(
            (count, previousSeaDepth),
            (acc, currentSeaDepth) =>
            {
                (int count, int previousSeaDepth) = acc;
                int newCount = count + (currentSeaDepth > previousSeaDepth ? 1 : 0);
                return (newCount, currentSeaDepth);
            });
        return result.count;
    }

    public static int CountIncreasedSeaDepthWindows(IEnumerable<int> seaDepths)
    {
        int first = seaDepths.First();
        int second = seaDepths.Skip(1).First();
        List<int> result = new();
        (List<int> windows, _, _) = seaDepths.Skip(2).Aggregate(
            (result, first, second),
            (acc, third) =>
            {
                (List<int> result, int first, int second) = acc;
                result.Add(first + second + third);
                return (result, second, third);
            });
        return CountIncreasedSeaDepths(windows);
    }
}
