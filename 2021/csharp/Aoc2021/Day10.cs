namespace Aoc2021;

public class Day10 : IDay
{
    public override string Day { get; } = nameof(Day10)[3..];

    public override string Result1()
    {
        return TotalSyntaxErrorScore(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return MiddleCompletionScore(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static int TotalSyntaxErrorScore(IEnumerable<string> lines)
    {
        return lines
            .Select(SyntaxErrorScore)
            .Sum();
    }

    public static long MiddleCompletionScore(IEnumerable<string> lines)
    {
        List<long> scores = lines
            .Where(line => SyntaxErrorScore(line) == 0)
            .Select(CompletionScore)
            .ToList();
        scores.Sort();
        return scores[scores.Count / 2];

        static long CompletionScore(string line)
        {
            return line
                .Aggregate(new List<char>(), (openings, c) =>
                {
                    if (c is '(' or '[' or '{' or '<')
                    {
                        openings.Add(c);
                    }
                    else
                    {
                        openings.RemoveAt(openings.Count - 1);
                    }
                    return openings;
                })
                .Select(c => c switch
                {
                    '(' => ')',
                    '[' => ']',
                    '{' => '}',
                    '<' => '>',
                    _ => '\0'
                })
                .Reverse()
                .Aggregate(0L, (score, c) =>
                {
                    long points = c switch
                    {
                        ')' => 1L,
                        ']' => 2L,
                        '}' => 3L,
                        '>' => 4L,
                        _ => '\0'
                    };
                    return (score * 5L) + points;
                });
        }
    }

    private static int SyntaxErrorScore(string line)
    {
        List<char> openings = new();
        foreach (char c in line)
        {
            if (c is '(' or '[' or '{' or '<')
            {
                openings.Add(c);
                continue;
            }

            char expectedClosing = openings.Last() switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                '<' => '>',
                _ => '\0'
            };
            if (c != expectedClosing)
            {
                return c switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137,
                    _ => throw new ApplicationException("Invalid char"),
                };
            }
            openings.RemoveAt(openings.Count - 1);
        }
        return 0;
    }
}
