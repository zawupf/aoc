using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day09Tests
    {
        [Fact]
        public void FindFirstInvalidNumberWorks()
        {
            var data = Utils.ReadInputLines("09-test").Select(long.Parse).ToArray();
            Assert.Equal(127, Day09.FindFirstInvalidNumber(data, 5));
        }

        [Fact]
        public void FindWeaknessNumberWorks()
        {
            var data = Utils.ReadInputLines("09-test").Select(long.Parse).ToArray();
            Assert.Equal(62, Day09.FindWeakness(data, 127));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day09();
            Assert.Equal("167829540", run.Result1());
            Assert.Equal("28045630", run.Result2());
        }
    }
}
