using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day14 : IDay
    {
        public override string Day { get; } = nameof(Day14)[3..];

        public override string Result1() =>
            new Computer().Run(InputLines).MemSum.ToString();

        public override string Result2() =>
            new Computer().Run2(InputLines).MemSum.ToString();
    }

    public record Computer()
    {
        public ulong Mask { get; private set; } = 0UL;
        public ulong Overwrite { get; private set; } = 0UL;
        public List<ulong> FloatingBits { get; private set; } = new();
        public Dictionary<ulong, ulong> Mem = new();

        private static readonly Regex rxMask = new(@"^mask = (.+)$");
        private static readonly Regex rxMem = new(@"^mem\[(\d+)\] = (\d+)$");
        public Computer Run(IEnumerable<string> code)
        {
            foreach (var line in code)
            {
                var m = rxMem.Match(line);
                if (m.Success)
                {
                    var address = ulong.Parse(m.Groups[1].Value);
                    var value = ulong.Parse(m.Groups[2].Value);
                    var modifiedValue = ApplyMask(value);
                    Mem[address] = modifiedValue;
                    continue;
                }

                m = rxMask.Match(line);
                if (m.Success)
                {
                    var mask = m.Groups[1].Value;
                    SetMask(mask);
                    continue;
                }

                throw new ApplicationException($"Invalid code: {line}");
            }

            return this;
        }

        public Computer Run2(IEnumerable<string> code)
        {
            foreach (var line in code)
            {
                var m = rxMem.Match(line);
                if (m.Success)
                {
                    var address = ulong.Parse(m.Groups[1].Value);
                    var value = ulong.Parse(m.Groups[2].Value);
                    var modifiedAddresses = ApplyMask2(address);
                    foreach (var adr in modifiedAddresses)
                    {
                        Mem[adr] = value;
                    }
                    continue;
                }

                m = rxMask.Match(line);
                if (m.Success)
                {
                    var mask = m.Groups[1].Value;
                    SetMask(mask);
                    continue;
                }

                throw new ApplicationException($"Invalid code: {line}");
            }

            return this;
        }

        public ulong MemSum => Mem.Values.Aggregate((a, b) => a + b);

        public ulong ApplyMask(ulong value) => (value & Mask) | Overwrite;

        public List<ulong> ApplyMask2(ulong value)
        {
            var val = value | Overwrite;
            if (FloatingBits.Count == 0)
                return new() { val };

            List<ulong> result = new();
            var mask = ~Mask;
            ulong n = IntPow(2ul, (ulong)FloatingBits.Count);
            for (ulong i = 0; i < n; i++)
            {
                var v = val & mask;
                var j = i;
                for (int index = 0; j != 0; ++index, j >>= 1)
                {
                    if ((j & 1ul) != 0ul)
                    {
                        v |= IntPow(2ul, FloatingBits[index]);
                    }
                }
                result.Add(v);
            }
            return result;
        }

        public void SetMask(string maskPattern)
        {
            ulong mask = 0UL;
            ulong overwrite = 0UL;
            List<ulong> floatingBits = new();
            ulong bit = IntPow(2ul, 35ul);
            for (int i = 0; i < maskPattern.Length; i++, bit >>= 1)
            {
                char c = maskPattern[i];
                if (c == 'X')
                {
                    floatingBits.Add(35ul - (ulong)i);
                    mask |= bit;
                }
                else
                    overwrite |= c switch
                    {
                        '0' => 0,
                        '1' => bit,
                        _ => throw new ApplicationException($"Invalid mask: {maskPattern}"),
                    };
            }
            (Mask, Overwrite, FloatingBits) = (mask, overwrite, floatingBits);
        }

        private static ulong IntPow(ulong x, ulong pow)
        {
            ulong ret = 1UL;
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
}
