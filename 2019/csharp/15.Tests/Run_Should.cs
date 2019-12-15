using System;
using Xunit;

namespace Aoc._2019._15.Tests
{
    public class Run_Should
    {
        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("238", run.Job1());
            Assert.Equal("392", run.Job2());
        }
    }
}
