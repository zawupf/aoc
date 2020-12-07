using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day07Tests
    {
        [Fact]
        public void ParseWorks()
        {
            var rules = BagRules.Parse(Utils.ReadInputLines("07-test"));
            Assert.True(rules.ContainsColor(new("light red")));
            Assert.True(rules.ContainsColor(new("dark orange")));
            Assert.True(rules.ContainsColor(new("bright white")));
            Assert.True(rules.ContainsColor(new("muted yellow")));
            Assert.True(rules.ContainsColor(new("shiny gold")));
            Assert.True(rules.ContainsColor(new("dark olive")));
            Assert.True(rules.ContainsColor(new("vibrant plum")));
            Assert.True(rules.ContainsColor(new("faded blue")));
            Assert.True(rules.ContainsColor(new("dotted black")));

            Assert.False(rules.ContainsColor(new("")));
            Assert.False(rules.ContainsColor(new("foo bar")));

            Assert.Equal(1, rules.SubbagCount(new("light red"), new("bright white")));
            Assert.Equal(2, rules.SubbagCount(new("light red"), new("muted yellow")));
            Assert.Equal(3, rules.SubbagCount(new("dark orange"), new("bright white")));
            Assert.Equal(4, rules.SubbagCount(new("dark orange"), new("muted yellow")));
            Assert.Equal(1, rules.SubbagCount(new("bright white"), new("shiny gold")));
            Assert.Equal(2, rules.SubbagCount(new("muted yellow"), new("shiny gold")));
            Assert.Equal(9, rules.SubbagCount(new("muted yellow"), new("faded blue")));
            Assert.Equal(1, rules.SubbagCount(new("shiny gold"), new("dark olive")));
            Assert.Equal(2, rules.SubbagCount(new("shiny gold"), new("vibrant plum")));
            Assert.Equal(3, rules.SubbagCount(new("dark olive"), new("faded blue")));
            Assert.Equal(4, rules.SubbagCount(new("dark olive"), new("dotted black")));
            Assert.Equal(5, rules.SubbagCount(new("vibrant plum"), new("faded blue")));
            Assert.Equal(6, rules.SubbagCount(new("vibrant plum"), new("dotted black")));
            Assert.Equal(0, rules.SubbagCount(new("faded blue"), new("dotted black")));
            Assert.Equal(0, rules.SubbagCount(new("dotted black"), new("foo bar")));
        }

        [Fact]
        public void BagsToHoldWorks()
        {
            var rules = BagRules.Parse(Utils.ReadInputLines("07-test"));
            Assert.Equal(4, rules.BagsToHold(new("shiny gold")).Length);
        }

        [Fact]
        public void TotalBagCountWorks()
        {
            var rules = BagRules.Parse(Utils.ReadInputLines("07-test"));
            Assert.Equal(32, rules.TotalSubbagCount(new("shiny gold")));
        }

        [Fact]
        public void Stars()
        {
            var run = new Day07();
            Assert.Equal("115", run.Result1());
            Assert.Equal("1250", run.Result2());
        }
    }
}
