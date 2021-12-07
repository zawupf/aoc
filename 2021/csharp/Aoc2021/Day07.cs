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
        List<(int position, int count)> positionCount = positions
            .GroupBy(
                position => position,
                (position, positions) => (position, positions.Count())
            )
            .ToList();

        (int position, int fuel) result = Fuels().MinBy(position => position.fuel);
        return result;

        List<(int position, int fuel)> Fuels()
        {
            int min = positionCount.MinBy(pc => pc.position).position;
            int max = positionCount.MaxBy(pc => pc.position).position;

            return Enumerable.Range(min, max - min + 1)
                .Select(position => (position, Fuel(position)))
                .ToList();
        }

        int Fuel(int position)
        {
            return positionCount
                .Select(item => item.count * distanceToFuel(Math.Abs(item.position - position)))
                .Sum();
        }
    }
}
