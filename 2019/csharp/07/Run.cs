using System;
using System.Linq;
using System.Collections.Generic;
using Aoc._2019._05;
using static Aoc._2019.Utils;

namespace Aoc._2019._07
{
    public class Run : IRun
    {
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

        public static long MaxThrusterSignal(long[] code)
        {
            var phaseset = new long[] { 0, 1, 2, 3, 4 };
            var results =
                from phases in GetPermutations(phaseset, phaseset.Length)
                select Analyzer.Exec(phases.ToArray(), code);
            return results.Max();
        }

        public static long MaxThrusterSignalFeedback(long[] code)
        {
            var phaseset = new long[] { 5, 6, 7, 8, 9 };
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

        private long[] ReadCode()
        {
            return Computer.Compile(ReadInputText("07"));
        }
    }

    public class Analyzer
    {
        private long phase;
        private Computer computer;
        private List<long> output;

        public bool IsHalted => computer?.IsHalted ?? false;
        public bool IsPaused => computer?.IsPaused ?? false;

        private long PopOutput()
        {
            var result = output.First();
            output.RemoveAt(0);
            return result;
        }

        public Analyzer(long phase)
        {
            this.phase = phase;
        }

        public long Run(long input, long[] code)
        {
            computer = new Computer(code);
            computer.Exec(new List<long> { this.phase, input }, out output);
            return PopOutput();
        }

        public long Continue(long input)
        {
            computer.Continue(input, output);
            return PopOutput();
        }

        public static long Exec(long[] phases, long[] code)
        {
            var analyzers =
                from phase in phases
                select new Analyzer(phase);

            long output = 0;
            foreach (var analyzer in analyzers)
            {
                output = analyzer.Run(output, code);
            }

            return output;
        }

        public static long ExecFeedback(long[] phases, long[] code)
        {
            var analyzers = (
                from phase in phases
                select new Analyzer(phase)
            ).ToArray();

            long output = 0;
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
