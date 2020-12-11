using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day11Tests
    {
        [Fact]
        public void StabeStateWorks()
        {
            Assert.Equal(
                37,
                Day11.StableState(
                    Seats.Parse(
                        Utils.ReadInputLines("11-test")), 4, false)
                .Count(Seats.Type.OccupiedSeat));
            Assert.Equal(
                26,
                Day11.StableState(
                    Seats.Parse(
                        Utils.ReadInputLines("11-test")), 5, true)
                .Count(Seats.Type.OccupiedSeat));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day11();
            Assert.Equal("2310", run.Result1());
            Assert.Equal("2074", run.Result2());
        }
    }
}
