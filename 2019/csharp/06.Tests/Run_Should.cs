using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Aoc._2019._06.Tests
{
    public class Run_Should
    {
        private static string[] testOrbits = new string[] {
            "COM)B",
            "B)C",
            "C)D",
            "D)E",
            "E)F",
            "B)G",
            "G)H",
            "D)I",
            "E)J",
            "J)K",
            "K)L",
        };

        private static string[] testOrbits2 = new string[] {
            "COM)B",
            "B)C",
            "C)D",
            "D)E",
            "E)F",
            "B)G",
            "G)H",
            "D)I",
            "E)J",
            "J)K",
            "K)L",
            "K)YOU",
            "I)SAN",
        };

        [Fact]
        public void UniversalOrbitMap_Add_Works()
        {
            var orbits = new UniversalOrbitMap();
            orbits.Add("A", "B");
            orbits.Add("A", "C");

            Assert.Equal("A", orbits.PlanetOf("B"));
            Assert.Equal("A", orbits.PlanetOf("C"));

            Assert.Equal(new HashSet<string> { "B", "C" }, orbits.MoonsOf("A"));
        }

        [Fact]
        public void UniversalOrbitMap_Parse_Works()
        {
            var orbits = UniversalOrbitMap.Parse(testOrbits);

            Assert.Equal("COM", orbits.PlanetOf("B"));
            Assert.Equal("E", orbits.PlanetOf("J"));
            Assert.Equal("E", orbits.PlanetOf("F"));
            Assert.Equal(new HashSet<string> { "E", "I" }, orbits.MoonsOf("D"));
        }

        [Fact]
        public void UniversalOrbitMap_AllPlanetsOf_Works()
        {
            var orbits = UniversalOrbitMap.Parse(testOrbits);
            var allPlanetsQuery =
                from planet in orbits.AllPlanetsOf("L") select planet;
            Assert.Equal(
                new string[] { "K", "J", "E", "D", "C", "B", "COM" },
                allPlanetsQuery.ToArray()
            );
        }

        [Fact]
        public void UniversalOrbitMap_Checksum_Works()
        {
            var orbits = UniversalOrbitMap.Parse(testOrbits);
            Assert.Equal(42, orbits.Checksum());
        }

        [Fact]
        public void UniversalOrbitMap_MinimalTransfersCount_Works()
        {
            var orbits = UniversalOrbitMap.Parse(testOrbits2);
            Assert.Equal(4, orbits.MinimalTransfersCount());
        }

        [Fact]
        public void Stars()
        {
            var run = new Run();
            Assert.Equal("247089", run.Job1());
            Assert.Equal("442", run.Job2());
        }
    }
}
