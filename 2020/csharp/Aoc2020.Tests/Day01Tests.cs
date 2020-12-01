using Xunit;

namespace Aoc2020.Tests
{
    public class Day01Tests
    {
        [Fact]
        public void FindTwoEntriesWithSum_Works()
        {
            int[] expenseReport = { 1721, 979, 366, 299, 675, 1456 };
            Assert.Equal((1721, 299), Day01.FindTwoEntriesWithSum(2020, expenseReport));
        }

        [Fact]
        public void FindThreeEntriesWithSum_Works()
        {
            int[] expenseReport = { 1721, 979, 366, 299, 675, 1456 };
            Assert.Equal((979, 366, 675), Day01.FindThreeEntriesWithSum(2020, expenseReport));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day01();
            Assert.Equal("1018944", run.Result1());
            Assert.Equal("8446464", run.Result2());
        }
    }
}
