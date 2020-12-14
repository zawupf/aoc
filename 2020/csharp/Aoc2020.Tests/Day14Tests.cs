using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day14Tests
    {
        [Fact]
        public void RunWorks()
        {
            Assert.Equal(165ul, new Computer().Run(Utils.ReadInputLines("14-test")).MemSum);
        }

        [Fact]
        public void Run2Works()
        {
            Assert.Equal(208ul, new Computer().Run2(Utils.ReadInputLines("14-test2")).MemSum);
        }

        [Fact]
        public void Stars()
        {
            var run = new Day14();
            Assert.Equal("2346881602152", run.Result1());
            Assert.Equal("3885232834169", run.Result2());
        }
    }
}
