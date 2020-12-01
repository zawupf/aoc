using System;

namespace Aoc2020
{
    class Program
    {
        static void Main(string[] _)
        {
            Console.WriteLine("Hello, Advent Of Code 2020!");

            // Run<Day00>();
            Run<Day01>();
            // Run<Day02>();
            // Run<Day03>();
            // Run<Day04>();
            // Run<Day05>();
            // Run<Day06>();
            // Run<Day07>();
            // Run<Day08>();
            // Run<Day09>();
            // Run<Day10>();
            // Run<Day11>();
            // Run<Day12>();
            // Run<Day13>();
            // Run<Day14>();
            // Run<Day15>();
            // Run<Day16>();
            // Run<Day17>();
            // Run<Day18>();
            // Run<Day19>();
            // Run<Day20>();
            // Run<Day21>();
            // Run<Day22>();
            // Run<Day23>();
            // Run<Day24>();
            // Run<Day25>();

            return;

            void Run<T>() where T : IDay, new()
            {
                var run = new T();
                var day = int.Parse(run.Day);
                var suffix = day == 0 ? " - Test" : "";
                Dump($"\nDay {day}{suffix}");
                try
                {
                    Dump($"Result 1: {run.Result1()}");
                    Dump($"Result 2: {run.Result2()}");
                }
                catch (NotImplementedException)
                {
                }
            }

            void Dump(string message) => Console.WriteLine(message);
        }
    }
}
