using System;
using System.Collections.Generic;
using System.Linq;
using static Aoc._2019.Utils;

namespace Aoc._2019._06
{
    public class Run : IRun
    {
        public string Job1()
        {
            var orbits = UniversalOrbitMap.Parse(ReadInputLines("06"));
            return orbits.Checksum().ToString();
        }

        public string Job2()
        {
            var orbits = UniversalOrbitMap.Parse(ReadInputLines("06"));
            return orbits.MinimalTransfersCount().ToString();
        }
    }

    public class UniversalOrbitMap
    {
        private Dictionary<string, HashSet<string>> planet2moons = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, string> moon2planet = new Dictionary<string, string>();

        static public UniversalOrbitMap Parse(IEnumerable<string> lines)
        {
            var orbits = new UniversalOrbitMap();
            foreach (var line in lines)
            {
                var masses = line.Split(')');
                var planet = masses[0];
                var moon = masses[1];
                orbits.Add(planet, moon);
            }
            return orbits;
        }

        public void Add(string planet, string moon)
        {
            if (planet2moons.GetValueOrDefault(planet)?.Add(moon) is null)
                planet2moons.Add(planet, new HashSet<string> { moon });

            moon2planet.Add(moon, planet);
        }

        public HashSet<string> MoonsOf(string planet) => planet2moons.GetValueOrDefault(planet);

        public string PlanetOf(string moon) => moon2planet.GetValueOrDefault(moon);

        public IEnumerable<string> AllPlanetsOf(string moon)
        {
            for (var planet = PlanetOf(moon); planet is string; planet = PlanetOf(planet))
                yield return planet;
        }

        public int Checksum()
        {
            var result =
                from moon in moon2planet.Keys
                select (from planet in AllPlanetsOf(moon) select planet).Count();
            return result.Sum();
        }

        public int MinimalTransfersCount()
        {
            var you = AllPlanetsOf("YOU");
            var san = AllPlanetsOf("SAN");
            var uniques1 = you.Except(san);
            var uniques2 = san.Except(you);
            return uniques1.Count() + uniques2.Count();
        }
    }
}
