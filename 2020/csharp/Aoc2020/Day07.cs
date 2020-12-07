using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day07 : IDay
    {
        public override string Day { get; } = nameof(Day07)[3..];

        public override string Result1() =>
            BagRules.Parse(InputLines)
            .BagsToHold(new("shiny gold"))
            .Length
            .ToString();


        public override string Result2() =>
            BagRules.Parse(InputLines)
            .TotalSubbagCount(new("shiny gold"))
            .ToString();
    }

    public record BagRules()
    {
        public record BagColor(string Value);
        public record Bag(BagColor Color, Dictionary<BagColor, int> Subbags);

        private Dictionary<BagColor, Bag> Rules { get; init; } = new();

        public Bag[] BagsToHold(BagColor bagColor)
        {
            var directBags =
                Rules.Values
                .Where(bag => SubbagCount(bag.Color, bagColor) > 0);

            var indirectBags =
                directBags
                .Select(bag => BagsToHold(bag.Color));

            return
                indirectBags
                .Aggregate(directBags, (result, bags) => result.Concat(bags))
                .Distinct()
                .ToArray();
        }

        public int TotalSubbagCount(BagColor bagColor) =>
            Rules[bagColor]
            .Subbags
            .Select(kv =>
            {
                var (bagColor, count) = kv;
                return count + count * TotalSubbagCount(bagColor);
            })
            .Aggregate(0, (result, count) => result + count);

        public bool ContainsColor(BagColor color) => Rules.ContainsKey(color);
        public int SubbagCount(BagColor bag, BagColor subbag)
        {
            try
            {
                return Rules[bag].Subbags[subbag];
            }
            catch (KeyNotFoundException)
            {
                return 0;
            }
        }

        public static BagRules Parse(IEnumerable<string> lines) =>
            new BagRules() with
            {
                Rules = lines
                    .Select(line => ParseBag(line))
                    .ToDictionary((bag) => bag.Color)
            };

        private static readonly Regex rxBag = new(@"^(.*?) bags contain (.*).$");
        public static Bag ParseBag(string line)
        {
            var groups = rxBag.Match(line).Groups;
            BagColor color = new(groups[1].Value);
            var subbags = ParseSubbags(groups[2].Value);

            return new(color, subbags);
        }

        private static readonly Regex rxSubbag = new(@"^(\d+) (.*) bags?$");
        public static Dictionary<BagColor, int> ParseSubbags(string data)
        {
            return
                data
                .Split(", ")
                .Select(ToColorCount)
                .Aggregate(new Dictionary<BagColor, int>(), BuildBagCountDict);

            static (BagColor, int) ToColorCount(string bagCount)
            {
                if (bagCount == "no other bags")
                {
                    return (new BagColor(""), 0);
                }

                var groups = rxSubbag.Match(bagCount).Groups;
                return (new BagColor(groups[2].Value), int.Parse(groups[1].Value));
            }

            static Dictionary<BagColor, int> BuildBagCountDict(
                Dictionary<BagColor, int> result, (BagColor, int) bagColor)
            {
                var (color, count) = bagColor;
                if (count != 0)
                {
                    result.Add(color, count);
                }
                return result;
            }
        }
    }
}
