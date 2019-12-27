using System;
using System.Linq;
using Xunit;

namespace Aoc._2019._09.Tests
{
    public class Run_Should
    {
        [Fact]
        public void Test_Boost_Works()
        {
            var run = new Run();
            var outputs = run.Boost(1);
            Assert.Single(outputs);
        }

        [Fact]
        public void Stars()
        {
            var run = new Run();
            Assert.Equal("2941952859", run.Job1());
            Assert.Equal("66113", run.Job2());
        }
    }
}
