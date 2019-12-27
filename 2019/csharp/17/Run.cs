using System;
using System.Linq;
using System.Collections.Generic;
using Aoc._2019._05;
using static Aoc._2019.Utils;

namespace Aoc._2019._17
{
    public class Run : IRun
    {
        public string Job1()
        {
            var robot = new Robot(ReadInputText("17"));
            var result = robot.SumOfAlignmentParameters();
            return result.ToString();
        }

        public string Job2()
        {
            var robot = new Robot(ReadInputText("17"));
            var steps = robot.Steps();
            return String.Join(' ',
                (from step in steps select ToString(step)));

            string ToString((char, int) val)
            {
                var (turn, count) = val;
                return $"{turn},{count}";
            }
        }
    }

    public class Robot
    {
        private Computer computer;
        private string[] rows;
        private int Height => rows.Length;
        private int Width => Height == 0 ? 0 : rows[0].Length;
        private char Ascii(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height
                ? rows[y][x]
                : '.';
        }

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
        }

        private bool IsIntersection(int x, int y)
        {
            return (
                IsScaffold(Ascii(x, y)) &&
                IsScaffold(Ascii(x + 1, y)) &&
                IsScaffold(Ascii(x - 1, y)) &&
                IsScaffold(Ascii(x, y + 1)) &&
                IsScaffold(Ascii(x, y - 1))
            );
        }

        private bool IsScaffold(char c)
        {
            return c == '#' || IsRobot(c);
        }

        private bool IsRobot(char c)
        {
            return c == '^' || c == 'v' || c == '<' || c == '>';
        }

        public string Render()
        {
            var c = computer.Clone();
            List<long> outputs;
            c.Exec(new List<long>(), out outputs);

            return new String((from o in outputs select (char)o).ToArray());
        }

        public IEnumerable<(char turn, int count)> Steps()
        {
            var (r, (x, y)) = FindRobot();
            char turn;

            while (true)
            {
                var (left, right, front) = HasScaffold();
                var done = !left && !right && !front;
                if (done)
                    yield break;

                Turn(left, right);
                var count = CountSteps();
                yield return (turn, count);
            }

            void Turn(bool left, bool right)
            {
                (turn, r) = (left, right) switch
                {
                    (true, false) => ('L', r switch
                    {
                        '^' => '<',
                        '<' => 'v',
                        'v' => '>',
                        '>' => '^',
                        _ => throw new Exception()
                    }),
                    (false, true) => ('R', r switch
                    {
                        '^' => '>',
                        '>' => 'v',
                        'v' => '<',
                        '<' => '^',
                        _ => throw new Exception()
                    }),
                    _ => throw new Exception()
                };
            }

            int CountSteps()
            {
                var count = 0;
                while (HasScaffoldFront())
                    ++count;
                return count;
            }

            bool HasScaffoldFront()
            {
                var (_, _, front) = HasScaffold();
                return front;
            }

            (bool left, bool right, bool front) HasScaffold()
            {
                return r switch
                {
                    '^' => (
                        IsScaffold(Ascii(x - 1, y)),
                        IsScaffold(Ascii(x + 1, y)),
                        IsScaffold(Ascii(x, y - 1))
                        ),
                    'v' => (
                        IsScaffold(Ascii(x + 1, y)),
                        IsScaffold(Ascii(x - 1, y)),
                        IsScaffold(Ascii(x + 1, y))
                        ),
                    '<' => (
                        IsScaffold(Ascii(x, y + 1)),
                        IsScaffold(Ascii(x, y - 1)),
                        IsScaffold(Ascii(x - 1, y))
                        ),
                    '>' => (
                        IsScaffold(Ascii(x, y - 1)),
                        IsScaffold(Ascii(x, y + 1)),
                        IsScaffold(Ascii(x + 1, y))
                        ),
                    _ => throw new Exception("Invalid robot direction"),
                };
            }

            (char, (int, int)) FindRobot()
            {
                for (int y = 1; y < Height - 1; ++y)
                {
                    for (int x = 1; x < Width - 1; ++x)
                    {
                        var c = Ascii(x, y);
                        if (IsRobot(c))
                            return (c, (x, y));
                    }
                }
                throw new Exception("Robot not found");
            }
        }
    }
}
