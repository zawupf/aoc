using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day11 : IDay
    {
        public override string Day { get; } = nameof(Day11)[3..];

        public override string Result1() => null;

        public override string Result2() => null;

        public char[,] Data(IEnumerable<string> lines)
        {
            return new char[1, 1];
        }

        public record Seats(string[] Rows)
        {
            public enum Type
            {
                Floor = '.',
                EmptySeat = 'L',
                OccupiedSeat = '#',
            }

            public static Seats Parse(IEnumerable<string> lines) =>
                new(lines.ToArray());

            public Seats Next()
            {
                var (height, width) = (Rows.Length, Rows[0].Length);
                var nextRows = new string[height];
                foreach (var row in Rows.)
                {

                }
            }
        }
    }
}
