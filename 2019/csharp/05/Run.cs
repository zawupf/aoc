using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc._2019._05
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var code = ReadCode();
            var result = Exec(code, 1);
            return result.ToString();
        }

        public string Job2()
        {
            var code = ReadCode();
            var result = Exec(code, 5);
            return result.ToString();
        }

        public static long Exec(long[] code, long input)
        {
            var computer = new Computer(code);
            var outputs = new List<long>();
            computer.Exec(new List<long> { input }, out outputs);
            return DiagnosticCode(outputs);

            long DiagnosticCode(in List<long> outputs)
            {
                long errors = outputs.Take(outputs.Count() - 1).Count(value => value != 0);
                if (errors != 0)
                    throw new Exception($"{errors} error(s) in output data");

                long diagnosticCode = outputs.Last();
                return diagnosticCode;
            }
        }

        private long[] ReadCode()
        {
            var code = (
                from number in ReadAllText("05/input1.txt").Split(',')
                select long.Parse(number)
            ).ToArray();
            return code;
        }
    }

    public class Computer
    {
        private long[] code;
        private long position = 0;
        private long relativeBase = 0;
        private List<long> inputs = new List<long> { };
        private List<long> outputs = new List<long> { };

        public bool IsHalted { get; private set; }
        public bool IsPaused { get; private set; }

        public Computer Clone()
        {
            var computer = new Computer(code);
            computer.position = position;
            computer.relativeBase = relativeBase;
            computer.inputs = new List<long>(inputs);
            computer.outputs = new List<long>(outputs);
            computer.IsHalted = IsHalted;
            computer.IsPaused = IsPaused;
            return computer;
        }

        public static long[] Compile(string sourceCode)
        {
            var code = (
                from number in sourceCode.Split(',')
                select long.Parse(number)
            ).ToArray();
            return code;
        }

        public Computer(long[] code, long? noun = null, long? verb = null)
        {
            this.code = code.Clone() as long[];
            this.code[1] = noun ?? code[1];
            this.code[2] = verb ?? code[2];
            Reset();
        }

        public void Reset()
        {
            position = 0;
            relativeBase = 0;
            IsHalted = false;
            IsPaused = false;
        }

        public long[] Exec(in List<long> inputs, out List<long> outputs)
        {
            Reset();
            this.inputs = inputs;
            while (HandleOpcode()) ;
            outputs = this.outputs;
            return code;
        }

        public long[] Continue(long input, List<long> outputs)
        {
            IsHalted = false;
            IsPaused = false;
            this.inputs.Add(input);
            this.outputs = outputs;
            while (HandleOpcode()) ;
            return code;
        }

        public void Patch(long position, long value)
        {
            code[position] = value;
        }

        public enum Opcode
        {
            Add = 1,
            Mul = 2,
            Input = 3,
            Output = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LessThan = 7,
            Equals = 8,
            AdjustRelativeBase = 9,
            Halt = 99,
        }

        public enum Mode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2,
        }

        public Opcode CurrentOpcode() => (Opcode)(code[position] % 100);

        public Mode CurrentMode(long index)
        {
            long modes = code[position] / 100;
            long shift = LongPow(10, index);
            long lowerModes = modes % (shift * 10);
            long mode = lowerModes / shift;

            return mode switch
            {
                0 => Mode.Position,
                1 => Mode.Immediate,
                2 => Mode.Relative,
                _ => throw new Exception($"Invalid mode: {mode}"),
            };

            long LongPow(long x, long _pow)
            {
                ulong pow = (ulong)_pow;
                long ret = 1;
                while (pow != 0)
                {
                    if ((pow & 1) == 1)
                        ret *= x;
                    x *= x;
                    pow >>= 1;
                }
                return ret;
            }
        }

        public long ReadParameter(long index)
        {
            Mode mode = CurrentMode(index);
            var adr = mode switch
            {
                Mode.Position => code[position + index + 1],
                Mode.Immediate => position + index + 1,
                Mode.Relative => relativeBase + code[position + index + 1],
                _ => throw new InvalidModeException(),
            };
            EnsureAddressIsValid(adr);
            return code[adr];
        }

        public void WriteParameter(long index, long value)
        {
            Mode mode = CurrentMode(index);
            var adr = mode switch
            {
                Mode.Position => code[position + index + 1],
                Mode.Relative => relativeBase + code[position + index + 1],
                _ => throw new InvalidModeException(),
            };
            EnsureAddressIsValid(adr);
            code[adr] = value;
        }

        private void EnsureAddressIsValid(long adr)
        {
            if (adr >= code.Length)
                EnlargeMemory(CalculateNewLength());

            long CalculateNewLength()
            {
                long length = code.Length;
                while (adr >= length)
                    length *= 2;
                return length;
            }

            void EnlargeMemory(long length)
            {
                long[] memory = code;
                code = new long[length];

                long i = 0;
                for (; i < memory.Length; ++i)
                    code[i] = memory[i];

                for (; i < length; ++i)
                    code[i] = 0;
            }
        }

        private bool HandleOpcode()
        {
            Opcode opcode = CurrentOpcode();

            bool doContinue = opcode switch
            {
                Opcode.Add => Add(),
                Opcode.Mul => Mul(),
                Opcode.Input => Input(),
                Opcode.Output => Output(),
                Opcode.JumpIfTrue => JumpIfTrue(),
                Opcode.JumpIfFalse => JumpIfFalse(),
                Opcode.LessThan => LessThan(),
                Opcode.Equals => Equals(),
                Opcode.AdjustRelativeBase => AdjustRelativeBase(),
                Opcode.Halt => Halt(),
                _ => throw new InvalidOpcodeException(),
            };
            return doContinue;
        }

        private bool Halt()
        {
            IsHalted = true;
            return false;
        }

        private bool Add()
        {
            WriteParameter(2, ReadParameter(0) + ReadParameter(1));
            position += 4;
            return true;
        }

        private bool Mul()
        {
            WriteParameter(2, ReadParameter(0) * ReadParameter(1));
            position += 4;
            return true;
        }

        private bool Input()
        {
            if (!inputs.Any())
            {
                return Pause();
            }

            WriteParameter(0, inputs.First());
            inputs.RemoveAt(0);
            position += 2;
            return true;
        }

        private bool Pause()
        {
            IsPaused = true;
            return false;
        }

        private bool Output()
        {
            var data = ReadParameter(0);
            outputs.Add(data);
            position += 2;
            return true;
        }

        private bool JumpIfTrue()
        {
            return Jump(ReadParameter(0) != 0);
        }

        private bool JumpIfFalse()
        {
            return Jump(ReadParameter(0) == 0);
        }

        private bool Jump(bool jump)
        {
            if (jump)
            {
                position = ReadParameter(1);
            }
            else
            {
                position += 3;
            }
            return true;
        }

        private bool LessThan()
        {
            WriteParameter(2, ReadParameter(0) < ReadParameter(1) ? 1 : 0);
            position += 4;
            return true;
        }

        private bool Equals()
        {
            WriteParameter(2, ReadParameter(0) == ReadParameter(1) ? 1 : 0);
            position += 4;
            return true;
        }

        private bool AdjustRelativeBase()
        {
            relativeBase += ReadParameter(0);
            position += 2;
            return true;
        }
    }

    public class InvalidModeException : Exception { }
    public class InvalidOpcodeException : Exception { }
}
