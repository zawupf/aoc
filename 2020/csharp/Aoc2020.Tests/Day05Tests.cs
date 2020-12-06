using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day05Tests
    {
        [Theory]
        [InlineData("FBFBBFFRLR", 44, 5, 357)]
        [InlineData("BFFFBBFRRR", 70, 7, 567)]
        [InlineData("FFFBBBFRRR", 14, 7, 119)]
        [InlineData("BBFFBBFRLL", 102, 4, 820)]
        public void SeatSelectionWorks(string code, int row, int column, int seatId)
        {
            Assert.Equal(row, Day05.Row(code));
            Assert.Equal(column, Day05.Column(code));
            Assert.Equal(seatId, Day05.SeatId(code));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day05();
            Assert.Equal("930", run.Result1());
            Assert.Equal("515", run.Result2());
        }
    }
}
