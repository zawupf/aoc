using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Aoc._2019._11
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var run = new Run(InputPrefix);
            var robot = new Robot(run.ReadAllText("11/input1.txt"));
            var steps = robot.Walk(Robot.Field.Color.Black).Last();
            return robot.PaintedPanelsCount().ToString();
        }

        public string Job2()
        {
            var run = new Run(InputPrefix);
            var robot = new Robot(run.ReadAllText("11/input1.txt"));
            var steps = robot.Walk(Robot.Field.Color.White).Last();
            return robot.Image();
        }
    }

    public class Robot
    {
        private _05.Computer brain;
        private Dictionary<(int, int), Field> fields;
        private State state;

        public bool IsHalted { get => brain.IsHalted; }

        public Robot(string sourceCode)
        {
            brain = new _05.Computer(_05.Computer.Compile(sourceCode));
            fields = new Dictionary<(int, int), Field>();
            state = new State { viewDirection = State.ViewDirection.Up, position = (0, 0) };
        }

        public int PaintedPanelsCount()
        {
            return fields.Count();
        }

        public IEnumerable<(State state, Field field)> Walk(Robot.Field.Color firstFieldColor)
        {
            bool firstStep = true;
            var output = new List<long>();
            do
            {
                foreach (var field in Work(firstFieldColor))
                {
                    yield return (state, field);
                    firstStep = false;
                }
            } while (!IsHalted);


            IEnumerable<Field> Work(Robot.Field.Color firstFieldColor)
            {
                while (true)
                {
                    var field = fields.GetValueOrDefault(
                        state.position,
                        new Field
                        {
                            position = state.position,
                            color = firstStep ? firstFieldColor : Field.Color.Black,
                        }
                    );

                    if (!IsHalted)
                        brain.Continue((long)field.color, output);

                    if (output.Count == 0)
                        break;
                    var color = output[0];
                    var turn = output[1];
                    output.RemoveRange(0, 2);

                    Paint(field, color);
                    Turn(turn);
                    Move();
                    yield return field;
                }
            }

            void Paint(Field field, long color)
            {
                field.color = color switch
                {
                    0 => Field.Color.Black,
                    1 => Field.Color.White,
                    _ => throw new Exception("Invalid color"),
                };
                fields[state.position] = field;
            }

            void Turn(long turn)
            {
                state.viewDirection = turn switch
                {
                    0 => state.viewDirection switch // turn left
                    {
                        State.ViewDirection.Up => State.ViewDirection.Left,
                        State.ViewDirection.Left => State.ViewDirection.Down,
                        State.ViewDirection.Down => State.ViewDirection.Right,
                        State.ViewDirection.Right => State.ViewDirection.Up,
                        _ => throw new Exception("Invalid view direction"),
                    },
                    1 => state.viewDirection switch // turn right
                    {
                        State.ViewDirection.Up => State.ViewDirection.Right,
                        State.ViewDirection.Right => State.ViewDirection.Down,
                        State.ViewDirection.Down => State.ViewDirection.Left,
                        State.ViewDirection.Left => State.ViewDirection.Up,
                        _ => throw new Exception("Invalid view direction"),
                    },
                    _ => throw new Exception("Invalid turn"),
                };
            }

            void Move()
            {
                var (x, y) = state.position;
                state.position = state.viewDirection switch
                {
                    State.ViewDirection.Up => (x, y + 1),
                    State.ViewDirection.Down => (x, y - 1),
                    State.ViewDirection.Right => (x + 1, y),
                    State.ViewDirection.Left => (x - 1, y),
                    _ => throw new Exception("Invalid view direction"),
                };
            }
        }

        public string Image()
        {
            var bbox = (
                minX: Int32.MaxValue, maxX: Int32.MinValue,
                minY: Int32.MaxValue, maxY: Int32.MinValue
            );

            foreach (var field in fields)
            {
                var (x, y) = field.Key;
                if (x < bbox.minX) bbox.minX = x;
                if (x > bbox.maxX) bbox.maxX = x;
                if (y < bbox.minY) bbox.minY = y;
                if (y > bbox.maxY) bbox.maxY = y;
            }

            var renderer = new StringBuilder();
            for (int y = bbox.maxY; y >= bbox.minY; --y)
            {
                renderer.Append('\n');
                for (int x = bbox.minX; x <= bbox.maxX; ++x)
                {
                    Field.Color color = Field.Color.Black;
                    if (fields.ContainsKey((x, y)))
                        color = fields[(x, y)].color;
                    renderer.Append(color == Field.Color.White ? '#' : ' ');
                }
            }

            return renderer.ToString();
        }

        public struct State
        {
            public ViewDirection viewDirection;
            public (int, int) position;

            public override string ToString()
            {
                return $"State(({position.Item1}, {position.Item2}), {viewDirection})";
            }

            public enum ViewDirection
            {
                Up = 0,
                Down,
                Left,
                Right,
            }
        }

        public struct Field
        {
            public (int, int) position;
            public Color color;

            public override string ToString()
            {
                return $"Field(({position.Item1}, {position.Item2}), {color})";
            }

            public enum Color
            {
                Black = 0,
                White = 1
            }
        }
    }
}
