using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day16 : IDay
    {
        public override string Day { get; } = nameof(Day16)[3..];

        public override string Result1() =>
            Ticket.Ticket.Parse(InputText)
            .InvalidForeignValues()
            .Sum()
            .ToString();

        public override string Result2() =>
            Ticket.Ticket.Parse(InputText)
            .ReadMyTicket()
            .Where(kv => kv.Key.StartsWith("departure"))
            .Aggregate(1L, (result, kv) => result * kv.Value)
            .ToString();
    }
}

namespace Aoc2020.Ticket
{
    public record Range(int Min, int Max)
    {
        public bool IsValid(int value) => value >= Min && value <= Max;
    }

    public record Rule(string Name, Range Range1, Range Range2)
    {
        public bool IsValid(int value) =>
            Range1.IsValid(value) || Range2.IsValid(value);

        private static readonly Regex rx = new(@"^(\D+): (\d+)-(\d+) or (\d+)-(\d+)$");
        public static Rule Parse(string input)
        {
            var m = rx.Match(input);
            if (!m.Success)
                throw new ApplicationException($"Invalid Range: {input}");

            string name = m.Groups[1].Value;
            int min1 = int.Parse(m.Groups[2].Value);
            int max1 = int.Parse(m.Groups[3].Value);
            int min2 = int.Parse(m.Groups[4].Value);
            int max2 = int.Parse(m.Groups[5].Value);

            return new(name, new(min1, max1), new(min2, max2));
        }

        public static Rule[] ParseMany(IEnumerable<string> input) =>
            input.Select(Parse).ToArray();
    }

    public record Ticket(Rule[] Rules, int[] MyTicket, int[][] ForeignTickets)
    {
        public Dictionary<string, int> ReadMyTicket() =>
            Enumerable.Range(0, Rules.Length)
            .Zip(MatchRulesToIndex())
            .Aggregate(new Dictionary<string, int>(), (d, t) =>
            {
                var (iRule, iValue) = t;
                d[Rules[iRule].Name] = MyTicket[iValue];
                return d;
            });

        public int[] MatchRulesToIndex()
        {
            var validTickets = ForeignTickets.Where(IsAnyRuleValid).ToArray();
            var all = Enumerable.Range(0, validTickets[0].Length);
            var usedIndexes = new HashSet<int>();
            return
                Enumerable.Range(0, Rules.Length)
                .Zip(Rules)
                .Select(rule => (
                    rule.First,
                    validTickets
                    .Select(values =>
                        Enumerable.Range(0, values.Length)
                        .Zip(values)
                        .Where(iv => rule.Second.IsValid(iv.Second))
                        .Select(iv => iv.First))
                    .Aggregate(all, (s1, s2) => s1.Intersect(s2))
                    .ToArray()))
                .OrderBy(t => t.Item2.Length)
                .Select(t =>
                {
                    var (i, idxs) = t;
                    if (idxs.Length > 1)
                        idxs = idxs.Except(usedIndexes).ToArray();

                    if (idxs.Length != 1)
                        throw new ApplicationException("Panic!");

                    var value = idxs.First();
                    usedIndexes.Add(value);
                    return (i, value);
                })
                .OrderBy(t => t.i)
                .Select(t => t.value)
                .ToArray();
        }

        public int[] InvalidForeignValues()
        {
            return
                ForeignTickets
                .Aggregate(Enumerable.Empty<int>(), JoinValues)
                .Where(value => !IsAnyRuleValid(value))
                .ToArray();

            static IEnumerable<int> JoinValues(IEnumerable<int> acc, IEnumerable<int> list) =>
                acc.Concat(list);
        }

        private bool IsAnyRuleValid(int value) =>
            Rules.Any(rule => rule.IsValid(value));

        private bool IsAnyRuleValid(int[] values) =>
            values.All(IsAnyRuleValid);

        public static Ticket Parse(string input)
        {
            string[] chunks = input.Split("\n\n", 3);

            var rules = Rule.ParseMany(chunks[0].Split('\n'));
            var myTicket = ParseTicket(chunks[1].Split('\n')[1]);
            var foreignTickets =
                chunks[2]
                .Split('\n')
                .Skip(1)
                .Select(ParseTicket)
                .ToArray();

            return new(rules, myTicket, foreignTickets);

            static int[] ParseTicket(string input) =>
                input
                .Split(',')
                .Select(int.Parse)
                .ToArray();
        }
    }
}
