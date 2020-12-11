using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day11 : IDay
    {
        public override string Day { get; } = nameof(Day11)[3..];

        public override string Result1() =>
            StableState(Seats.Parse(InputLines), 4, false)
            .Count(Seats.Type.OccupiedSeat)
            .ToString();

        public override string Result2() =>
            StableState(Seats.Parse(InputLines), 5, true)
            .Count(Seats.Type.OccupiedSeat)
            .ToString();

        public static Seats StableState(Seats currentSeats, int limit, bool look)
        {
            while (true)
            {
                Seats nextSeats = currentSeats.Next(limit, look);
                if (nextSeats.Rows.SequenceEqual(currentSeats.Rows))
                    return currentSeats;
                currentSeats = nextSeats;
            }
        }
    }

    public record Seats(string[] Rows)
    {
        public int Width => Rows[0].Length;

        public int Height => Rows.Length;

        public int Count(Type type) =>
            Rows
            .Select(row => row.Sum(x => x == (char)type ? 1 : 0))
            .Sum();

        public enum Type
        {
            Floor = '.',
            EmptySeat = 'L',
            OccupiedSeat = '#',
        }

        public static Seats Parse(IEnumerable<string> lines) =>
            new(lines.ToArray());

        public Seats Next(int limit, bool look)
        {
            List<string> nextRows = new();
            for (int y = 0; y < Height; y++)
            {
                StringBuilder nextRow = new();
                for (int x = 0; x < Width; x++)
                {
                    nextRow.Append((char)(TypeAt(x, y) switch
                    {
                        Type.Floor => Type.Floor,
                        Type.EmptySeat =>
                            AdjacentOccupiedSeats(x, y, look).Any()
                            ? Type.EmptySeat
                            : Type.OccupiedSeat,
                        Type.OccupiedSeat =>
                            AdjacentOccupiedSeats(x, y, look).Take(limit).Count() == limit
                            ? Type.EmptySeat
                            : Type.OccupiedSeat,
                        _ => throw new ApplicationException("Panic!"),
                    }));
                }
                nextRows.Add(nextRow.ToString());
            }
            return new(nextRows.ToArray());
        }

        public IEnumerable<(int x, int y)> AdjacentOccupiedSeats(int x, int y, bool look) =>
            AdjacentSeatsAt(x, y, look)
            .Where(p => TypeAt(p.x, p.y) == Type.OccupiedSeat);

        public Type TypeAt(int x, int y) => (Type)Rows[y][x];

        public (int x, int y)[] AdjacentSeatsAt(int x, int y, bool look)
        {
            Func<int, int, int, int, (int, int)> f = look ? Sight : Near;
            return
                new (int x, int y)[] {
                    f(x, y, -1, -1), f(x, y, 0, -1), f(x, y, 1, -1),
                    f(x, y, -1,  0),                 f(x, y, 1,  0),
                    f(x, y, -1,  1), f(x, y, 0,  1), f(x, y, 1,  1),
                }
                .Where(p => IsValid(p.x, p.y))
                .ToArray();

            bool IsValid(int x, int y) =>
                x >= 0 && x < Width && y >= 0 && y < Height;

            (int, int) Near(int x, int y, int dx, int dy) => (x + dx, y + dy);

            (int, int) Sight(int x, int y, int dx, int dy)
            {
                for (
                    (x, y) = (x + dx, y + dy);
                    IsValid(x, y) && TypeAt(x, y) == Type.Floor;
                    (x, y) = (x + dx, y + dy)) ;
                return (x, y);
            }
        }
    }
}
