namespace Aoc2021;

public class Day01 : IDay
{
    public override string Day { get; } = nameof(Day01)[3..];

    public override string Result1()
    {
        int[] expenseReport = InputLines.Select((line) => ParseInt(line)).ToArray();
        (int entry1, int entry2) = FindTwoEntriesWithSum(2020, expenseReport);
        return (entry1 * entry2).ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        int[] expenseReport = InputLines.Select((line) => ParseInt(line)).ToArray();
        (int entry1, int entry2, int entry3) = FindThreeEntriesWithSum(2020, expenseReport);
        return (entry1 * entry2 * entry3).ToString(CultureInfo.InvariantCulture);
    }

    public static (int, int) FindTwoEntriesWithSum(int sum, int[] entries)
    {
        return Combine2(entries).First(sumIs2020);

        bool sumIs2020((int, int) entry)
        {
            return entry.Item1 + entry.Item2 == sum;
        }
    }

    public static (int, int, int) FindThreeEntriesWithSum(int sum, int[] entries)
    {
        return Combine3(entries).First(sumIs2020);

        bool sumIs2020((int, int, int) entry)
        {
            return entry.Item1 + entry.Item2 + entry.Item3 == sum;
        }
    }

    public static IEnumerable<(int, int, int)> Combine3(IEnumerable<int> entries)
    {
        (int head, IEnumerable<int> tail) = (entries.First(), entries.Skip(1));
        while (tail.Any())
        {
            foreach ((int first, int second) in Combine2(tail))
            {
                yield return (head, first, second);
            }
            (head, tail) = (tail.First(), tail.Skip(1));
        }
    }
}
