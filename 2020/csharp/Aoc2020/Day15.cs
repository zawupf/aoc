using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day15 : IDay
    {
        public override string Day { get; } = nameof(Day15)[3..];

        public override string Result1() =>
            Play(InputText).ElementAt(2020 - 1).ToString();

        public override string Result2() =>
            Play(InputText).ElementAt(30000000 - 1).ToString();

        public static IEnumerable<int> Play(string input)
        {
            int turn = 0;
            int last = -1;
            int[] startingNumbers = input.Split(',').Select(int.Parse).ToArray();
            foreach (var number in startingNumbers)
            {
                yield return number;
            }

            var turns =
                startingNumbers
                .Aggregate(
                    new Dictionary<int, (int prev, int prePrev)>(),
                    (dict, number) =>
                    {
                        last = number;
                        dict.Add(last, (++turn, 0));
                        return dict;
                    }
                );

            var invalid = (0, 0);
            while (true)
            {
                int next = Diff(turns.GetValueOrDefault(last, invalid));

                yield return next;

                int nextPrePrev = First(turns.GetValueOrDefault(next, invalid));
                turns[next] = (++turn, nextPrePrev);

                last = next;
            }

            int Diff((int a, int b) v) => v.b != 0 ? v.a - v.b : 0;
            int First((int a, int b) v) => v.a;
        }
    }
}
