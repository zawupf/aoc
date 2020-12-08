using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day08Tests
    {
        [Fact]
        public void InfiniteLoopDetectionWorks()
        {
            var code = Code.Parse(Utils.ReadInputLines("08-test"));
            code.Run();
            Assert.Equal(5, code.Accumulator);
        }

        [Fact]
        public void MutateCodeWorks()
        {
            Assert.Equal(8, Day08.MutateCode(Utils.ReadInputLines("08-test")).Accumulator);
        }

        [Fact]
        public void Stars()
        {
            var run = new Day08();
            Assert.Equal("1753", run.Result1());
            Assert.Equal("733", run.Result2());
        }
    }
}
