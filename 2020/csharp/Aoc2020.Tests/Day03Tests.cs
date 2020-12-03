using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day03Tests
    {
        private static readonly Forest forest = Forest.Parse(Utils.ReadInputLines("03-test"));
        private static readonly Slope[] Slopes =
        {
            new(1, 1),
            new(3, 1),
            new(5, 1),
            new(7, 1),
            new(1, 2),
        };

        public static IEnumerable<object[]> Slopes_Data() =>
            new int[] { 2, 7, 3, 4, 2 }
                .Zip(Slopes)
                .Select(data => new object[] { data.First, data.Second });

        [Theory]
        [MemberData(nameof(Slopes_Data))]
        public void HikeWorks(int result, Slope slope)
        {
            Assert.Equal(result, Day03.Hike(forest, slope)[Forest.Creature.Tree]);
        }

        [Fact]
        public void Stars()
        {
            var run = new Day03();
            Assert.Equal("167", run.Result1());
            Assert.Equal("736527114", run.Result2());
        }
    }
}
