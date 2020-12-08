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
            var code = Code.Parse(lines);
            while (code.State == Code.Status.Ok)
            {
                var instruction = code.Instructions[code.Current];
                var (type, _) = instruction;
                var newType = MutateType(type);
                TryRunWithNewType(type, newType, instruction);
                code.Next();
            }

            return code;

            static string MutateType(string type) => type switch
            {
                "nop" => "jmp",
                "jmp" => "nop",
                _ => type,
            };

            void TryRunWithNewType(string type, string newType, Instruction instruction)
            {
                if (type == newType)
                {
                    return;
                }

                var snapshot = code.Snapshot;
                instruction.Type = newType;

                code.Run();
                if (code.State != Code.Status.NormalEnd)
                {
                    instruction.Type = type;
                    code.Snapshot = snapshot;
                }
            }
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
        private HashSet<int> Visited = new();

        public (int, int, Status, HashSet<int>) Snapshot
        {
            get => (Current, Accumulator, State, new(Visited));
            set => (Current, Accumulator, State, Visited) = value;
        }

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
