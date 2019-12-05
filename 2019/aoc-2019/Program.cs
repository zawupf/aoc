﻿using System;

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
            Run(5, new _05.Run());

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
