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

        public static int Exec(int[] code, int input)
        {
            var computer = new Computer(code);
            var outputs = new List<int>();
            computer.Exec(new List<int> { input }, out outputs);
            return DiagnosticCode(outputs);

            int DiagnosticCode(in List<int> outputs)
            {
                int errors = outputs.Take(outputs.Count() - 1).Count(value => value != 0);
                if (errors != 0)
                    throw new Exception($"{errors} error(s) in output data");

                int diagnosticCode = outputs.Last();
                return diagnosticCode;
            }
        }

        private int[] ReadCode()
        {
            var code = (
                from number in ReadAllText("05/input1.txt").Split(',')
                select int.Parse(number)
            ).ToArray();
            return code;
        }
    }

    public class Computer
    {
        private int[] code;
        private int position = 0;
        private List<int> inputs = new List<int> { };
        private List<int> outputs = new List<int> { };

        public Computer(int[] code, int? noun = null, int? verb = null)
        {
            this.code = code.Clone() as int[];
            this.code[1] = noun ?? code[1];
            this.code[2] = verb ?? code[2];
        }

        public int[] Exec(in List<int> inputs, out List<int> outputs)
        {
            this.inputs = inputs;
            position = 0;
            while (HandleOpcode()) ;
            outputs = this.outputs;
            return code;
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
            Halt = 99,
        }

        public enum Mode
        {
            Position = 0,
            Immediate = 1,
        }

        public Opcode CurrentOpcode() => (Opcode)(code[position] % 100);

        public Mode CurrentMode(uint index)
        {
            int modes = code[position] / 100;
            int shift = IntPow(10, index);
            int lowerModes = modes % (shift * 10);
            int mode = lowerModes / shift;

            return mode switch
            {
                0 => Mode.Position,
                _ => Mode.Immediate,
            };

            int IntPow(int x, uint pow)
            {
                int ret = 1;
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

        public int ReadParameter(uint index)
        {
            Mode mode = CurrentMode(index);
            return mode switch
            {
                Mode.Position => code[code[position + index + 1]],
                Mode.Immediate => code[position + index + 1],
                _ => throw new InvalidModeException(),
            };
        }

        public void WriteParameter(uint index, int value)
        {
            Mode mode = CurrentMode(index);
            if (mode != Mode.Position)
                throw new InvalidModeException();

            code[code[position + index + 1]] = value;
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
                Opcode.Halt => false,
                _ => throw new InvalidOpcodeException(),
            };
            return doContinue;
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
            WriteParameter(0, inputs.First());
            inputs.RemoveAt(0);
            position += 2;
            return true;
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
    }

    public class InvalidModeException : Exception { }
    public class InvalidOpcodeException : Exception { }
}
