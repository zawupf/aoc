using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day04 : IDay
    {
        public override string Day { get; } = nameof(Day04)[3..];

        public override string Result1() => Passport.ParseMany(InputText)
            .Count(passport => passport.AreRequiredFieldsPresent())
            .ToString();

        public override string Result2() => Passport.ParseMany(InputText)
            .Count(passport => passport.IsValid())
            .ToString();
    }

    public class Passport
    {
        public enum Field
        {
            byr, iyr, eyr, hgt, hcl, ecl, pid, cid,
        }

        private readonly Dictionary<Field, string> data = new();

        public Field[] RequiredFields = {
            Field.byr, Field.iyr, Field.eyr, Field.hgt,
            Field.hcl, Field.ecl, Field.pid
        };

        public bool AreRequiredFieldsPresent() =>
            RequiredFields.All(field => data.ContainsKey(field));

        public bool IsValid() => AreRequiredFieldsPresent()
            && data.All(kv => IsValid(kv.Key, kv.Value));

        private static bool IsValid(Field key, string value)
        {
            try
            {
                return key switch
                {
                    Field.byr => IsByrValid(),
                    Field.iyr => IsIyrValid(),
                    Field.eyr => IsEyrValid(),
                    Field.hgt => IsHgtValid(),
                    Field.hcl => IsHclValid(),
                    Field.ecl => IsEclValid(),
                    Field.pid => IsPidValid(),
                    Field.cid => true,
                    _ => false,
                };
            }
            catch (Exception)
            {
                return false;
            }

            bool IsByrValid() => int.Parse(value) is >= 1920 and <= 2002;
            bool IsIyrValid() => int.Parse(value) is >= 2010 and <= 2020;
            bool IsEyrValid() => int.Parse(value) is >= 2020 and <= 2030;
            bool IsHgtValid()
            {
                Regex rx = new(@"^(\d+)(cm|in)$");
                var matches = rx.Match(value);
                int height = int.Parse(matches.Groups[1].Value);
                string unit = matches.Groups[2].Value;
                return unit switch
                {
                    "cm" => height is >= 150 and <= 193,
                    "in" => height is >= 59 and <= 76,
                    _ => false,
                };
            };
            bool IsHclValid()
            {
                Regex rx = new(@"^#[0-9a-f]{6}$");
                var matches = rx.Match(value);
                return matches.Success;
            };
            bool IsEclValid() => value is "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth";
            bool IsPidValid()
            {
                Regex rx = new(@"^\d{9}$");
                var matches = rx.Match(value);
                return matches.Success;
            };
        }

        public static Passport Parse(string data) => data
            .Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(new Passport(), (passport, fieldData) =>
            {
                var keyValue = fieldData.Split(':', 2);
                if (Enum.TryParse(keyValue[0], out Field field))
                {
                    passport.data[field] = keyValue[1];
                }
                return passport;
            });

        public static IEnumerable<Passport> ParseMany(string data) => data
            .Split("\n\n")
            .Select(data => Parse(data));
    }
}
