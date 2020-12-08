using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day08 : IDay
    {
        public override string Day { get; } = nameof(Day08)[3..];

        public override string Result1() =>
            Code
            .Parse(InputLines)
            .Run()
            .Accumulator
            .ToString();

        public override string Result2() =>
            MutateCode(InputLines)
            .Accumulator
            .ToString();

        public static Code MutateCode(IEnumerable<string> lines)
        {
            Instruction[] instructions = Instruction.ParseMany(lines);
            foreach (var instruction in instructions)
            {
                var (type, _) = instruction;

                if (type == "nop")
                {
                    instruction.Type = "jmp";
                }
                else if (type == "jmp")
                {
                    instruction.Type = "nop";
                }
                else
                {
                    continue;
                }

                Code code = new(instructions);
                code.Run();
                if (code.State == Code.Status.NormalEnd)
                {
                    return code;
                }

                instruction.Type = type;
            }

            return null;
        }
    }

    public record Instruction(string Type, int Arg)
    {
        public string Type { get; set; } = Type;

        public static Instruction Parse(string line) =>
            new(line[..3], int.Parse(line[4..]));

        public static Instruction[] ParseMany(IEnumerable<string> lines) =>
            lines.Select(Parse).ToArray();
    }

    public record Code(Instruction[] Instructions)
    {
        public int Current { get; set; } = 0;
        public int Accumulator { get; set; } = 0;
        public Status State { get; set; } = Status.Ok;
        private readonly HashSet<int> Visited = new();

        public static Code Parse(IEnumerable<string> lines) =>
            new(Instruction.ParseMany(lines));

        public enum Status
        {
            Ok,
            InfiniteLoop,
            NormalEnd,
        }

        public Code Run()
        {
            while (State == Status.Ok)
            {
                Next();
            }
            return this;
        }

        public Code Next()
        {
            if (TestState() == Status.Ok)
            {
                var (type, arg) = Instructions[Current];
                switch (type)
                {
                    case "nop":
                        ++Current;
                        break;
                    case "acc":
                        Accumulator += arg;
                        ++Current;
                        break;
                    case "jmp":
                        Current += arg;
                        break;
                    default:
                        throw new ArgumentException($"Invalid instruction '{type}'");
                }
            }
            return this;
        }

        private Status TestState()
        {
            if (Current < 0 || Current >= Instructions.Length)
            {
                State = Status.NormalEnd;
            }
            else if (!Visited.Add(Current))
            {
                State = Status.InfiniteLoop;
            }
            return State;
        }
    }
}
