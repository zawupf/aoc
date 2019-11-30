using System;

namespace Aoc._2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Advent Of Code!");

            Run(0, new _00.Run());

            return;

            void Run(int day, _00.IRun run)
            {
                var suffix = day == 0 ? "" : " - Template";
                Dump($"\nDay {day}{suffix}");
                Dump($"Result 1: {run.Job1()}");
                Dump($"Result 2: {run.Job2()}");
            }

            void Dump(string message) => Console.WriteLine(message);
        }
    }
}
