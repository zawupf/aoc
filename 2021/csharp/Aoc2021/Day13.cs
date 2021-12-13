namespace Aoc2021;

public class Day13 : IDay
{
    public override string Day { get; } = nameof(Day13)[3..];

    public override string Result1()
    {
        return DotCount(InputLines).First()
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return Fold(InputLines);
    }

    public static IEnumerable<int> DotCount(IEnumerable<string> lines)
    {
        (List<(int, int)> dots, List<(string, int)> foldInstructions) = Parse(lines);

        foreach ((string, int) foldInstruction in foldInstructions)
        {
            dots = Fold(dots, foldInstruction);
            yield return dots.Count;
        }
    }

    public static string Fold(IEnumerable<string> lines)
    {
        (List<(int, int)> dots, List<(string, int)> foldInstructions) = Parse(lines);

        foreach ((string, int) foldInstruction in foldInstructions)
        {
            dots = Fold(dots, foldInstruction);
        }

        (int mx, int my) = dots
            .Aggregate(
                (x: 0, y: 0),
                (n, dot) =>
                {
                    (int x, int y) = dot;
                    return (x > n.x ? x : n.x, y > n.y ? y : n.y);
                }
            );

        List<List<char>> result = Enumerable.Range(0, my + 1)
            .Select(y => new string(' ', mx + 1).ToList())
            .ToList();

        foreach ((int, int) dot in dots)
        {
            (int x, int y) = dot;
            result[y][x] = '#';
        }

        return "\n" + string.Join('\n', result.Select(line => string.Join("", line)));
    }

    private static List<(int, int)> Fold(List<(int, int)> dots, (string, int) foldInstruction)
    {
        (string direction, int offset) = foldInstruction;
        return dots
            .Select(dot =>
            {
                (int x, int y) = dot;
                bool flipY = direction == "y";
                int n = flipY ? y : x;
                return n < offset
                    ? dot
                    : (flipY ? x : Flip(n, offset), flipY ? Flip(n, offset) : y);
            })
            .Distinct()
            .ToList();
    }

    private static int Flip(int i, int offset)
    {
        return offset - (i - offset);
    }

    private static (List<(int, int)> dots, List<(string, int)> foldInstructions) Parse(IEnumerable<string> lines)
    {
        return lines
            .Aggregate(
                (dots: new List<(int, int)>(), foldInstructions: new List<(string, int)>()),
                (acc, line) =>
                {
                    if (line.Contains(','))
                    {
                        int[] coords = line.Split(',', 2).Select(ParseInt).ToArray();
                        acc.dots.Add((coords[0], coords[1]));
                    }
                    else if (line.StartsWith("fold along ", false, CultureInfo.InvariantCulture))
                    {
                        string[] chunks = line[11..].Split('=');
                        acc.foldInstructions.Add((chunks[0], ParseInt(chunks[1])));
                    }
                    return acc;
                }
            );
    }
}
