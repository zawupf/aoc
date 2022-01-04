namespace Aoc2021;

public class Day22 : IDay
{
    public override string Day { get; } = nameof(Day22)[3..];

    public override string Result1()
    {
        return Reactor
            .Init(InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return "xxx";
    }

    public class Reactor
    {
        private readonly HashSet<Cube> EnabledCubes = new();

        public static long Init(IEnumerable<string> lines)
        {
            Reactor reactor = new();

            foreach (string line in lines)
            {
                bool on = line.StartsWith("on", false, CultureInfo.InvariantCulture);
                Cuboid cuboid = Cuboid.Parse(on ? line[3..] : line[4..]);
                foreach (Cube cube in cuboid.Cubes())
                {
                    _ = on
                        ? reactor.EnabledCubes.Add(cube)
                        : reactor.EnabledCubes.Remove(cube);
                }
            }

            return reactor.EnabledCubes.Count;

            // static bool isInRange(Cube cube)
            // {
            //     (int x, int y, int z) = cube;
            //     return x >= -50 && x <= 50 && y >= -50 && y <= 50 && z >= -50 && z <= 50;
            // }
        }
    }

    private record Cube(int X, int Y, int Z);

    private record Cuboid(Cube Min, Cube Max)
    {
        public IEnumerable<Cube> Cubes()
        {
            for (int x = Min.X; x <= Max.X; x++)
            {
                for (int y = Min.Y; y <= Max.Y; y++)
                {
                    for (int z = Min.Z; z <= Max.Z; z++)
                    {
                        yield return new(x, y, z);
                    }
                }
            }
        }
        public static Cuboid Parse(string line)
        {
            int[][] ranges = line.Split(',')
                .Select(range => bound(
                    range[2..].Split("..")
                    .Select(ParseInt)
                    .ToArray()))
                .ToArray();

            return new(
                new(ranges[0][0],
                    ranges[1][0],
                    ranges[2][0]),
                new(ranges[0][1],
                    ranges[1][1],
                    ranges[2][1])
            );

            static int[] bound(int[] range)
            {
                (int min, int max) = (range[0], range[1]);

                if ((min < -50 && max < -50) || (min > 50 && max > 50))
                {
                    (min, max) = (0, -1);
                }
                else
                {
                    min = min switch
                    {
                        < -50 => -50,
                        > 50 => 50,
                        _ => min,
                    };
                    max = max switch
                    {
                        < -50 => -50,
                        > 50 => 50,
                        _ => max,
                    };
                }

                (range[0], range[1]) = (min, max);
                return range;
            }
        }
    }
}
