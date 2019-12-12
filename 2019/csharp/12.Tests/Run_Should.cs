using System;
using System.Linq;
using Xunit;

namespace Aoc._2019._12.Tests
{
    public class Run_Should
    {
        [Fact]
        public void ParseMoon_works()
        {
            Assert.Equal(new Moon((-1, 0, 2)), Moon.Parse("<x=-1, y=0, z=2>"));
        }

        [Fact]
        public void Steps_works()
        {
            var expectedSteps = new[] {
                "pos=<x=-1, y=0, z=2>, vel=<x=0, y=0, z=0>",
                "pos=<x=2, y=-1, z=1>, vel=<x=3, y=-1, z=-1>",
                "pos=<x=5, y=-3, z=-1>, vel=<x=3, y=-2, z=-2>",
                "pos=<x=5, y=-6, z=-1>, vel=<x=0, y=-3, z=0>",
                "pos=<x=2, y=-8, z=0>, vel=<x=-3, y=-2, z=1>",
                "pos=<x=-1, y=-9, z=2>, vel=<x=-3, y=-1, z=2>",
                "pos=<x=-1, y=-7, z=3>, vel=<x=0, y=2, z=1>",
                "pos=<x=2, y=-2, z=1>, vel=<x=3, y=5, z=-2>",
                "pos=<x=5, y=2, z=-2>, vel=<x=3, y=4, z=-3>",
                "pos=<x=5, y=3, z=-4>, vel=<x=0, y=1, z=-2>",
                "pos=<x=2, y=1, z=-3>, vel=<x=-3, y=-2, z=1>",
            };
            var planet = Planet.Parse("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>");

            var steps = planet.Steps().GetEnumerator();
            bool hasCurrent = steps.MoveNext();
            for (int i = 0; hasCurrent && i <= 10; ++i, hasCurrent = steps.MoveNext())
            {
                Assert.Equal(expectedSteps[i], steps.Current.Moons[0].ToString());
            }
        }

        [Fact]
        public void Steps_no_Clone_works()
        {
            Assert.Equal(
                "pos=<x=2, y=1, z=-3>, vel=<x=-3, y=-2, z=1>",
                Planet.Parse("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>")
                    .Steps(false).Take(11).Last().Moons[0].ToString()
            );
            Assert.Equal(
                "pos=<x=2, y=1, z=-3>, vel=<x=-3, y=-2, z=1>",
                Planet.Parse("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>")
                    .Steps(false).ElementAt(10).Moons[0].ToString()
            );
        }

        [Fact]
        public void TotalEnergy_works()
        {
            Assert.Equal(
                1940,
                Planet.Parse("<x=-8, y=-10, z=0>\n<x=5, y=5, z=10>\n<x=2, y=-7, z=3>\n<x=9, y=-8, z=-3>")
                    .Steps(false).ElementAt(100).TotalEnergy()
            );
        }

        [Fact]
        public void Equals_works()
        {
            var p1 = Planet.Parse("<x=-8, y=-10, z=0>\n<x=5, y=5, z=10>\n<x=2, y=-7, z=3>\n<x=9, y=-8, z=-3>");
            var p2 = Planet.Parse("<x=-8, y=-10, z=0>\n<x=5, y=5, z=10>\n<x=2, y=-7, z=3>\n<x=9, y=-8, z=-3>");
            Assert.True(Enumerable.SequenceEqual(p1.Moons, p2.Moons));
        }

        [Fact]
        public void CycleSteps_works()
        {
            var planet = Planet.Parse("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>");
            Assert.Equal(2772, planet.CycleSteps());

            planet = Planet.Parse("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>");
            Assert.Equal(2772, planet.CycleSteps2());

            planet = Planet.Parse("<x=-8, y=-10, z=0>\n<x=5, y=5, z=10>\n<x=2, y=-7, z=3>\n<x=9, y=-8, z=-3>");
            Assert.Equal(4686774924, planet.CycleSteps2());
        }

        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("7179", run.Job1());
            Assert.Equal("428576638953552", run.Job2());
        }
    }
}
