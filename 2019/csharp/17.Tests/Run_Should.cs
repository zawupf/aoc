using System;
using Xunit;

namespace Aoc._2019._17.Tests
{
    public class Run_Should
    {
        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("2804", run.Job1());
        }
    }
}
