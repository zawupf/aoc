using System.Linq;
using System;
using Xunit;

namespace Aoc._2019._10.Tests
{
    public class Run_Should
    {
        [Fact]
        public void BestLocation_Works()
        {
            var asteroids = Asteroids.Parse(@"
                .#..#
                .....
                #####
                ....#
                ...##
            ");
            var (count, location) = asteroids.BestLocation();
            Assert.Equal(8, count);
            Assert.Equal((3, 4), location);
        }

        [Fact]
        public void Vaporize_Works()
        {
            var asteroids = Asteroids.Parse(@"
                .#..##.###...#######
                ##.############..##.
                .#.######.########.#
                .###.#######.####.#.
                #####.##.#.##.###.##
                ..#####..#.#########
                ####################
                #.####....###.#.#.##
                ##.#################
                #####.##.###..####..
                ..######..##.#######
                ####.##.####...##..#
                .#####..#.######.###
                ##...#.##########...
                #.##########.#######
                .####.#.###.###.#.##
                ....##.##.###..#####
                .#.#.###########.###
                #.#.#.#####.####.###
                ###.##.####.##.#..##
            ");
            var (count, location) = asteroids.BestLocation();
            Assert.Equal((11, 13), location);

            var shots = asteroids.Vaporize(location);
            Assert.Equal((11, 12), shots.ElementAt(0));
            Assert.Equal((11, 12), shots.ElementAt(0));
            Assert.Equal((12, 1), shots.ElementAt(1));
            Assert.Equal((12, 2), shots.ElementAt(2));
            Assert.Equal((12, 8), shots.ElementAt(9));
            Assert.Equal((16, 0), shots.ElementAt(19));
            Assert.Equal((16, 9), shots.ElementAt(49));
            Assert.Equal((10, 16), shots.ElementAt(99));
            Assert.Equal((9, 6), shots.ElementAt(198));
            Assert.Equal((8, 2), shots.ElementAt(199));
            Assert.Equal((10, 9), shots.ElementAt(200));
            Assert.Equal((11, 1), shots.ElementAt(298));
            Assert.Equal((11, 1), shots.Last());
        }

        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("276", run.Job1());
            Assert.Equal("1321", run.Job2());
        }
    }
}
