namespace Aoc2021;

public class Day07 : IDay
{
    public override string Day { get; } = nameof(Day07)[3..];

    public override string Result1()
    {
        return FindBestPostion(InputText.Split(',').Select(ParseInt).ToList())
            .fuel
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return FindBestCrabPostion(InputText.Split(',').Select(ParseInt).ToList())
            .fuel
            .ToString(CultureInfo.InvariantCulture);
    }

    public static (int position, int fuel) FindBestPostion(IEnumerable<int> positions)
    {
        return FindMinFuelPosition(positions, d => d);
    }

    public static (int position, int fuel) FindBestCrabPostion(IEnumerable<int> positions)
    {
        return FindMinFuelPosition(positions, d => d * (d + 1) / 2);
    }

    public static (int position, int fuel) FindMinFuelPosition(
        IEnumerable<int> positions,
        Func<int, int> distanceToFuel
    )
    {
        Dictionary<int, int>? positionCount = positions
            .GroupBy(
                position => position,
                (position, positions) => (position, count: positions.Count())
            )
            .ToDictionary(
                item => item.position,
                item => item.count
            );

        (int position, int fuel) result = CalculateFuels().MinBy(position => position.fuel);
        return result;

        IEnumerable<(int position, int fuel)> CalculateFuels()
        {
            int min = positionCount.Keys.Min();
            int max = positionCount.Keys.Max();

            return Enumerable.Range(min, max - min + 1).Select(position => (position, CalculateFuel(position)));
        }

        int CalculateFuel(int position)
        {
            return positionCount
                .Select(item => item.Value * distanceToFuel(Math.Abs(item.Key - position)))
                .Sum();
        }
    }
}
