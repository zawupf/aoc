using System.Collections.Generic;
using Xunit;

namespace Aoc2020.Tests
{
    public class Day02Tests
    {
        private static PasswordPolicy Create(string pw, char l, int min, int max) =>
            new(pw, new(l, min, max));

        public static IEnumerable<object[]> LineParsingWorks_Data()
        {
            yield return new object[] { Create("abcde", 'a', 1, 3), "1-3 a: abcde" };
            yield return new object[] { Create("cdefg", 'b', 1, 3), "1-3 b: cdefg" };
            yield return new object[] { Create("ccccccccc", 'c', 2, 9), "2-9 c: ccccccccc" };
        }

        [Theory]
        [MemberData(nameof(LineParsingWorks_Data))]
        public void LineParsingWorks(PasswordPolicy passwordPolicy, string inputLine)
        {
            Assert.Equal(passwordPolicy, PasswordPolicy.Parse(inputLine));
        }

        [Fact]
        public void OldPasswordValidationWorks()
        {
            Assert.True(PasswordPolicy.Parse("1-3 a: abcde").IsValidOldPassword());
            Assert.False(PasswordPolicy.Parse("1-3 b: cdefg").IsValidOldPassword());
            Assert.True(PasswordPolicy.Parse("2-9 c: ccccccccc").IsValidOldPassword());
        }

        [Fact]
        public void NewPasswordValidationWorks()
        {
            Assert.True(PasswordPolicy.Parse("1-3 a: abcde").IsValidNewPassword());
            Assert.False(PasswordPolicy.Parse("1-3 b: cdefg").IsValidNewPassword());
            Assert.False(PasswordPolicy.Parse("2-9 c: ccccccccc").IsValidNewPassword());
        }

        [Fact]
        public void Stars()
        {
            var run = new Day02();
            Assert.Equal("645", run.Result1());
            Assert.Equal("737", run.Result2());
        }
    }
}
