using System;
using Xunit;

namespace Aoc._2019._14.Tests
{
    public class Run_Should
    {
        [Theory]
        [InlineData(10, "A", "10 A")]
        [InlineData(10, "A", "  10   A  ")]
        [InlineData(10, "A  b  c  d", "  10   A  b  c  d ")]
        public void Chemical_Parse_works(int qty, string nam, string text)
        {
            var expected = new Chemical(qty, nam);
            var parsed = Chemical.Parse(text);
            Assert.Equal(expected, parsed);
        }

        [Theory]
        [InlineData("10 ORE => 10 A")]
        [InlineData("1 ORE => 1 B")]
        [InlineData("7 A, 1 B => 1 C")]
        [InlineData("7 A, 1 C => 1 D")]
        [InlineData("7 A, 1 D => 1 E")]
        [InlineData("7 A, 1 E => 1 FUEL")]
        public void Reaction_Parse_works(string text)
        {
            var reaction = Reaction.Parse(text);
            Assert.Equal(text, reaction.ToString());
        }

        [Theory]
        [InlineData(31, "14/sample1.txt")]
        [InlineData(165, "14/sample2.txt")]
        [InlineData(13312, "14/sample3.txt")]
        [InlineData(180697, "14/sample4.txt")]
        [InlineData(2210736, "14/sample5.txt")]
        public void Factory_Take_works(int oreCount, string path)
        {
            var run = new Run("../../../../");
            var factory = Factory.Parse(run.ReadLines(path));
            factory.Insert(new Chemical(long.MaxValue, "ORE"));
            Assert.Equal(oreCount, factory.Take("1 FUEL"));
        }

        [Theory]
        [InlineData(82892753, "14/sample3.txt")]
        [InlineData(5586022, "14/sample4.txt")]
        [InlineData(460664, "14/sample5.txt")]
        public void Factory_Buy_works(int fuelCount, string path)
        {
            var run = new Run("../../../../");
            var factory = Factory.Parse(run.ReadLines(path));
            factory.Insert("1000000000000 ORE");
            Assert.Equal(fuelCount, factory.BuyFuel());
        }

        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("97422", run.Job1());
            Assert.Equal("13108426", run.Job2());
        }
    }
}
