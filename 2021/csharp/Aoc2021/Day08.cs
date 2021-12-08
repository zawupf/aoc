namespace Aoc2021;

public class Day08 : IDay
{
    public override string Day { get; } = nameof(Day08)[3..];

    public override string Result1()
    {
        return CountUniqueOutputDigits(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return DecodeSum(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static int CountUniqueOutputDigits(IEnumerable<string> lines)
    {
        return lines
            .Select(line => line.Split(' ')[11..].Sum(isUniqueDigit))
            .Sum();

        static int isUniqueDigit(string digit)
        {
            // digits: 1, 4, 7, 8 -> length: 2, 4, 3, 7
            return digit.Length is 2 or 4 or 3 or 7 ? 1 : 0;
        }
    }

    public static int DecodeSum(IEnumerable<string> lines)
    {
        return lines
            .Select(decode)
            .Sum();

        static int decode(string line)
        {
            List<string> wrongDigits = line
                .Split(' ')[..10]
                .Select(SortSegments)
                .ToList();
            string wrong1 = wrongDigits.First(digit => digit.Length == 2); //   c  f
            string wrong4 = wrongDigits.First(digit => digit.Length == 4); //  bcd f
            string wrong7 = wrongDigits.First(digit => digit.Length == 3); // a c  f
            string wrong8 = wrongDigits.First(digit => digit.Length == 7); // abcdefg

            // Above wrong digits give the following
            // Permutation sequence: c f -> b d -> a -> e g
            IEnumerable<IEnumerable<char>> cfPerms = Permutations(wrong1, 2);
            foreach (IEnumerable<char> cf in cfPerms)
            {
                char c = cf.First();
                char f = cf.Skip(1).First();

                IEnumerable<IEnumerable<char>> bdPerms = Permutations(wrong4.Except(wrong1), 2);
                foreach (IEnumerable<char> bd in bdPerms)
                {
                    char b = bd.First();
                    char d = bd.Skip(1).First();

                    char a = wrong7.Except(wrong4).First();

                    IEnumerable<IEnumerable<char>> egPerms =
                        Permutations(wrong8.Except(wrong4).Except(wrong7), 2);
                    foreach (IEnumerable<char> eg in egPerms)
                    {
                        char e = eg.First();
                        char g = eg.Skip(1).First();

                        Dictionary<char, char> map = new()
                        {
                            [a] = 'a',
                            [b] = 'b',
                            [c] = 'c',
                            [d] = 'd',
                            [e] = 'e',
                            [f] = 'f',
                            [g] = 'g',
                        };

                        HashSet<string> decodedDigits =
                            wrongDigits
                            .Select(digit => SortSegments(digit.Select(segment => map[segment])))
                            .ToHashSet();

                        if (digits.All(digit => decodedDigits.Contains(digit)))
                        {
                            List<string> resultDigits = line.Split(' ')[11..].ToList();
                            List<string> output = resultDigits
                                .Select(digit =>
                                {
                                    string decodedDigit = SortSegments(digit.Select(segment => map[segment]));
                                    int number = digits.FindIndex(d => d == decodedDigit);
                                    return number.ToString(CultureInfo.InvariantCulture);
                                })
                                .ToList();
                            return ParseInt(string.Join("", output));
                        }
                    }
                }
            }

            throw new ApplicationException("Ouch, no permutation found");
        }
    }

    private static List<string> digits = new()
    {
        "abcefg",
        "cf",
        "acdeg",
        "acdfg",
        "bcdf",
        "abdfg",
        "abdefg",
        "acf",
        "abcdefg",
        "abcdfg",
    };

    private static string SortSegments(IEnumerable<char> segments)
    {
        List<char> digit = segments.ToList();
        digit.Sort();
        return string.Join("", digit);
    }
}
