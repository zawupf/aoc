using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020
{
    using static Forest;

    public class Day03 : IDay
    {
        public override string Day { get; } = nameof(Day03)[3..];

        public override string Result1()
        {
            Forest forest = Parse(InputLines);
            Dictionary<Creature, int> creatureCount = Hike(forest, new(3, 1));
            return creatureCount[Creature.Tree].ToString();
        }

        public override string Result2()
        {
            Slope[] slopes =
            {
                new(1,1),
                new(3,1),
                new(5,1),
                new(7,1),
                new(1,2),
            };
            Forest forest = Parse(InputLines);

            return slopes
                .Select(slope => Hike(forest, slope)[Creature.Tree])
                .Aggregate((result, count) => result * count)
                .ToString();
        }

        public static Dictionary<Creature, int> Hike(Forest forest, Slope slope)
        {
            Position position = new(0, 0);
            Dictionary<Creature, int> creatureCount = new()
            {
                { Creature.EmptySpace, 0 },
                { Creature.Tree, 0 },
            };

            while (forest.Contains(position = position.Move(slope)))
            {
                creatureCount[forest.CreatureAt(position)] += 1;
            }

            return creatureCount;
        }
    }

    public record Forest(Creature[][] Map)
    {
        public enum Creature
        {
            EmptySpace,
            Tree,
        }

        public bool Contains(Position p) => p.Y >= 0 && p.Y < Map.Length;

        public Creature CreatureAt(Position p) => Map[p.Y][NormalizedX(p.X)];

        private int NormalizedX(int x) => x % Map[0].Length;

        public static Forest Parse(IEnumerable<string> lines) =>
            new(lines.Select(line => ParseRow(line)).ToArray());

        private static Creature[] ParseRow(string line) =>
            line.Select(ToCreature).ToArray();

        private static Creature ToCreature(char c) => c switch
        {
            '#' => Creature.Tree,
            '.' => Creature.EmptySpace,
            _ => Creature.EmptySpace,
        };
    }

    public record Position(int X, int Y)
    {
        public Position Move(Slope slope) =>
            new Position(X + slope.DX, Y + slope.DY);
    }

    public record Slope(int DX, int DY);
}
