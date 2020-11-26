using System.Linq;
using System.Collections.Generic;
using static Aoc2020Calendar.Utils;

namespace Aoc2020Calendar
{
    public class Day00 : IDay
    {
        public string Day { get; } = nameof(Day00)[3..];

        public string Result1()
        {
            var fuels =
                from line in ReadInputLines(Day)
                select int.Parse(line) into mass
                select RequiredFuel(mass)
                ;

            return fuels.Sum().ToString();
        }

        public string Result2()
        {
            var fuels =
                from line in ReadInputLines(Day)
                select int.Parse(line) into mass
                select RequiredTotalFuel(mass)
                ;

            return fuels.Sum().ToString();
        }

        static public int RequiredFuel(int mass) => mass / 3 - 2;

        static public int RequiredTotalFuel(int mass)
        {
            return fuelChunks(mass).Sum();

            IEnumerable<int> fuelChunks(int mass)
            {
                var fuel = RequiredFuel(mass);
                if (fuel > 0)
                {
                    yield return fuel;
                    foreach (var fuelChunk in fuelChunks(fuel))
                        yield return fuelChunk;
                }
            }
        }
    }
}
