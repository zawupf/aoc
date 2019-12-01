using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Aoc._2019._01
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var fuels =
                from line in ReadLines("01/input1.txt")
                select int.Parse(line) into mass
                select RequiredFuel(mass)
                ;

            return fuels.Sum().ToString();
        }

        public string Job2()
        {
            var fuels =
                from line in ReadLines("01/input1.txt")
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
