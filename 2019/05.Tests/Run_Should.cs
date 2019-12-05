using System;
using System.Collections.Generic;
using Xunit;

namespace Aoc._2019._05.Tests
{
    public class Run_Should
    {
        [Theory]
        [InlineData(new int[] { 1, 0, 0, 0, 99 },
                    new int[] { 2, 0, 0, 0, 99 })]
        [InlineData(new int[] { 2, 3, 0, 3, 99 },
                    new int[] { 2, 3, 0, 6, 99 })]
        [InlineData(new int[] { 2, 4, 4, 5, 99, 0 },
                    new int[] { 2, 4, 4, 5, 99, 9801 })]
        [InlineData(new int[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 },
                    new int[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void PositionMode_Works(int[] code, int[] expected)
        {
            List<int> outputs = new List<int>();
            var result = new Computer(code).Exec(new List<int>(), out outputs);
            Assert.NotEqual(code, result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new int[] { 1002, 4, 3, 4, 33 },
                    new int[] { 1002, 4, 3, 4, 99 })]
        public void ImmediateMode_Works(int[] code, int[] expected)
        {
            List<int> outputs = new List<int>();
            var result = new Computer(code).Exec(new List<int>(), out outputs);
            Assert.NotEqual(code, result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Equal_Works()
        {
            var code = new int[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 };
            Assert.Equal(1, Run.Exec(code, 8));
            Assert.Equal(0, Run.Exec(code, 9));

            code = new int[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 };
            Assert.Equal(1, Run.Exec(code, 8));
            Assert.Equal(0, Run.Exec(code, 9));
        }

        [Fact]
        public void LessThan_Works()
        {
            var code = new int[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 };
            Assert.Equal(0, Run.Exec(code, 8));
            Assert.Equal(1, Run.Exec(code, 7));

            code = new int[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 };
            Assert.Equal(0, Run.Exec(code, 8));
            Assert.Equal(1, Run.Exec(code, 7));
        }

        [Fact]
        public void Jump_Works()
        {
            var code = new int[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 };
            Assert.Equal(0, Run.Exec(code, 0));
            Assert.Equal(1, Run.Exec(code, -1));

            code = new int[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 };
            Assert.Equal(0, Run.Exec(code, 0));
            Assert.Equal(1, Run.Exec(code, -1));
        }

        [Fact]
        public void ComplexCode_Works()
        {
            var code = new int[] {
                3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20,
                31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1,
                46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1,
                46, 98, 99
            };
            Assert.Equal(999, Run.Exec(code, 7));
            Assert.Equal(1000, Run.Exec(code, 8));
            Assert.Equal(1001, Run.Exec(code, 9));
        }

        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("9025675", run.Job1());
            Assert.Equal("11981754", run.Job2());
        }
    }
}
