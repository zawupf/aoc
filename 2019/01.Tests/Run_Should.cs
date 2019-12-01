using System;
using Xunit;

namespace Aoc._2019._01.Tests
{
    public class Run_Should
    {
        [Theory]
        [InlineData(2, 12)]
        [InlineData(2, 14)]
        [InlineData(654, 1969)]
        [InlineData(33583, 100756)]
        public void RequiredFuel_Works(int fuel, int mass)
        {
            Assert.Equal(fuel, Run.RequiredFuel(mass));
        }

        [Theory]
        [InlineData(2, 14)]
        [InlineData(966, 1969)]
        [InlineData(50346, 100756)]
        public void RequiredTotalFuel_Works(int fuel, int mass)
        {
            Assert.Equal(fuel, Run.RequiredTotalFuel(mass));
        }
    }
}
