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
            var result = fft.Phases(ReadAllText("16/input1.txt"), 1).ElementAt(99);
            Console.WriteLine($"length: {result.Count()}");
            return fft.Current(0, 8);
        }

        public string Job2()
        {
            // return "in work";
            var fft = new FFT();
            var result = fft.Phases(ReadAllText("16/input1.txt"), 100).ElementAt(99);
            Console.WriteLine($"length: {result.Count()}");
            return fft.Current(0, 8);
        }
    }

    public class FFT // Flawed Frequency Transmission
    {
        private int[] current;

        public IEnumerable<int[]> Phases(string input, int count = 1)
        {
            return Phases(ParseInput(input, count));
        }

        public IEnumerable<int[]> Phases(IEnumerable<int> input)
        {
            current = input.ToArray();

            while (NextPhase())
            {
                yield return current;
            }
        }

        public string Current(int offset, int count)
        {
            return string.Join(
                "",
                current[offset..(offset + count)].Select(value => value.ToString())
            );
        }

        private bool NextPhase()
        {
            var partialSums = new List<int>();
            var result = new int[current.Length];
            for (int i = 0; i < current.Length; ++i)
                result[i] = Calculate(i);
            current = result;
            return true;

            int Calculate(int shift)
            {
                InitPartialSums(shift);
                var sum = 0;
                bool add = true;
                foreach (var s in partialSums)
                {
                    if (add) sum += s;
                    else sum -= s;
                    add = !add;
                }

                return Math.Abs(sum) % 10;
            }

            void InitPartialSums(int shift)
            {
                partialSums.Clear();
                var offset = shift;
                var length = shift + 1;
                var step = 2 * length;
                for (int i = offset; i < current.Length; i += step)
                {
                    var end = i + length;
                    if (end > current.Length) end = current.Length;
                    partialSums.Add(current[i..end].Sum());
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
