using System;
using Xunit;

namespace Aoc._2019._04.Tests
{
    public class Run_Should
    {
        [Fact]
        public void IsValidAdjacentDigits_Works()
        {
            Assert.True(Run.IsValidAdjacentDigits(111111));
            Assert.True(Run.IsValidAdjacentDigits(223450));
            Assert.False(Run.IsValidAdjacentDigits(123789));
        }

        [Fact]
        public void IsValidIncreasingDigits_Works()
        {
            Assert.True(Run.IsValidIncreasingDigits(111111));
            Assert.False(Run.IsValidIncreasingDigits(223450));
            Assert.True(Run.IsValidIncreasingDigits(123789));
        }

        [Fact]
        public void IsValidAdjacentDigits2_Works()
        {
            Assert.False(Run.IsValidAdjacentDigits2(111111));
            Assert.True(Run.IsValidAdjacentDigits2(223450));
            Assert.False(Run.IsValidAdjacentDigits2(123789));
            Assert.True(Run.IsValidAdjacentDigits2(112233));
            Assert.False(Run.IsValidAdjacentDigits2(123444));
            Assert.True(Run.IsValidAdjacentDigits2(111122));
        }

        [Fact]
        public void Stars()
        {
            var run = new Run();
            Assert.Equal("2050", run.Job1());
            Assert.Equal("1390", run.Job2());
        }
    }
}
