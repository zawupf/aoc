namespace Aoc2021;

public class Day00 : IDay
{
    public override string Day { get; } = nameof(Day00)[3..];

    public override string Result1()
    {
        IEnumerable<int> fuels =
            from line in InputLines
            select ParseInt(line) into mass
            select RequiredFuel(mass)
            ;

        return fuels.Sum().ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        IEnumerable<int> fuels =
            from line in InputLines
            select ParseInt(line) into mass
            select RequiredTotalFuel(mass)
            ;

        return fuels.Sum().ToString(CultureInfo.InvariantCulture);
    }

    public static int RequiredFuel(int mass)
    {
        return (mass / 3) - 2;
    }

    public static int RequiredTotalFuel(int mass)
    {
        return fuelChunks(mass).Sum();

        static IEnumerable<int> fuelChunks(int mass)
        {
            int fuel = RequiredFuel(mass);
            if (fuel > 0)
            {
                yield return fuel;
                foreach (int fuelChunk in fuelChunks(fuel))
                {
                    yield return fuelChunk;
                }
            }
        }
    }
}
