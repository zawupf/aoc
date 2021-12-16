namespace Aoc2021;

public class Day16 : IDay
{
    public override string Day { get; } = nameof(Day16)[3..];

    public override string Result1()
    {
        return VersionSum(InputText)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return Value(InputText)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static ulong VersionSum(string packet)
    {
        return BITS.Load(packet).Decode().VersionSum;
    }

    public static ulong Value(string packet)
    {
        return BITS.Load(packet).Decode().Value;
    }

    public class BITS
    {
        private readonly uint[] data;
        private int offset;

        public ulong VersionSum { get; private set; }

        public ulong Value { get; private set; }

        public BITS(string packet)
        {
            data = packet
                .Chunk(8)
                .Select(chunk => string.Join("", chunk))
                .Select(ToUInt32)
                .ToArray();

            static uint ToUInt32(string s)
            {
                if (s.Length % 8 != 0)
                {
                    s += new string('0', 8 - (s.Length % 8));
                }
                return Convert.ToUInt32(s, 16);
            }
        }

        public static BITS Load(string packet)
        {
            return new BITS(packet);
        }

        public BITS Decode()
        {
            Value = ReadPacket();
            return this;
        }

        private ulong ReadPacket()
        {
            _ = ReadVersion();
            ulong typeId = ReadTypeId();
            return typeId switch
            {
                4UL => ReadLiteral(),
                _ => ReadOperator(typeId),
            };
        }

        private ulong ReadVersion()
        {
            ulong version = ReadBits(3);
            VersionSum += version;
            return version;
        }

        private ulong ReadTypeId()
        {
            return ReadBits(3);
        }

        private ulong ReadLiteral()
        {
            List<ulong> nibbles = new();
            bool isLast = false;
            while (!isLast)
            {
                isLast = ReadBit() == 0UL;
                nibbles.Add(ReadBits(4));
            }

            if (nibbles.Count > 16)
            {
                throw new ArgumentOutOfRangeException("Too many nibbles");
            }

            return Enumerable.Range(0, nibbles.Count)
                .Zip((nibbles as IEnumerable<ulong>).Reverse())
                .Aggregate(0UL, (literal, indexNibble) =>
                {
                    (int index, ulong nibble) = indexNibble;
                    literal |= nibble << (4 * index);
                    return literal;
                });
        }

        private ulong ReadOperator(ulong typeId)
        {
            ulong lengthTypeId = ReadBit();
            List<ulong> values = lengthTypeId switch
            {
                0UL => ReadOperatorBits().ToList(),
                _ => ReadOperatorPackets().ToList(),
            };

            return typeId switch
            {
                0 => sum(values),
                1 => product(values),
                2 => minimum(values),
                3 => maximum(values),
                5 => greaterThan(values),
                6 => lessThan(values),
                7 => equal(values),
                _ => throw new ArgumentException("Invalid type id", nameof(typeId)),
            };

            static ulong sum(IEnumerable<ulong> values)
            {
                return values.Aggregate(0UL, (acc, value) => acc += value);
            }

            static ulong product(IEnumerable<ulong> values)
            {
                return values.Aggregate(1UL, (acc, value) => acc *= value);
            }

            static ulong minimum(IEnumerable<ulong> values)
            {
                return values.Min();
            }

            static ulong maximum(IEnumerable<ulong> values)
            {
                return values.Max();
            }

            static ulong greaterThan(IEnumerable<ulong> values)
            {
                ulong first = values.First();
                ulong second = values.Skip(1).First();
                return first > second ? 1UL : 0UL;
            }

            static ulong lessThan(IEnumerable<ulong> values)
            {
                ulong first = values.First();
                ulong second = values.Skip(1).First();
                return first < second ? 1UL : 0UL;
            }

            static ulong equal(IEnumerable<ulong> values)
            {
                ulong first = values.First();
                ulong second = values.Skip(1).First();
                return first == second ? 1UL : 0UL;
            }
        }

        private IEnumerable<ulong> ReadOperatorBits()
        {
            ulong bitCount = ReadBits(15);
            int targetOffset = offset + (int)bitCount;
            while (offset != targetOffset)
            {
                yield return ReadPacket();
            }
        }

        private IEnumerable<ulong> ReadOperatorPackets()
        {
            ulong packetCount = ReadBits(11);
            while (packetCount-- != 0)
            {
                yield return ReadPacket();
            }
        }

        private ulong ReadBits(ulong count)
        {
            if (count > 64UL)
            {
                throw new ArgumentException("Bit count overflow", nameof(count));
            }

            ulong result = 0UL;
            while (count-- != 0UL)
            {
                result |= ReadBit() << (int)count;
            }

            return result;
        }

        private ulong ReadBit()
        {
            ulong result = PeekBitAt(0);
            offset += 1;
            return result;
        }

        private ulong PeekBitAt(int offset)
        {
            int adr = this.offset + offset;
            int index = adr / 32;
            ulong mask = 1u << (31 - (adr % 32));

            return (data[index] & mask) != 0 ? 1UL : 0UL;
        }
    }
}
