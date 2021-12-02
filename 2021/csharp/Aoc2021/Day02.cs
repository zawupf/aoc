namespace Aoc2021;

public class Day02 : IDay
{
    public override string Day { get; } = nameof(Day02)[3..];

    public override string Result1()
    {
        List<Command> commands = ParseCommands(InputLines);
        Submarine submarine = MoveFirstTry(new(0, 0, 0), commands);
        int result = submarine.HorizontalPosition * submarine.Depth;
        return result.ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        List<Command> commands = ParseCommands(InputLines);
        Submarine submarine = MoveSecondTry(new(0, 0, 0), commands);
        int result = submarine.HorizontalPosition * submarine.Depth;
        return result.ToString(CultureInfo.InvariantCulture);
    }

    public record Submarine(int HorizontalPosition, int Depth, int Aim);
    public record Command(string Direction, int Value);

    public static List<Command> ParseCommands(IEnumerable<string> input)
    {
        return input.Select((line) =>
        {
            string[] chunks = line.Split(' ', 2);
            return new Command(chunks[0], ParseInt(chunks[1]));
        }).ToList();
    }

    public static Submarine MoveFirstTry(Submarine submarine, IEnumerable<Command> commands)
    {
        return commands
            .Aggregate(submarine, (submarine, command) =>
            {
                (int horizontalPosition, int depth, _) = submarine;
                (string direction, int value) = command;
                return direction switch
                {
                    "forward" => submarine with { HorizontalPosition = horizontalPosition + value },
                    "down" => submarine with { Depth = depth + value },
                    "up" => submarine with { Depth = depth - value },
                    _ => throw new ArgumentException("Invalid command direction"),
                };
            });
    }

    public static Submarine MoveSecondTry(Submarine submarine, IEnumerable<Command> commands)
    {
        return commands
            .Aggregate(submarine, (submarine, command) =>
            {
                (int horizontalPosition, int depth, int aim) = submarine;
                (string direction, int value) = command;
                return direction switch
                {
                    "forward" => submarine with
                    {
                        HorizontalPosition = horizontalPosition + value,
                        Depth = depth + (aim * value),
                    },
                    "down" => submarine with { Aim = aim + value },
                    "up" => submarine with { Aim = aim - value },
                    _ => throw new ArgumentException("Invalid command direction"),
                };
            });
    }
}
