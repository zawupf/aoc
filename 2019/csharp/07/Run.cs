using System;
using System.Linq;
using System.Collections.Generic;
using Aoc._2019._05;

namespace Aoc._2019._07
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var result = MaxThrusterSignal(ReadCode());
            return result.ToString();
        }

        public string Job2()
        {
            var result = MaxThrusterSignalFeedback(ReadCode());
            return result.ToString();
        }

        public static int MaxThrusterSignal(int[] code)
        {
            var phaseset = new int[] { 0, 1, 2, 3, 4 };
            var results =
                from phases in GetPermutations(phaseset, phaseset.Length)
                select Analyzer.Exec(phases.ToArray(), code);
            return results.Max();
        }

        public static int MaxThrusterSignalFeedback(int[] code)
        {
            var phaseset = new int[] { 5, 6, 7, 8, 9 };
            var results =
                from phases in GetPermutations(phaseset, phaseset.Length)
                select Analyzer.ExecFeedback(phases.ToArray(), code);
            return results.Max();
        }

        static IEnumerable<IEnumerable<T>>
            GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private int[] ReadCode()
        {
            return Computer.Compile(ReadAllText("07/input1.txt"));
        }
    }

    public class Analyzer
    {
        private int phase;
        private Computer computer;
        private List<int> output;

        public bool IsHalted => computer?.IsHalted ?? false;
        public bool IsPaused => computer?.IsPaused ?? false;

        private int PopOutput()
        {
            var result = output.First();
            output.RemoveAt(0);
            return result;
        }

        public Analyzer(int phase)
        {
            this.phase = phase;
        }

        public int Run(int input, int[] code)
        {
            computer = new Computer(code);
            computer.Exec(new List<int> { this.phase, input }, out output);
            return PopOutput();
        }

        public int Continue(int input)
        {
            computer.Continue(input, output);
            return PopOutput();
        }

        public static int Exec(int[] phases, int[] code)
        {
            var analyzers =
                from phase in phases
                select new Analyzer(phase);

            int output = 0;
            foreach (var analyzer in analyzers)
            {
                output = analyzer.Run(output, code);
            }

            return output;
        }

        public static int ExecFeedback(int[] phases, int[] code)
        {
            var analyzers = (
                from phase in phases
                select new Analyzer(phase)
            ).ToArray();

            int output = 0;
            while (!analyzers.Last().IsHalted)
            {
                foreach (var analyzer in analyzers)
                {
                    if (analyzer.IsPaused)
                        output = analyzer.Continue(output);
                    else
                        output = analyzer.Run(output, code);
                }
            }

            return output;
        }
    }
}
