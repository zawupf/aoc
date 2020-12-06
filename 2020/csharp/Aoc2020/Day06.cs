using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day06 : IDay
    {
        public override string Day { get; } = nameof(Day06)[3..];

        public override string Result1() => CountAnswers(AnyAnswer).ToString();

        public override string Result2() => CountAnswers(CommonAnswer).ToString();

        public int CountAnswers(Func<IEnumerable<HashSet<char>>, HashSet<char>> combinator) =>
            InputText
            .Split("\n\n")
            .Select(ParseGroup)
            .Select(combinator)
            .Select(answers => answers.Count)
            .Sum();

        public static HashSet<char>[] ParseGroup(string groupData) =>
            groupData.Split('\n')
            .Select(line => line.ToHashSet())
            .ToArray();

        public static HashSet<char> AnyAnswer(IEnumerable<HashSet<char>> answers) =>
            answers
            .Aggregate((result, answer) =>
            {
                result.UnionWith(answer);
                return result;
            });

        public static HashSet<char> CommonAnswer(IEnumerable<HashSet<char>> answers) =>
            answers
            .Aggregate((result, answer) =>
            {
                result.IntersectWith(answer);
                return result;
            });
    }
}
