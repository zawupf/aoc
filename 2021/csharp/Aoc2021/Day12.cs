namespace Aoc2021;

public class Day12 : IDay
{
    public override string Day { get; } = nameof(Day12)[3..];

    public override string Result1()
    {
        return PathCount(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return ExtraPathCount(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static int PathCount(IEnumerable<string> lines)
    {
        List<(string, string)> ways = Parse(lines);
        return Pathes(ways).Count();
    }

    public static int ExtraPathCount(IEnumerable<string> lines)
    {
        List<(string, string)> ways = Parse(lines);

        HashSet<string> smallCaves = ways
            .Aggregate(
                new HashSet<string>(),
                (smallCaves, way) =>
                {
                    if (IsSmallInnerCave(way.Item1))
                    {
                        _ = smallCaves.Add(way.Item1);
                    }
                    else if (IsSmallInnerCave(way.Item2))
                    {
                        _ = smallCaves.Add(way.Item2);
                    }
                    return smallCaves;
                }
            );

        return smallCaves
            .Aggregate(
                new HashSet<string>(),
                (pathes, cave) =>
                {
                    pathes.UnionWith(Pathes(ways, cave));
                    return pathes;
                }
            )
            .Count;
    }

    private static IEnumerable<string> Pathes(List<(string, string)> ways, string specialCave = "")
    {
        return Walk("start", new(), ways, specialCave);

        static IEnumerable<string> Walk(string cave, List<string> path, List<(string, string)> ways, string specialCave)
        {
            path = new List<string>(path)
            {
                cave
            };

            if (cave == "end")
            {
                yield return string.Join(',', path);
            }

            List<string> nextCaves = ways
                .Aggregate(
                    new List<string>(),
                    (caves, way) =>
                    {
                        (string cave1, string cave2) = way;
                        if (cave1 == cave)
                        {
                            caves.Add(cave2);
                        }
                        else if (cave2 == cave)
                        {
                            caves.Add(cave1);
                        }
                        return caves;
                    }
                );

            if (IsSmallCave(cave))
            {
                if (cave == specialCave)
                {
                    specialCave = "";
                }
                else
                {
                    ways = ways
                        .Where(way => way.Item1 != cave && way.Item2 != cave)
                        .ToList();
                }
            }

            foreach (string nextCave in nextCaves)
            {
                IEnumerable<string> pathes = Walk(nextCave, path, ways, specialCave);
                foreach (string p in pathes)
                {
                    yield return p;
                }
            }
        }
    }

    private static List<(string, string)> Parse(IEnumerable<string> lines)
    {
        return lines
            .Select(line =>
            {
                string[] chunks = line.Split('-');
                return (chunks[0], chunks[1]);
            })
            .ToList();
    }

    private static readonly HashSet<char> lowerLetters = "abcdefghijklmnopqrstuvwxyz".ToHashSet();

    private static bool IsSmallCave(string name)
    {
        return lowerLetters.Contains(name[0]);
    }

    private static bool IsSmallInnerCave(string name)
    {
        return name is not "start" and not "end" && lowerLetters.Contains(name[0]);
    }
}
