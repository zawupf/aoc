using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Aoc._2019._05.Tests
{
    public class Run_Should
    {
        [Theory]
        [InlineData(new long[] { 1, 0, 0, 0, 99 },
                    new long[] { 2, 0, 0, 0, 99 })]
        [InlineData(new long[] { 2, 3, 0, 3, 99 },
                    new long[] { 2, 3, 0, 6, 99 })]
        [InlineData(new long[] { 2, 4, 4, 5, 99, 0 },
                    new long[] { 2, 4, 4, 5, 99, 9801 })]
        [InlineData(new long[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 },
                    new long[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void PositionMode_Works(long[] code, long[] expected)
        {
            List<long> outputs;
            var result = new Computer(code).Exec(new List<long>(), out outputs);
            Assert.NotEqual(code, result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new long[] { 1002, 4, 3, 4, 33 },
                    new long[] { 1002, 4, 3, 4, 99 })]
        public void ImmediateMode_Works(long[] code, long[] expected)
        {
            List<long> outputs;
            var result = new Computer(code).Exec(new List<long>(), out outputs);
            Assert.NotEqual(code, result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Equal_Works()
        {
            var code = new long[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 };
            Assert.Equal(1, Run.Exec(code, 8));
            Assert.Equal(0, Run.Exec(code, 9));

            code = new long[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 };
            Assert.Equal(1, Run.Exec(code, 8));
            Assert.Equal(0, Run.Exec(code, 9));
        }

        [Fact]
        public void LessThan_Works()
        {
            var code = new long[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 };
            Assert.Equal(0, Run.Exec(code, 8));
            Assert.Equal(1, Run.Exec(code, 7));

            code = new long[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 };
            Assert.Equal(0, Run.Exec(code, 8));
            Assert.Equal(1, Run.Exec(code, 7));
        }

        [Fact]
        public void Jump_Works()
        {
            var code = new long[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 };
            Assert.Equal(0, Run.Exec(code, 0));
            Assert.Equal(1, Run.Exec(code, -1));

            code = new long[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 };
            Assert.Equal(0, Run.Exec(code, 0));
            Assert.Equal(1, Run.Exec(code, -1));
        }

        [Fact]
        public void ComplexCode_Works()
        {
            var code = new long[] {
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
        public void Relative_Mode_Works()
        {
            var code = new long[] {
                 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006,
                 101, 0, 99
            };

            var computer = new Computer(code);
            var outputs = new List<long>();
            computer.Exec(new List<long>(), out outputs);
            Assert.Equal(code, outputs.ToArray());
        }

        [Fact]
        public void Large_Numbers_Works()
        {
            var code = new long[] { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
            var computer = new Computer(code);
            var outputs = new List<long>();
            computer.Exec(new List<long>(), out outputs);
            Assert.Equal(16, (from o in outputs select o).Last().ToString().Length);

            code = new long[] { 104, 1125899906842624, 99 };
            computer = new Computer(code);
            outputs = new List<long>();
            computer.Exec(new List<long>(), out outputs);
            Assert.Equal(1125899906842624, (from o in outputs select o).Last());
        }

        [Fact]
        public void Stars()
        {
            var run = new Run();
            Assert.Equal("9025675", run.Job1());
            Assert.Equal("11981754", run.Job2());
        }
    }
}
