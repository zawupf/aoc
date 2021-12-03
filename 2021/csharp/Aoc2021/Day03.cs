using System.Text;

namespace Aoc2021;

public class Day03 : IDay
{
    public override string Day { get; } = nameof(Day03)[3..];

    public override string Result1()
    {
        return PowerConsumtionRate
            .FromReport(InputLines.ToList())
            .Result
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return LifeSupportRate
            .FromReport(InputLines.ToList())
            .Result
            .ToString(CultureInfo.InvariantCulture);
    }

    public record PowerConsumtionRate(int Gamma, int Epsilon)
    {
        public int Result => Gamma * Epsilon;

        public static PowerConsumtionRate FromReport(List<string> lines)
        {
            int numChars = lines[0].Length;

            StringBuilder gammaBuilder = new();
            for (int c = 0; c != numChars; c++)
            {
                _ = gammaBuilder.Append(MostCharAt(c, lines));
            }

            int gamma = BitstringToInt(gammaBuilder.ToString());
            int epsilon = (~gamma) & ((1 << numChars) - 1);
            return new(gamma, epsilon);
        }
    }

    public record LifeSupportRate(int OxygenGenerator, int CO2Scrubber)
    {
        public int Result => OxygenGenerator * CO2Scrubber;

        public static LifeSupportRate FromReport(List<string> lines)
        {
            string oxygenGeneratorString = filter(lines, MostCharAt);
            string co2ScrubberString = filter(lines, LeastCharAt);

            string filter(List<string> lines, Func<int, List<string>, char> fun)
            {
                for (int c = 0; lines.Count > 1; c++)
                {
                    char useChar = fun(c, lines);
                    lines = lines.Where(line => line[c] == useChar).ToList();
                }

                return lines[0];
            }

            int oxygenGenerator = BitstringToInt(oxygenGeneratorString);
            int co2Scrubber = BitstringToInt(co2ScrubberString);
            return new(oxygenGenerator, co2Scrubber);
        }
    }

    private static char MostCharAt(int index, List<string> lines)
    {
        return Math.Sign(lines.Sum(line => line[index] == '1' ? 1 : -1)) switch
        {
            1 => '1',
            -1 => '0',
            _ => '1',
        };
    }

    private static char LeastCharAt(int index, List<string> lines)
    {
        return Math.Sign(lines.Sum(line => line[index] == '1' ? 1 : -1)) switch
        {
            1 => '0',
            -1 => '1',
            _ => '0',
        };
    }

    private static int BitstringToInt(string bits)
    {
        return Convert.ToInt32(bits, 2);
    }
}
