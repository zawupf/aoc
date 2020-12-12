using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day12 : IDay
    {
        public override string Day { get; } = nameof(Day12)[3..];

        public override string Result1() =>
            ManhattanDistance(
                InputLines
                .Select(Movement.Parse)
                .Aggregate((0, 0, 0), Move))
            .ToString();

        public override string Result2() =>
            ManhattanDistance(
                InputLines
                .Select(Movement.Parse)
                .Aggregate(((0, 0), (10, 1)), MoveWaypoint))
            .ToString();

        public static (int x, int y, int a) Move((int x, int y, int a) p, Movement m) =>
            m.Action switch
            {
                'E' => (p.x + m.Value, p.y, p.a),
                'W' => (p.x - m.Value, p.y, p.a),
                'N' => (p.x, p.y + m.Value, p.a),
                'S' => (p.x, p.y - m.Value, p.a),
                'L' => (p.x, p.y, p.a + m.Value),
                'R' => (p.x, p.y, p.a - m.Value),
                'F' => NormalizedAngle(p.a) switch
                {
                    0 => (p.x + m.Value, p.y, p.a),
                    180 => (p.x - m.Value, p.y, p.a),
                    90 => (p.x, p.y + m.Value, p.a),
                    270 => (p.x, p.y - m.Value, p.a),
                    _ => throw new ArgumentException($"Invalid angle: {p.a}"),
                },
                _ => throw new ArgumentException($"Invalid action: {m.Action}"),
            };

        public static ((int x, int y) s, (int x, int y) w) MoveWaypoint(
            ((int x, int y) s, (int x, int y) w) p, Movement m) =>
            m.Action switch
            {
                'E' => (p.s, (p.w.x + m.Value, p.w.y)),
                'W' => (p.s, (p.w.x - m.Value, p.w.y)),
                'N' => (p.s, (p.w.x, p.w.y + m.Value)),
                'S' => (p.s, (p.w.x, p.w.y - m.Value)),
                'L' => m.Value switch
                {
                    0 => (p.s, p.w),
                    90 => (p.s, (-p.w.y, p.w.x)),
                    180 => (p.s, (-p.w.x, -p.w.y)),
                    270 => (p.s, (p.w.y, -p.w.x)),
                    _ => throw new ArgumentException($"Invalid angle: {m.Value}"),
                },
                'R' => m.Value switch
                {
                    0 => (p.s, p.w),
                    90 => (p.s, (p.w.y, -p.w.x)),
                    180 => (p.s, (-p.w.x, -p.w.y)),
                    270 => (p.s, (-p.w.y, p.w.x)),
                    _ => throw new ArgumentException($"Invalid angle: {m.Value}"),
                },
                'F' => ((p.s.x + p.w.x * m.Value, p.s.y + p.w.y * m.Value), p.w),
                _ => throw new ArgumentException($"Invalid action: {m.Action}"),
            };

        public static int ManhattanDistance((int x, int y, int a) p) =>
            Math.Abs(p.x) + Math.Abs(p.y);

        public static int ManhattanDistance(((int x, int y) s, (int x, int y) w) p) =>
            Math.Abs(p.s.x) + Math.Abs(p.s.y);

        public static int NormalizedAngle(int a) => (a + 3600) % 360;
    }

    public record Movement(char Action, int Value)
    {
        public static Movement Parse(string line) =>
            new Movement(line[0], int.Parse(line[1..]));
    }
}
