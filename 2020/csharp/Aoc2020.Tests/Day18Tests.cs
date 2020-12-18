using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    using Ticket;

    public class Day18Tests
    {
        [Fact]
        public void SilverWorks()
        {
            Assert.Equal(71L, Day18.EvaluateSimple("1 + 2 * 3 + 4 * 5 + 6"));
            Assert.Equal(51L, Day18.EvaluateSimple("1 + (2 * 3) + (4 * (5 + 6))"));
            Assert.Equal(26L, Day18.EvaluateSimple("2 * 3 + (4 * 5)"));
            Assert.Equal(437L, Day18.EvaluateSimple("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Assert.Equal(12240L, Day18.EvaluateSimple("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Assert.Equal(13632L, Day18.EvaluateSimple("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
        }

        [Fact]
        public void GoldWorks()
        {
            Assert.Equal(231L, Day18.EvaluateAdvanced("1 + 2 * 3 + 4 * 5 + 6"));
            Assert.Equal(51L, Day18.EvaluateAdvanced("1 + (2 * 3) + (4 * (5 + 6))"));
            Assert.Equal(46L, Day18.EvaluateAdvanced("2 * 3 + (4 * 5)"));
            Assert.Equal(1445L, Day18.EvaluateAdvanced("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Assert.Equal(669060L, Day18.EvaluateAdvanced("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Assert.Equal(23340L, Day18.EvaluateAdvanced("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day18();
            Assert.Equal("69490582260", run.Result1());
            Assert.Equal("362464596624526", run.Result2());
        }
    }
}
