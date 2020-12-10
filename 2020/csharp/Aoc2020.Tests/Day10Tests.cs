using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day10Tests
    {
        [Fact]
        public void OnesTimesThreesWorks()
        {
            Assert.Equal(7 * 5, Day10.OnesTimesThrees(Utils.ReadInputLines("10-test1")));
            Assert.Equal(22 * 10, Day10.OnesTimesThrees(Utils.ReadInputLines("10-test2")));
        }

        [Fact]
        public void SupportedArrangementsCountWorks()
        {
            Assert.Equal(8L, Day10.SupportedArrangementsCount(Utils.ReadInputLines("10-test1")));
            Assert.Equal(19208L, Day10.SupportedArrangementsCount(Utils.ReadInputLines("10-test2")));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day10();
            Assert.Equal("2059", run.Result1());
            Assert.Equal("86812553324672", run.Result2());
        }
    }
}
