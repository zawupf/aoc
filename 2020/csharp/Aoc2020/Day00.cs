using System.Linq;
using System.Collections.Generic;

namespace Aoc2020
{
    public class Day00 : IDay
    {
        public override string Day { get; } = nameof(Day00)[3..];

        public override string Result1()
        {
            var fuels =
                from line in InputLines
                select int.Parse(line) into mass
                select RequiredFuel(mass)
                ;

            return fuels.Sum().ToString();
        }

        public override string Result2()
        {
            var fuels =
                from line in InputLines
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
