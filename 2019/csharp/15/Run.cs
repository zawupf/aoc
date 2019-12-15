using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;
using Aoc._2019._05;

namespace Aoc._2019._15
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var robot = Robot.Boot(ReadAllText("15/input1.txt"));
            var steps = robot.ScanGrid(true);
            return steps.ToString();
        }

        public string Job2()
        {
            var robot = Robot.Boot(ReadAllText("15/input1.txt"));
            robot.ScanGrid(true);
            robot.ClearGrid();
            robot.ScanGrid(false);
            return robot.MaxPathLength().ToString();
        }
    }

    public class Robot
    {
        private (int, int) position = (0, 0);
        private (int, int)? oxygenSystem = null;
        private Dictionary<(int, int), Info> grid =
            new Dictionary<(int, int), Info>() { [(0, 0)] = Info.Reachable };
        private Computer computer;
        private int steps = 0;
        private List<int> paths = new List<int>();

        public Robot Clone()
        {
            var robot = new Robot();
            robot.position = position;
            robot.oxygenSystem = oxygenSystem;
            robot.grid = new Dictionary<(int, int), Info>(grid);
            robot.computer = computer.Clone();
            robot.steps = steps;
            robot.paths = new List<int>();
            return robot;
        }

        public int MaxPathLength()
        {
            return paths.Max();
        }

        public void ClearGrid()
        {
            grid.Clear();
            grid[position] = Info.Reachable;
            steps = 0;
        }

        public static Robot Boot(string sourceCode)
        {
            var robot = new Robot();
            robot.computer = new Computer(Computer.Compile(sourceCode));
            return robot;
        }

        public int ScanGrid(bool findOxygen)
        {
            while (true)
            {
                foreach (var move in PeekMoves())
                {
                    var (_status, _pos) = Step(move);
                    if (_status == Status.WallHit)
                        grid[_pos] = Info.Blocked;
                    else
                        Step(Opposite(move));
                }

                var isDeadEnd = (
                    from move in Moves()
                    where InfoAt(Position(move)) == Info.Blocked
                    select move
                ).Count() == 3;

                if (findOxygen)
                {
                    if (isDeadEnd)
                        grid[position] = Info.Blocked;
                }
                else
                {
                    var keepGoing = (
                        from move in Moves()
                        where InfoAt(Position(move)) == Info.NotVisited
                        select move
                    ).Count() == 1;

                    if (isDeadEnd && !keepGoing)
                    {
                        paths.Add(steps);
                        break;
                    }
                }

                var nextMove = NextMove(findOxygen);
                if (InfoAt(Position(nextMove)) == Info.Reachable)
                    --steps;
                else
                    ++steps;
                var (status, pos) = Step(nextMove);
                HandleStatus(status, pos);
                // Render();

                if (findOxygen && oxygenSystem != null)
                    break;
            }

            return steps;

            Move Opposite(Move move)
            {
                return move switch
                {
                    Move.North => Move.South,
                    Move.South => Move.North,
                    Move.East => Move.West,
                    Move.West => Move.East,
                    _ => throw new Exception("Invalid move"),
                };
            }

            void Render()
            {
                var bbox = (
                    x: (min: int.MaxValue, max: int.MinValue),
                    y: (min: int.MaxValue, max: int.MinValue)
                );

                foreach (var pos in grid.Keys)
                {
                    var (x, y) = pos;
                    if (x < bbox.x.min) bbox.x.min = x;
                    if (x > bbox.x.max) bbox.x.max = x;
                    if (y < bbox.y.min) bbox.y.min = y;
                    if (y > bbox.y.max) bbox.y.max = y;
                }

                var result = new StringBuilder();
                for (int row = bbox.y.min; row <= bbox.y.max; ++row)
                {
                    result.Append('\n');
                    for (int col = bbox.x.min; col <= bbox.x.max; ++col)
                    {
                        var pos = (col, row);
                        var c = grid.GetValueOrDefault(pos, Info.NotVisited) switch
                        {
                            Info.Blocked => '#',
                            Info.Reachable => '.',
                            Info.NotVisited => ' ',
                            _ => throw new Exception("Invalid grid info"),
                        };
                        if (pos == position) c = 'D';
                        else if (pos == oxygenSystem) c = 'O';
                        result.Append(c);
                    }
                }
                result.Append($"\nSteps: {steps}");
                Console.WriteLine(result.ToString());
                Console.ReadKey();
            }
        }

        (Status, (int, int)) Step(Move move)
        {
            var outputs = new List<long>();
            var pos = Position(move);
            computer.Continue((long)move, outputs);
            var status = (Status)outputs.First();
            return (status, pos);
        }

        void HandleStatus(Status status, (int, int) pos)
        {
            switch (status)
            {
                case Status.WallHit:
                    grid[pos] = Info.Blocked;
                    break;
                case Status.Moved:
                    grid[pos] = Info.Reachable;
                    position = pos;
                    break;
                case Status.OxygenSystemReached:
                    grid[pos] = Info.Reachable;
                    position = pos;
                    oxygenSystem = position;
                    break;
                default:
                    throw new Exception("Invalid status");
            }
        }

        private IEnumerable<Move> Moves()
        {
            yield return Move.North;
            yield return Move.East;
            yield return Move.South;
            yield return Move.West;
        }

        private (int, int) Position(Move move)
        {
            var (x, y) = position;
            return move switch
            {
                Move.North => (x, y - 1),
                Move.South => (x, y + 1),
                Move.East => (x + 1, y),
                Move.West => (x - 1, y),
                _ => throw new Exception("Invalid move"),
            };
        }

        private Move NextMove(bool findOxygen)
        {
            if (findOxygen)
            {
                return (
                    from move in Moves()
                    let info = InfoAt(Position(move))
                    where info != Info.Blocked
                    orderby info
                    select move
                ).First();
            }
            else
            {
                var _paths = (
                    from move in Moves()
                    let info = InfoAt(Position(move))
                    where info == Info.NotVisited
                    select move
                ).ToArray();

                var robots = (
                    from move in _paths[1..]
                    select CloneRobot(move)
                );

                foreach (var r in robots)
                {
                    r.ScanGrid(false);
                    foreach (var p in r.paths)
                    {
                        paths.Add(p);
                    }
                }

                return _paths[0];
            }

            Robot CloneRobot(Move move)
            {
                var r = Clone();
                var (stat, pos) = r.Step(move);
                r.HandleStatus(stat, pos);
                ++r.steps;
                return r;
            }
        }

        private IEnumerable<Move> PeekMoves()
        {
            return (
                from move in Moves()
                let info = InfoAt(Position(move))
                where info == Info.NotVisited
                select move
            );
        }

        private Info InfoAt((int, int) pos)
        {
            return grid.GetValueOrDefault(pos, Info.NotVisited);
        }

        public enum Move
        {
            North = 1,
            South,
            West,
            East,
        }

        public enum Status
        {
            WallHit = 0,
            Moved,
            OxygenSystemReached,
        }

        public enum Info
        {
            NotVisited = 0,
            Reachable,
            Blocked
        }
    }
}
