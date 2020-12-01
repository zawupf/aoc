using System;
using System.Collections.Generic;
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
            return Combine2(entries).Where(sumIs2020).First();

            bool sumIs2020((int, int) entry) => entry.Item1 + entry.Item2 == sum;
        }

        static public (int, int, int) FindThreeEntriesWithSum(int sum, int[] entries)
        {
            return Combine3(entries).Where(sumIs2020).First();

            bool sumIs2020((int, int, int) entry) => entry.Item1 + entry.Item2 + entry.Item3 == sum;
        }

        static public IEnumerable<(int, int)> Combine2(IEnumerable<int> entries)
        {
            var (head, tail) = (entries.First(), entries.Skip(1));
            while (tail.Any())
            {
                foreach (var current in tail)
                {
                    yield return (head, current);
                }
                (head, tail) = (tail.First(), tail.Skip(1));
            }
        }

        static public IEnumerable<(int, int, int)> Combine3(IEnumerable<int> entries)
        {
            var (head, tail) = (entries.First(), entries.Skip(1));
            while (tail.Any())
            {
                foreach (var (first, second) in Combine2(tail))
                {
                    yield return (head, first, second);
                }
                (head, tail) = (tail.First(), tail.Skip(1));
            }
        }
    }
}
