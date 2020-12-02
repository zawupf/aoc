using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day02 : IDay
    {
        public override string Day { get; } = nameof(Day02)[3..];

        public override string Result1() => InputLines
            .Select(line => PasswordPolicy.Parse(line))
            .Count(p => p.IsValidOldPassword())
            .ToString();

        public override string Result2() => InputLines
            .Select(line => PasswordPolicy.Parse(line))
            .Count(p => p.IsValidNewPassword())
            .ToString();
    }

    public record PasswordPolicy(string Password, Policy Policy)
    {
        public bool IsValidOldPassword()
        {
            int count = Password.Count(c => c == Policy.Letter);
            return Policy.Min <= count && count <= Policy.Max;
        }

        public bool IsValidNewPassword()
        {
            var (c1, c2) = (Password[Policy.Min - 1], Password[Policy.Max - 1]);
            return (c1 == Policy.Letter || c2 == Policy.Letter) && c1 != c2;
        }

        private static readonly Regex rx = new Regex(@"^(\d+)-(\d+) (.): (.+)$");

        static public PasswordPolicy Parse(string line)
        {
            var matches = rx.Match(line);
            int min = int.Parse(matches.Groups[1].Value);
            int max = int.Parse(matches.Groups[2].Value);
            char letter = matches.Groups[3].Value[0];
            string password = matches.Groups[4].Value;
            Policy policy = new(letter, min, max);
            return new PasswordPolicy(password, policy);
        }
    }

    public record Policy(char Letter, int Min, int Max);
}
