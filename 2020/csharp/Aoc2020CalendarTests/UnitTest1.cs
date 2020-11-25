using System;
using Xunit;
using Aoc2020Calendar;

namespace Aoc2020CalendarTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var foo = new Foo("bar");
            Assert.True(foo == new Foo("bar"));
        }
    }
}
