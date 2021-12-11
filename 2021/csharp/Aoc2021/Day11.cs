namespace Aoc2021;

public class Day11 : IDay
{
    public override string Day { get; } = nameof(Day11)[3..];

    public override string Result1()
    {
        return TotalFlashCount100(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return FirstStepFullFlash(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static int TotalFlashCount100(IEnumerable<string> lines)
    {
        int[][] grid = BuildGrid(lines);
        return FlashCount(grid).Take(100).Sum();
    }

    public static int FirstStepFullFlash(IEnumerable<string> lines)
    {
        int[][] grid = BuildGrid(lines);
        return FlashCount(grid).TakeWhile(n => n != 100).Count() + 1;
    }

    private static IEnumerable<int> FlashCount(int[][] grid)
    {
        while (true)
        {
            List<(int x, int y)> flashes =
                AllPoints()
                .Aggregate(
                    new List<(int x, int y)>(),
                    (flashes, p) =>
                    {
                        int power = ++grid[p.y][p.x];
                        if (power == 10)
                        {
                            flashes.Add((p.x, p.y));
                        }
                        return flashes;
                    }
                );

            yield return Flash(flashes, grid);

            _ = AllPoints()
                .Aggregate(
                    0,
                    (_, p) =>
                    {
                        if (grid[p.y][p.x] > 9)
                        {
                            grid[p.y][p.x] = 0;
                        }
                        return _;
                    }
                );
        }
    }

    private static int Flash(IEnumerable<(int x, int y)> points, int[][] grid)
    {
        int flashCount = 0;
        foreach ((int x, int y) in points)
        {
            List<(int x, int y)> flashes = new();
            foreach ((int x, int y) p in AdjacentPoints(x, y))
            {
                int power = ++grid[p.y][p.x];
                if (power == 10)
                {
                    flashes.Add(p);
                }
            }
            flashCount += 1 + Flash(flashes, grid);
        }
        return flashCount;
    }

    private const int N = 10;

    private static int[][] BuildGrid(IEnumerable<string> lines)
    {
        return lines
            .Select(line => line.Select(c => c - '0').ToArray())
            .ToArray();
    }

    private static IEnumerable<(int x, int y)> AllPoints()
    {
        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                yield return (x, y);
            }
        }
    }

    private static IEnumerable<(int x, int y)> AdjacentPoints(int x, int y)
    {
        return new (int x, int y)[]
            {
                (x - 1, y),
                (x + 1, y),
                (x    , y - 1),
                (x    , y + 1),
                (x - 1, y - 1),
                (x + 1, y + 1),
                (x + 1, y - 1),
                (x - 1, y + 1),
            }
            .Where(p => p.x >= 0 && p.x < N && p.y >= 0 && p.y < N);
    }
}
