namespace Aoc2021;

public class Day05 : IDay
{
    public override string Day { get; } = nameof(Day05)[3..];

    public override string Result1()
    {
        return CountMostDangerousPoints(InputLines, false)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return CountMostDangerousPoints(InputLines, true)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static int CountMostDangerousPoints(IEnumerable<string> lines, bool withDiagonalLines)
    {
        return CountCoverage(
                lines
                .Select(Line.FromString)
                .Where(line => withDiagonalLines || !isDiagonalLine(line))
            )
            .Count(pointCount => pointCount.Value >= 2);

        static bool isDiagonalLine(Line line)
        {
            return line.P1.X != line.P2.X && line.P1.Y != line.P2.Y;
        }
    }

    private static Dictionary<Point, int> CountCoverage(IEnumerable<Line> lines)
    {
        return lines.Aggregate(new Dictionary<Point, int>(), addPoints);

        static Dictionary<Point, int> addPoints(Dictionary<Point, int> grid, Line line)
        {
            return line.Points().Aggregate(grid, incrementPoint);
        }

        static Dictionary<Point, int> incrementPoint(Dictionary<Point, int> grid, Point point)
        {
            _ = grid.TryAdd(point, 0);
            grid[point] += 1;
            return grid;
        }
    }

    public record Point(int X, int Y)
    {
        public static Point FromString(string input)
        {
            string[] chunks = input.Split(",", 2);
            return new(ParseInt(chunks[0]), ParseInt(chunks[1]));
        }

    }

    public record Line(Point P1, Point P2)
    {
        public IEnumerable<Point> Points()
        {
            int dx = Math.Sign(P2.X - P1.X);
            int dy = Math.Sign(P2.Y - P1.Y);
            int n = Math.Max(Math.Abs(P2.X - P1.X), Math.Abs(P2.Y - P1.Y)) + 1;
            for (int x = P1.X, y = P1.Y; n != 0; n--, x += dx, y += dy)
            {
                yield return new(x, y);
            }
        }

        public static Line FromString(string input)
        {
            string[] chunks = input.Split(" -> ", 2);
            return new(Point.FromString(chunks[0]), Point.FromString(chunks[1]));
        }
    }
}
