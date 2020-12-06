using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day06Tests
    {
        [Fact]
        public void Stars()
        {
            var run = new Day06();
            Assert.Equal("6768", run.Result1());
            Assert.Equal("3489", run.Result2());
        }
    }
}
