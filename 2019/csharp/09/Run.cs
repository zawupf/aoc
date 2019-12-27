using System;
using System.Collections.Generic;
using System.Linq;
using static Aoc._2019.Utils;

namespace Aoc._2019._09
{
    public class Run : IRun
    {
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
            var code = _05.Computer.Compile(ReadInputText("09"));
            var computer = new _05.Computer(code);
            var outputs = new List<long>();
            computer.Exec(new List<long> { input }, out outputs);
            return outputs.ToArray();
        }
    }
}
