using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day16Tests
    {
        [Fact]
        public void SilverWorks()
        {
            // Assert.Equal(
            //     new int[] { 0, 3, 6, 0, 3, 3, 1, 0, 4, 0 },
            //     Day16.Play("0,3,6").Take(10).ToArray());
            // Assert.Equal(436, Day16.Play("0,3,6").ElementAt(2019));
        }

        [Fact]
        public void GoldWorks()
        {
            // Assert.Equal(175594, Day16.Play("0,3,6").ElementAt(30000000 - 1));

            // Skipped for performance reasons
            // Assert.Equal(2578, Day16.Play("1,3,2").ElementAt(30000000 - 1));
            // Assert.Equal(3544142, Day16.Play("2,1,3").ElementAt(30000000 - 1));
            // Assert.Equal(261214, Day16.Play("1,2,3").ElementAt(30000000 - 1));
            // Assert.Equal(6895259, Day16.Play("2,3,1").ElementAt(30000000 - 1));
            // Assert.Equal(18, Day16.Play("3,2,1").ElementAt(30000000 - 1));
            // Assert.Equal(362, Day16.Play("3,1,2").ElementAt(30000000 - 1));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day16();
            // Assert.Equal("", run.Result1());
            // Assert.Equal("", run.Result2()); // Skipped for performance reasons
        }
    }
}
