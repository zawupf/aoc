using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc._2019._09
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            return Boost(1).Last().ToString();
        }

        public string Job2()
        {
            return Boost(2).Last().ToString();
        }

        public long[] Boost(long input)
        {
            var code = _05.Computer.Compile(ReadAllText("09/input1.txt"));
            var computer = new _05.Computer(code);
            var outputs = new List<long>();
            computer.Exec(new List<long> { input }, out outputs);
            return outputs.ToArray();
        }
    }
}
