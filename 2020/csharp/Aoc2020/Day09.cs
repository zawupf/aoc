using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day09 : IDay
    {
        public override string Day { get; } = nameof(Day09)[3..];

        public override string Result1() =>
            FindFirstInvalidNumber(Data, 25)
            .ToString();

        public override string Result2() =>
            FindWeakness(Data, long.Parse(Result1()))
            .ToString();

        private long[] Data => InputLines.Select(long.Parse).ToArray();

        public static long FindFirstInvalidNumber(long[] data, int preambleLength)
        {
            for (int i = preambleLength; i < data.Length; i++)
            {
                var preamble = data[(i - preambleLength)..i];
                var current = data[i];

                if (!Utils.Combine2(preamble)
                    .Where(pair => pair.Item1 + pair.Item2 == current)
                    .Any())
                {
                    return current;
                }
            }

            throw new ApplicationException("No invalid number found");
        }

        public static long FindWeakness(long[] data, long invalidNumber)
        {
            for (int i = 0; i < data.Length; i++)
            {
                long sum = 0;
                int j = i;
                for (; j < data.Length && sum < invalidNumber; j++)
                {
                    sum += data[j];
                }
                if (sum == invalidNumber)
                {
                    return data[i..j].Min() + data[i..j].Max();
                }
            }

            throw new ApplicationException("XMAS is too strong");
        }
    }
}
