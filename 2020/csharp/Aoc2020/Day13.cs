using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day13 : IDay
    {
        public override string Day { get; } = nameof(Day13)[3..];

        public override string Result1() =>
            DepartureInfo(InputLines).result.ToString();

        public override string Result2() => null;

        public static (long busId, long wait, long result)
            DepartureInfo(IEnumerable<string> input)
        {
            string[] lines = input.ToArray();
            long timestamp = long.Parse(lines[0]);
            long[] ids =
                lines[1].Split(',')
                .Where(value => value != "x")
                .Select(long.Parse)
                .ToArray();

            for (var departureTime = timestamp; true; departureTime++)
            {
                foreach (var busId in ids)
                {
                    if (departureTime % busId == 0)
                        return (
                            busId,
                            departureTime - timestamp,
                            busId * (departureTime - timestamp));
                }
            }
        }
    }
}
