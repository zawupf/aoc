using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    using Ticket;

    public class Day16Tests
    {
        [Fact]
        public void SilverWorks()
        {
            Assert.Equal(
                71,
                Ticket
                    .Parse(Utils.ReadInputText("16-test"))
                    .InvalidForeignValues()
                    .Sum());
        }

        [Fact]
        public void GoldWorks()
        {
            var ticket = Ticket.Parse(Utils.ReadInputText("16-test2"));
            Assert.Equal(
                new int[] { 1, 0, 2 },
                ticket.MatchRulesToIndex());
            Assert.Equal(
                new Dictionary<string, int> { { "class", 12 }, { "row", 11 }, { "seat", 13 } },
                ticket.ReadMyTicket());
        }

        [Fact]
        public void Stars()
        {
            var run = new Day16();
            Assert.Equal("21071", run.Result1());
            Assert.Equal("3429967441937", run.Result2());
        }
    }
}
