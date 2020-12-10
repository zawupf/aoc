using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day10 : IDay
    {
        public override string Day { get; } = nameof(Day10)[3..];

        public override string Result1() =>
            OnesTimesThrees(InputLines).ToString();

        public override string Result2() =>
            SupportedArrangementsCount(InputLines).ToString();

        public static int OnesTimesThrees(IEnumerable<string> lines)
        {
            var data = Data(lines);
            var (ones, threes) =
                data[..^1]
                .Zip(data[1..])
                .Select(ab => ab.Second - ab.First)
                .Aggregate((0, 0), (onesThrees, difference) =>
                {
                    var (ones, threes) = onesThrees;
                    return difference switch
                    {
                        1 => (ones + 1, threes),
                        3 => (ones, threes + 1),
                        _ => onesThrees,
                    };
                });
            return ones * threes;
        }

        public static long SupportedArrangementsCount(IEnumerable<string> lines)
        {
            long count = 1L;
            var data = Data(lines);
            for (int i = 0; i < data.Length; i++)
            {
                int j = i + 2;
                while (j < data.Length && data[j] - data[i] <= 3) j++;
                int n = j - i;
                if (n == 4 && j < data.Length && data[j] - data[j - 1] == 1)
                    n += 1;
                if (n > 2)
                {
                    var m = n switch
                    {
                        3 => 2,
                        4 => 4,
                        5 => 7,
                        _ => throw new ApplicationException("Damn!")
                    };
                    count *= m;
                    i += n - 2;
                }
            }
            return count;
        }

        public static int[] Data(IEnumerable<string> lines)
        {
            var input = lines.Select(int.Parse);
            var max = input.Max();
            return
                input
                .OrderBy(jolt => jolt)
                .Prepend(0)
                .Append(max + 3)
                .ToArray();
        }
    }
}
