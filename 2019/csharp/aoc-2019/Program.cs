using System;

namespace Aoc._2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Advent Of Code!");

            // Run(0, new _00.Run());
            // Run(1, new _01.Run());
            // Run(2, new _02.Run());
            // Run(3, new _03.Run());
            // Run(4, new _04.Run());
            // Run(5, new _05.Run());
            // Run(6, new _06.Run());
            // Run(7, new _07.Run());
            // Run(8, new _08.Run());
            // Run(9, new _09.Run());
            // Run(10, new _10.Run());
            // Run(11, new _11.Run());
            // Run(12, new _12.Run());
            Run(13, new _13.Run());

            return;

            void Run(int day, IRun run)
            {
                var suffix = day == 0 ? " - Template" : "";
                Dump($"\nDay {day}{suffix}");
                try
                {
                    Dump($"Result 1: {run.Job1()}");
                    Dump($"Result 2: {run.Job2()}");
                }
                catch (NotImplementedException)
                {
                }
            }

            void Dump(string message) => Console.WriteLine(message);
        }
    }
}
