using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day16 : IDay
    {
        public override string Day { get; } = nameof(Day16)[3..];

        public override string Result1() =>
            null;

        public override string Result2() =>
            null;
    }

    public record Range(int Min, int Max);

    public record Rule(string Name, Range Range1, Range Range2);
}
