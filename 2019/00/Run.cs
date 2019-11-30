using System;

namespace Aoc._2019._00
{
    public interface IRun
    {
        public string Job1();
        public string Job2();
    };

    public class Run : IRun
    {
        public string Job1()
        {
            return "job1 dummy output";
        }

        public string Job2()
        {
            return "job2 dummy output";
        }
    }
}
