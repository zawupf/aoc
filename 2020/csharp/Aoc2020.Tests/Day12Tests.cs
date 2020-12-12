using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day12Tests
    {
        [Fact]
        public void MoveWorks()
        {
            Assert.Equal(
                25,
                Day12.ManhattanDistance(
                    Utils.ReadInputLines("12-test")
                    .Select(Movement.Parse)
                    .Aggregate((0, 0, 0), Day12.Move)));
        }

        [Fact]
        public void MoveWaypointWorks()
        {
            Assert.Equal(
                286,
                Day12.ManhattanDistance(
                    Utils.ReadInputLines("12-test")
                    .Select(Movement.Parse)
                    .Aggregate(((0, 0), (10, 1)), Day12.MoveWaypoint)));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day12();
            Assert.Equal("445", run.Result1());
            Assert.Equal("42495", run.Result2());
        }
    }
}
