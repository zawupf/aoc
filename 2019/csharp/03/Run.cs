using System.Collections.Generic;
using System;
using System.Linq;

namespace Aoc._2019._03
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var lines = ReadLines("03/input1.txt").ToArray();
            var distance = Distance(lines);
            return distance.ToString();
        }

        public string Job2()
        {
            var lines = ReadLines("03/input1.txt").ToArray();
            var steps = Steps(lines);
            return steps.ToString();
        }

        public static int Distance(string[] lines)
        {
            var wires = Move.ParseWires(lines);
            var pointsets = from wire in wires select WirePoints(wire);
            var wire1 = pointsets.First();
            var wire2 = pointsets.Skip(1).First();
            var points = wire1.Intersect(wire2);
            var query = from point in points
                        select distance(point);
            var result = query.Min();
            return result;

            int distance((int, int) point)
            {
                return Math.Abs(point.Item1) + Math.Abs(point.Item2);
            }
        }

        public static int Steps(string[] lines)
        {
            var wires = Move.ParseWires(lines);
            var pointsets = from wire in wires select WirePoints(wire);
            var wire1 = pointsets.First();
            var wire2 = pointsets.Skip(1).First();
            var points = wire1.Intersect(wire2);
            var query = from point in points
                        select steps(point);
            var result = query.Min();
            return result;

            int steps((int, int) point)
            {
                return (from wire in wires select CountStepsTo(point, wire)).Sum();
            }
        }

        public static HashSet<(int, int)> WirePoints(Move[] moves)
        {
            var points = new HashSet<(int, int)>();
            var cursor = (0, 0);

            foreach (var move in moves)
            {
                cursor = InsertPoints(points, move, cursor);
            }

            return points;

            (int, int) InsertPoints(
                HashSet<(int, int)> points, Move move, (int, int) cursor)
            {
                var delta =
                    move.Direction switch
                    {
                        Direction.Up => (0, 1),
                        Direction.Down => (0, -1),
                        Direction.Left => (-1, 0),
                        Direction.Right => (1, 0),
                        _ => throw new InvalidDirectionException(),
                    };

                for (int i = 0; i < move.Length; ++i)
                {
                    cursor.Item1 = cursor.Item1 + delta.Item1;
                    cursor.Item2 = cursor.Item2 + delta.Item2;
                    points.Add((cursor.Item1, cursor.Item2));
                }

                return cursor;
            }
        }

        public static int CountStepsTo((int, int) point, Move[] moves)
        {
            var steps = 0;
            var cursor = (0, 0);

            foreach (var move in moves)
            {
                (cursor, steps) = InsertPoints(move, cursor, steps);
                if (cursor == point)
                {
                    break;
                }
            }

            return steps;

            ((int, int), int) InsertPoints(
                Move move, (int, int) cursor, int steps)
            {
                var delta =
                    move.Direction switch
                    {
                        Direction.Up => (0, 1),
                        Direction.Down => (0, -1),
                        Direction.Left => (-1, 0),
                        Direction.Right => (1, 0),
                        _ => throw new InvalidDirectionException(),
                    };

                for (int i = 0; i < move.Length; ++i)
                {
                    cursor.Item1 = cursor.Item1 + delta.Item1;
                    cursor.Item2 = cursor.Item2 + delta.Item2;
                    ++steps;
                    if (cursor == point)
                    {
                        break;
                    }
                }

                return (cursor, steps);
            }
        }
    }

    public enum Direction
    {
        Up = 0,
        Down,
        Left,
        Right,
    }

    public class InvalidDirectionException : Exception { }

    public readonly struct Move
    {
        public Direction Direction { get; }
        public int Length { get; }

        public Move(Direction direction, int length)
        {
            Direction = direction;
            Length = length;
        }

        public override string ToString()
        {
            var direction = Direction switch
            {
                Direction.Up => 'U',
                Direction.Down => 'D',
                Direction.Left => 'L',
                Direction.Right => 'R',
                _ => throw new InvalidDirectionException(),
            };

            return $"{direction}{Length}";
        }

        public static Move Parse(string text)
        {
            var direction = text[0] switch
            {
                'U' => Direction.Up,
                'D' => Direction.Down,
                'L' => Direction.Left,
                'R' => Direction.Right,
                _ => throw new InvalidDirectionException(),
            };

            var length = int.Parse(text[1..]);

            return new Move(direction, length);
        }

        public static Move[] ParseMoves(string line)
        {
            var moves =
                from move in line.Split(',')
                select Parse(move);
            return moves.ToArray();
        }

        public static Move[][] ParseWires(string[] lines)
        {
            var wires =
                from line in lines
                select ParseMoves(line);
            return wires.ToArray();
        }
    }
}
