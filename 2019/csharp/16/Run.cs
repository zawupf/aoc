using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Aoc._2019._16
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var fft = new FFT();
            var result = fft.Phases(ReadAllText("16/input1.txt"), 4).ElementAt(99);
            Console.WriteLine($"length: {result.Count()}");
            return fft.Current(0, 8);
        }

        public string Job2()
        {
            return "in work";
        }
    }

    public class FFT // Flawed Frequency Transmission
    {
        private int[] basePattern;
        private IEnumerable<int> current;

        public FFT()
        {
            this.basePattern = new[] { 0, 1, 0, -1 };
        }

        public FFT(int[] basePattern)
        {
            this.basePattern = basePattern;
        }

        public IEnumerable<IEnumerable<int>> Phases(string input, int count = 1)
        {
            return Phases(ParseInput(input, count));
        }

        public IEnumerable<IEnumerable<int>> Phases(IEnumerable<int> input)
        {
            current = input;

            while (NextPhase())
            {
                yield return current;
            }
        }

        public string Current(int offset, int count)
        {
            return string.Join(
                "",
                current.Skip(offset).Take(count).Select(value => value.ToString())
            );
        }

        private bool NextPhase()
        {
            current = (
                from position in Enumerable.Range(1, current.Count())
                select Calculate(position)
            ).ToArray();
            return true;
        }

        private int Calculate(int position)
        {
            var sum = (
                from value in current.Zip(Pattern(position))
                    // where value.Second != 0
                select value.First * value.Second
            ).Sum();
            return Math.Abs(sum) % 10;
        }

        private IEnumerable<int> Pattern(int position)
        {
            return RegularPattern(position).Skip(1);

            IEnumerable<int> RegularPattern(int position)
            {
                while (true)
                {
                    foreach (var value in basePattern)
                    {
                        for (int i = 0; i < position; i++)
                        {
                            yield return value;
                        }
                    }
                }
            }
        }

        private IEnumerable<int> ParseInput(string input, int count)
        {
            var ints = (
                from c in input
                select int.Parse(c.ToString())
            );

            if (count == 1)
                return ints;

            var manyInts = ints;
            for (int i = 1; i < count; ++i)
            {
                manyInts = manyInts.Concat(ints);
            }
            return manyInts;
        }
    }
}
