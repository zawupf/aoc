namespace Aoc2021;

public class Day06 : IDay
{
    public override string Day { get; } = nameof(Day06)[3..];

    public override string Result1()
    {
        return SimulateFishPopulation(InputText.Split(',').Select(ParseLong).ToList())
            .ElementAt(80)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return SimulateFishPopulation(InputText.Split(',').Select(ParseLong).ToList())
            .ElementAt(256)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static IEnumerable<long> SimulateFishPopulation(List<long> fishTimers)
    {
        yield return fishTimers.Count;

        long[] timers = fishTimers
            .Aggregate(
                new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                (timers, timer) =>
                {
                    timers[timer] += 1;
                    return timers;
                }
            );

        while (true)
        {
            long motherCount = timers[0];
            for (long i = 1; i != timers.Length; i++)
            {
                timers[i - 1] = timers[i];
            }
            timers[8] = motherCount;
            timers[6] += motherCount;

            yield return timers.Sum();
        }
    }
}
