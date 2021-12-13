namespace Aoc2021;

public class Day09 : IDay
{
    public override string Day { get; } = nameof(Day09)[3..];

    public override string Result1()
    {
        return RiskLevelSum(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return BasinProduct(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    private static int[][] BuildGrid(IEnumerable<string> lines)
    {
        return lines
            .Select(line => line.ToCharArray().Select(c => c - '0').ToArray())
            .ToArray();
    }

    private static IEnumerable<(int x, int y, int height)> LowPoints(int[][] grid)
    {
        int yLength = grid.Length;
        int xLength = grid[0].Length;

        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                int height = grid[y][x];
                if (AdjacentPoints(x, y, grid)
                    .Select(p => grid[p.y][p.x])
                    .All(h => height < h))
                {
                    yield return (x, y, height);
                }
            }
        }
    }

    private static IEnumerable<(int x, int y)> AdjacentPoints(int x, int y, int[][] grid)
    {
        int yLength = grid.Length;
        int xLength = grid[0].Length;

        return new (int x, int y)[]
        {
            (x-1, y),
            (x+1, y),
            (x, y-1),
            (x, y+1),
        }
        .Where(p => p.x >= 0 && p.x < xLength && p.y >= 0 && p.y < yLength);

    }

    public static int RiskLevelSum(IEnumerable<string> lines)
    {
        int[][] grid = BuildGrid(lines);
        return LowPoints(grid).Sum(item => item.height + 1);
    }

    public static int BasinProduct(IEnumerable<string> lines)
    {
        int[][] grid = BuildGrid(lines);

        return LowPoints(grid)
            .Select(p => basinSize(p.x, p.y))
            .OrderByDescending(size => size)
            .Take(3)
            .Aggregate(1, (acc, v) => acc * v);

        int basinSize(int x, int y)
        {
            HashSet<(int, int)> basinPoints = new() { (x, y) };
            buildBasin(x, y, basinPoints);
            return basinPoints.Count;
        }

        void buildBasin(int x, int y, HashSet<(int, int)> basinPoints)
        {
            int height = grid[y][x];
            foreach ((int x, int y) p in AdjacentPoints(x, y, grid))
            {
                if (!basinPoints.Contains(p) && grid[p.y][p.x] != 9)
                {
                    _ = basinPoints.Add(p);
                    buildBasin(p.x, p.y, basinPoints);
                }
            }
        }
    }
}
