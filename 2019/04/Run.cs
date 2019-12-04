using System;
using System.Linq;

namespace Aoc._2019._04
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var query =
                from number in Enumerable.Range(128392, 643281 - 128392 + 1)
                where IsValidAdjacentDigits(number) && IsValidIncreasingDigits(number)
                select number;
            return query.Count().ToString();
        }

        public string Job2()
        {
            var query =
                from number in Enumerable.Range(128392, 643281 - 128392 + 1)
                where IsValidAdjacentDigits2(number) && IsValidIncreasingDigits(number)
                select number;
            return query.Count().ToString();
        }

        public static bool IsValidAdjacentDigits(int number)
        {
            var chars = number.ToString();
            var string1 = chars[..^1];
            var string2 = chars[1..];
            for (int i = 0; i < string1.Length; ++i)
            {
                if (string1[i] == string2[i])
                    return true;
            }
            return false;
        }

        public static bool IsValidAdjacentDigits2(int number)
        {
            var chars = number.ToString();
            for (int i = 0; i < chars.Length - 1; ++i)
            {
                char c = chars[i];
                int j = i + 1;
                while (j < chars.Length && chars[j] == c) ++j;
                if (j - i == 2)
                    return true;
                i = j - 1;
            }
            return false;
        }

        public static bool IsValidIncreasingDigits(int number)
        {
            var chars = number.ToString();
            var string1 = chars[..^1];
            var string2 = chars[1..];
            for (int i = 0; i < string1.Length; ++i)
            {
                if (string1[i] > string2[i])
                    return false;
            }
            return true;
        }
    }
}
