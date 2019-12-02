﻿using System;
using System.Linq;

namespace Aoc._2019._02
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var code = ReadCode();
            var result = Exec(code, 12, 2);
            return result.ToString();
        }

        public string Job2()
        {
            var code = ReadCode();
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    var result = Exec(code, noun, verb);
                    if (result == 19690720)
                    {
                        return (100 * noun + verb).ToString();
                    }
                }
            }
            return "Panic!";
        }

        private int Exec(int[] code, int? noun = null, int? verb = null)
        {
            var computer = new Computer(code, noun, verb);
            var result = computer.Exec();
            return result[0];
        }

        private int[] ReadCode()
        {
            var code = (
                from number in ReadAllText("02/input1.txt").Split(',')
                select int.Parse(number)
            ).ToArray();
            return code;
        }
    }

    public class Computer
    {
        private int[] code;
        private int position = 0;

        public Computer(int[] code, int? noun = null, int? verb = null)
        {
            this.code = code.Clone() as int[];
            this.code[1] = noun != null ? (int)noun : code[1];
            this.code[2] = verb != null ? (int)verb : code[2];
        }

        public int[] Exec()
        {
            position = 0;
            while (HandleOpcode()) ;
            return code;
        }

        private bool HandleOpcode()
        {
            int opcode = code[position];
            bool doContinue = opcode switch
            {
                1 => Add(),
                2 => Mul(),
                99 => false,
                _ => throw new Exception($"Invalid Opcode {opcode}"),
            };
            return doContinue;
        }

        private bool Add()
        {
            int in1 = code[position + 1];
            int in2 = code[position + 2];
            int res = code[position + 3];
            code[res] = code[in1] + code[in2];
            position += 4;
            return true;
        }

        private bool Mul()
        {
            int in1 = code[position + 1];
            int in2 = code[position + 2];
            int res = code[position + 3];
            code[res] = code[in1] * code[in2];
            position += 4;
            return true;
        }
    }
}
