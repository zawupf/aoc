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
            return Planet.Parse(ReadLines("12/input1.txt"))
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

        public Moon ViewX()
        {
            var (px, py, pz) = Position;
            var (vx, vy, vz) = Velocity;
            return new Moon((0, py, pz), (0, vy, vz));
        }

        public Moon ViewY()
        {
            var (px, py, pz) = Position;
            var (vx, vy, vz) = Velocity;
            return new Moon((px, 0, pz), (vx, 0, vz));
        }

        public Moon ViewZ()
        {
            var (px, py, pz) = Position;
            var (vx, vy, vz) = Velocity;
            return new Moon((px, py, 0), (vx, vy, 0));
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
            long stepsX = ViewX().CycleSteps();
            long stepsY = ViewY().CycleSteps();
            long stepsZ = ViewZ().CycleSteps();
            return LCD(stepsX, LCD(stepsY, stepsZ));

            long GCD(long a, long b)
            {
                if (b == 0) return a;
                return GCD(b, a % b);
            }

            long LCD(long a, long b)
            {
                return (a * b) / GCD(a, b);
            }

        }

        private Planet ViewX()
        {
            var moons = (from moon in Moons select moon.ViewX()).ToArray();
            return new Planet(moons);
        }

        private Planet ViewY()
        {
            var moons = (from moon in Moons select moon.ViewY()).ToArray();
            return new Planet(moons);
        }

        private Planet ViewZ()
        {
            var moons = (from moon in Moons select moon.ViewZ()).ToArray();
            return new Planet(moons);
        }
    }
}
