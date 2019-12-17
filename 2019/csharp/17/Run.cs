using System;
using System.Linq;
using System.Collections.Generic;
using Aoc._2019._05;

namespace Aoc._2019._17
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var robot = new Robot(ReadAllText("17/input1.txt"));
            var result = robot.SumOfAlignmentParameters();
            return result.ToString();
        }

        public string Job2()
        {
            throw new NotImplementedException();
        }
    }

    public class Robot
    {
        private Computer computer;
        private string[] rows;
        private int Height => rows.Length;
        private int Width => Height == 0 ? 0 : rows[0].Length;
        private char Ascii(int x, int y) => rows[y][x];

        public Robot(string source)
        {
            computer = new Computer(Computer.Compile(source));
            rows = Render().Trim().Split('\n');
        }

        public int SumOfAlignmentParameters()
        {
            var sum = 0;
            for (int y = 1; y < Height - 1; ++y)
            {
                for (int x = 1; x < Width - 1; ++x)
                {
                    if (IsIntersection(x, y))
                        sum += x * y;
                }
            }
            return sum;

            bool IsIntersection(int x, int y)
            {
                var scaffold = '#';
                return (
                    IsScaffold(Ascii(x, y)) &&
                    IsScaffold(Ascii(x + 1, y)) &&
                    IsScaffold(Ascii(x - 1, y)) &&
                    IsScaffold(Ascii(x, y + 1)) &&
                    IsScaffold(Ascii(x, y - 1))
                );
            }

            bool IsScaffold(char c)
            {
                return c == '#' || IsRobot(c);
            }

            bool IsRobot(char c)
            {
                return c == '^' || c == 'v' || c == '<' || c == '>';
            }
        }

        public string Render()
        {
            var c = computer.Clone();
            List<long> outputs;
            c.Exec(new List<long>(), out outputs);

            return new String((from o in outputs select (char)o).ToArray());
        }
    }
}
