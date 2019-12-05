using System.Collections.Generic;
using System;
using Xunit;
using System.Linq;

namespace Aoc._2019._03.Tests
{
    public class Run_Should
    {
        public static IEnumerable<object[]> Parse_Valid_Move_Works_Data()
        {
            yield return new object[] { "U1", new Move(Direction.Up, 1) };
            yield return new object[] { "D12", new Move(Direction.Down, 12) };
            yield return new object[] { "L123", new Move(Direction.Left, 123) };
            yield return new object[] { "R1234", new Move(Direction.Right, 1234) };
        }

        [Theory]
        [MemberData(nameof(Parse_Valid_Move_Works_Data))]
        public void Parse_Valid_Move_Works(string text, Move move)
        {
            Assert.Equal(move, Move.Parse(text));
        }

        [Theory]
        [InlineData(new string[] {
            "R75,D30,R83,U83,L12,D49,R71,U7,L72",
            "U62,R66,U55,R34,D71,R55,D58,R83" }, 159)]
        [InlineData(new string[] {
            "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
            "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" }, 135)]
        public void Distance_Works(string[] lines, int distance)
        {
            Assert.Equal(distance, Run.Distance(lines));
        }

        [Theory]
        [InlineData(new string[] {
            "R75,D30,R83,U83,L12,D49,R71,U7,L72",
            "U62,R66,U55,R34,D71,R55,D58,R83" }, 610)]
        [InlineData(new string[] {
            "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
            "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" }, 410)]
        public void Steps_Works(string[] lines, int steps)
        {
            Assert.Equal(steps, Run.Steps(lines));
        }

        [Fact]
        public void Stars()
        {
            var run = new Run("../../../../");
            Assert.Equal("8015", run.Job1());
            Assert.Equal("163676", run.Job2());
        }
    }
}
