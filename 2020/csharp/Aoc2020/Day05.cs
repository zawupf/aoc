using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day05 : IDay
    {
        public override string Day { get; } = nameof(Day05)[3..];

        public override string Result1() =>
            InputLines
            .Select(SeatId)
            .Max()
            .ToString();

        public override string Result2()
        {
            var usedSeatIds = InputLines.Select(SeatId).ToHashSet();
            return
                Enumerable.Range(0, 1024)
                .ToHashSet()
                .Except(usedSeatIds)
                .First(seatId => usedSeatIds.Contains(seatId + 1) && usedSeatIds.Contains(seatId - 1))
                .ToString();
        }

        public static int SeatId(string code) => Convert.ToInt32(
            code
            .Replace('F', '0')
            .Replace('B', '1')
            .Replace('L', '0')
            .Replace('R', '1'),
            2);

        public static int Row(string code) =>
            Convert.ToInt32(code[..7].Replace('F', '0').Replace('B', '1'), 2);

        public static int Column(string code) =>
            Convert.ToInt32(code[7..].Replace('L', '0').Replace('R', '1'), 2);
    }
}
