using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day04Tests
    {
        [Fact]
        public void TestDataWorks()
        {
            Assert.Equal(2,
                Passport.ParseMany(Utils.ReadInputText("04-test"))
                .Count(passport => passport.AreRequiredFieldsPresent()));

            Assert.Equal(4,
                Passport.ParseMany(Utils.ReadInputText("04-valid"))
                .Count(passport => passport.IsValid()));

            Assert.Equal(0,
                Passport.ParseMany(Utils.ReadInputText("04-invalid"))
                .Count(passport => passport.IsValid()));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day04();
            Assert.Equal("190", run.Result1());
            Assert.Equal("121", run.Result2());
        }
    }
}
