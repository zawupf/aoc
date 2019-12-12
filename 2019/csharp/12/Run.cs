using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Aoc._2019._12
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            return Planet.Parse(ReadLines("12/input1.txt"))
                .Steps(false)
                .ElementAt(1000)
                .TotalEnergy()
                .ToString();
        }

        public string Job2()
        {
            // return Planet.Parse(ReadLines("12/input1.txt"))
            // return Planet.Parse("<x=-8, y=-10, z=0>\n<x=5, y=5, z=10>\n<x=2, y=-7, z=3>\n<x=9, y=-8, z=-3>")
            return Planet.Parse("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>")
                .CycleSteps2()
                .ToString();
        }
    }

    public class Moon
    {
        public (int x, int y, int z) Position { get; private set; }
        public (int x, int y, int z) Velocity { get; private set; }


        public Moon((int x, int y, int z) position)
        {
            this.Position = position;
            this.Velocity = (0, 0, 0);
        }

        public Moon((int x, int y, int z) position, (int x, int y, int z) velocity)
        {
            this.Position = position;
            this.Velocity = velocity;
        }

        public override string ToString()
        {
            var (px, py, pz) = Position;
            var (vx, vy, vz) = Velocity;
            return $"pos=<x={px}, y={py}, z={pz}>, vel=<x={vx}, y={vy}, z={vz}>";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Moon)obj;
            return Position == other.Position && Velocity == other.Velocity;
        }

        public override int GetHashCode()
        {
            return (Position, Velocity).GetHashCode();
        }

        public static Moon Parse(string line)
        {
            var (begin, data, end) = (line[0], line[1..^1], line[^1]);
            if (begin != '<' || end != '>')
                throw new Exception("Invalid moon syntax");

            var chunks = data.Split(", ");
            var position = ParsePosition(chunks);

            return new Moon(position);

            (int x, int y, int z) ParsePosition(string[] chunks)
            {
                var x = ParseValue('x', chunks[0]);
                var y = ParseValue('y', chunks[1]);
                var z = ParseValue('z', chunks[2]);
                return (x, y, z);
            }

            int ParseValue(char name, string data)
            {
                if (data[0] != name || data[1] != '=')
                    throw new Exception($"Invalid position data: {data}");

                return int.Parse(data[2..]);
            }
        }

        public void UpdateVelocity(Moon moon)
        {
            var (px1, py1, pz1) = Position;
            var (px2, py2, pz2) = moon.Position;
            var dx = Update(px1, px2);
            var dy = Update(py1, py2);
            var dz = Update(pz1, pz2);

            var (vx, vy, vz) = Velocity;
            Velocity = (vx + dx, vy + dy, vz + dz);

            return;

            int Update(int p1, int p2)
            {
                var d = Delta(p1, p2);
                return d;
            }

            int Delta(int p1, int p2)
            {
                if (p1 < p2)
                    return 1;
                if (p2 < p1)
                    return -1;
                return 0;
            }
        }

        public void UpdatePosition()
        {
            var (px, py, pz) = Position;
            var (vx, vy, vz) = Velocity;
            Position = (px + vx, py + vy, pz + vz);
        }

        public int PotentialEnergy()
        {
            var (x, y, z) = Position;
            return Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
        }

        public int KineticEnergy()
        {
            var (x, y, z) = Velocity;
            return Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
        }

        public int TotalEnergy()
        {
            return PotentialEnergy() * KineticEnergy();
        }
    }

    public class Planet
    {
        public Moon[] Moons { get; }

        public Planet(Moon[] moons)
        {
            this.Moons = moons;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var moon in Moons)
            {
                result.Append($"\n{moon.ToString()}");
            }
            return result.ToString();
        }

        public static Planet Parse(string lines)
        {
            return Parse(lines.Split('\n'));
        }

        public static Planet Parse(IEnumerable<string> lines)
        {
            var moons = (
                from line in lines
                select Moon.Parse(line)
            ).ToArray();
            var planet = new Planet(moons);
            return planet;
        }

        Planet Clone()
        {
            var planet = new Planet(Moons.Clone() as Moon[]);
            for (int i = 0; i < planet.Moons.Length; ++i)
            {
                var moon = planet.Moons[i];
                planet.Moons[i] = new Moon(moon.Position, moon.Velocity);
            }
            return planet;
        }

        public IEnumerable<Planet> Steps(bool clone = true)
        {
            while (true)
            {
                var planet = clone ? Clone() : this;
                yield return planet;

                UpdateVelocities();
                UpdatePositions();
            }

            void UpdateVelocities()
            {
                foreach (var moon1 in Moons)
                {
                    foreach (var moon2 in Moons)
                    {
                        moon1.UpdateVelocity(moon2);
                    }
                }
            }

            void UpdatePositions()
            {
                foreach (var moon in Moons)
                {
                    moon.UpdatePosition();
                }
            }
        }

        public int PotentialEnergy()
        {
            return (from moon in Moons select moon.PotentialEnergy()).Sum();
        }

        public int KineticEnergy()
        {
            return (from moon in Moons select moon.KineticEnergy()).Sum();
        }

        public int TotalEnergy()
        {
            return (from moon in Moons select moon.TotalEnergy()).Sum();
        }

        public long CycleSteps()
        {
            long steps = 0;
            var initialPlanet = Clone();
            foreach (var _ in Steps(false).Skip(1))
            {
                ++steps;
                if (Enumerable.SequenceEqual(Moons, initialPlanet.Moons))
                    break;
            }
            return steps;
        }

        public long CycleSteps2()
        {
            long steps = 0;
            var initialPlanet = Clone();
            var initialPotentialEnergy = initialPlanet.PotentialEnergy();
            var initialKineticEnergy = initialPlanet.KineticEnergy();
            var initialTotalEnergy = initialPlanet.TotalEnergy();
            var (pot, kin, tot) = (true, true, true);
            foreach (var _ in Steps(false).Skip(1))
            {
                ++steps;
                if (pot && initialPotentialEnergy == PotentialEnergy())
                {
                    pot = false;
                    Console.WriteLine($"pot after: {steps}");
                }
                if (initialKineticEnergy == KineticEnergy())
                {
                    kin = false;
                    Console.WriteLine($"kin after: {steps}");
                }
                if (initialTotalEnergy == TotalEnergy())
                {
                    tot = false;
                    Console.WriteLine($"tot after: {steps}");
                }
                if (Math.Abs(PotentialEnergy() - KineticEnergy()) < 2)
                {
                    Console.WriteLine($"same egy: {steps}");
                }

                if (!pot && !kin && !tot)
                    break;
            }
            return steps;
        }
    }
}
