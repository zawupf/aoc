using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day13Tests
    {
        [Fact]
        public void DepartureInfoWorks()
        {
            var result = Day13.DepartureInfo(Utils.ReadInputLines("13-test"));
            Assert.Equal((59, 5, 295), result);
        }

        // [Theory]
        // [InlineData(1068781L, "7,13,x,x,59,x,31,19")]
        // [InlineData(3417L, "17,x,13,19")]
        // [InlineData(754018L, "67,7,59,61")]
        // [InlineData(779210L, "67,x,7,59,61")]
        // [InlineData(1261476L, "67,7,x,59,61")]
        // [InlineData(1202161486L, "1789,37,47,1889")]
        // public void GoldWorks(long result, string input)
        // {
        //     Assert.Equal(result, Day13.Gold(input));
        // }

        [Fact]
        public void Stars()
        {
            var run = new Day13();
            Assert.Equal("3966", run.Result1());
            // Assert.Equal("", run.Result2());
        }
    }
}
