using System;
using System.Linq;

namespace Aoc2020
{
    public class Day01 : IDay
    {
        public override string Day { get; } = nameof(Day01)[3..];

        public override string Result1()
        {
            int[] expenseReport = InputLines.Select((line) => int.Parse(line)).ToArray();
            var (entry1, entry2) = FindTwoEntriesWithSum(2020, expenseReport);
            return (entry1 * entry2).ToString();
        }

        public override string Result2()
        {
            int[] expenseReport = InputLines.Select((line) => int.Parse(line)).ToArray();
            var (entry1, entry2, entry3) = FindThreeEntriesWithSum(2020, expenseReport);
            return (entry1 * entry2 * entry3).ToString();
        }

        static public (int, int) FindTwoEntriesWithSum(int sum, int[] entries)
        {
            var (head, tail) = (entries[0], entries[1..]);
            while (tail.Length != 0)
            {
                foreach (var current in tail)
                {
                    if (head + current == sum)
                    {
                        return (head, current);
                    }
                }
                (head, tail) = (tail[0], tail[1..]);
            }

            throw new ArgumentException($"No two entires with sum {sum} found");
        }

        static public (int, int, int) FindThreeEntriesWithSum(int sum, int[] entries)
        {
            var (head, tail) = (entries[0], entries[1..]);
            while (tail.Length != 0)
            {
                try
                {
                    var (first, second) = FindTwoEntriesWithSum(sum - head, tail);
                    return (head, first, second);
                }
                catch (System.ArgumentException)
                {
                    (head, tail) = (tail[0], tail[1..]);
                }
            }

            throw new ArgumentException($"No three values with sum {sum} found");
        }
    }
}
