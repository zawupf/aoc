using System;
using System.Linq;
using System.Collections.Generic;

namespace Aoc._2019._10
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var asteroids = Asteroids.Parse(ReadAllText("10/input1.txt"));
            return asteroids.BestLocationCount().ToString();
        }

        public string Job2()
        {
            var asteroids = Asteroids.Parse(ReadAllText("10/input1.txt"));
            var (_, location) = asteroids.BestLocation();
            var shots = asteroids.Vaporize(location);
            var (x, y) = shots.ElementAt(199);
            return (x * 100 + y).ToString();
        }
    }

    public class Asteroids
    {
        private HashSet<(int, int)> asteroids = new HashSet<(int, int)>();
        private int width = 0;
        private int height = 0;

        public static Asteroids Parse(string data)
        {
            return Parse(data.Trim().Split('\n'));
        }

        public static Asteroids Parse(string[] lines)
        {
            var result = new Asteroids();
            result.height = lines.Length;
            result.width = lines[0].Length;

            int y = 0;
            foreach (var line in lines)
            {
                int x = 0;
                foreach (var c in line.Trim().ToCharArray())
                {
                    if (c != '.')
                        result.asteroids.Add((x, y));
                    ++x;
                }
                ++y;
            }

            return result;
        }

        public int BestLocationCount()
        {
            return BestLocation().Item1;
        }

        public (int, (int, int)) BestLocation()
        {
            var visibleCountQuery =
                from asteroid in asteroids
                select (VisibleCount(asteroid), asteroid);

            var sortedQuery =
                from count in visibleCountQuery
                orderby count.Item1 descending
                select count;

            return sortedQuery.First();
        }

        public int VisibleCount((int, int) location)
        {
            var query =
                from asteroid in asteroids
                where asteroid != location
                select Direction(location, asteroid);
            return query.Distinct().Count();
        }

        public static (int, int) Direction((int, int) from, (int, int) to)
        {
            var x = to.Item1 - from.Item1;
            var y = to.Item2 - from.Item2;
            var ggt = GGT(Math.Abs(x), Math.Abs(y));
            return (x / ggt, y / ggt);

            int GGT(int a, int b)
            {
                if (a < b) return GGT(b, a);
                if (a == 0 && b == 0) return 1;
                if (b == 0) return a;

                var mod = a % b;
                if (mod == 0) return b;

                return GGT(b, mod);
            }
        }

        public IEnumerable<(int, int)> Vaporize((int, int) location)
        {
            var sameDirection = (
                from asteroid in asteroids
                where location != asteroid
                select (
                    asteroid,
                    direction: Direction(location, asteroid),
                    distance: Distance(location, asteroid)
                )
            ).GroupBy(data => data.direction);

            var data = new List<(float angle, List<(int, int)>)>();
            foreach (var item in sameDirection)
            {
                var direction = item.Key;
                var (dx, dy) = direction;
                var angle = MathF.Atan2(dx, -dy);
                if (angle < 0.0f)
                    angle += 2.0f * MathF.PI;
                var asteroids = (
                    from i in item
                    orderby i.distance
                    select i.asteroid
                ).ToList();
                data.Add((angle, asteroids));
            }
            data.Sort();

            while (data.Count != 0)
            {
                foreach (var item in data)
                {
                    var target = item.Item2.First();
                    item.Item2.RemoveAt(0);
                    yield return target;
                }

                data.RemoveAll(item => item.Item2.Count == 0);
            }
        }

        int Distance((int, int) a, (int, int) b)
        {
            var (ax, ay) = a;
            var (bx, by) = b;
            var (dx, dy) = (bx - ax, by - ay);
            return dx * dx + dy * dy;
        }
    }
}
