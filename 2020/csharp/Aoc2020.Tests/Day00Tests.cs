using Xunit;

namespace Aoc2020.Tests
{
    public class Day00Tests
    {
        [Theory]
        [InlineData(2, 12)]
        [InlineData(2, 14)]
        [InlineData(654, 1969)]
        [InlineData(33583, 100756)]
        public void RequiredFuel_Works(int fuel, int mass)
        {
            Assert.Equal(fuel, Day00.RequiredFuel(mass));
        }

        [Theory]
        [InlineData(2, 14)]
        [InlineData(966, 1969)]
        [InlineData(50346, 100756)]
        public void RequiredTotalFuel_Works(int fuel, int mass)
        {
            Assert.Equal(fuel, Day00.RequiredTotalFuel(mass));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day00();
            Assert.Equal("3421505", run.Result1());
            Assert.Equal("5129386", run.Result2());
        }
    }
}
