using System;
using Xunit;

namespace Aoc._2019._13.Tests
{
    public class Run_Should
    {
        [Fact]
        public void Stars()
        {
            var run = new Run();
            Assert.Equal("173", run.Job1());
            Assert.Equal("8942", run.Job2());
        }
    }
}
