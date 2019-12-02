using System;
using Xunit;

namespace Aoc._2019._02.Tests
{
    public class Run_Should
    {
        [Theory]
        [InlineData(new int[] { 1, 0, 0, 0, 99 },
                    new int[] { 2, 0, 0, 0, 99 })]
        [InlineData(new int[] { 2, 3, 0, 3, 99 },
                    new int[] { 2, 3, 0, 6, 99 })]
        [InlineData(new int[] { 2, 4, 4, 5, 99, 0 },
                    new int[] { 2, 4, 4, 5, 99, 9801 })]
        [InlineData(new int[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 },
                    new int[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void Exec_Works(int[] code, int[] expected)
        {
            var result = new Computer(code).Exec();
            Assert.NotEqual(code, result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("3765464", run.Job1());
            Assert.Equal("7610", run.Job2());
        }
    }
}
