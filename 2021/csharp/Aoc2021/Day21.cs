namespace Aoc2021;

public class Day21 : IDay
{
    public override string Day { get; } = nameof(Day21)[3..];

    public override string Result1()
    {
        return WarmupGame.Init(InputLines)
            .Play()
            .Last()
            .Result1
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return "xxx";
    }

    public record Player(int Space, int Score = 0);

    public record WarmupGame(Player CurrentPlayer, Player OtherPlayer)
    {
        public DeterministicDie Die { get; } = new();

        public int Result1 => OtherPlayer.Score * Die.RollCount;

        public IEnumerable<WarmupGame> Play()
        {
            WarmupGame game = this;
            while (true)
            {
                int space = ((game.CurrentPlayer.Space + game.Die.Roll(3) - 1) % 10) + 1;
                int score = game.CurrentPlayer.Score + space;
                Player current = game.CurrentPlayer with
                {
                    Space = space,
                    Score = score,
                };

                yield return game = game with { CurrentPlayer = current };

                if (game.CurrentPlayer.Score >= 1000)
                {
                    break;
                }

                game = game with
                {
                    CurrentPlayer = game.OtherPlayer,
                    OtherPlayer = game.CurrentPlayer,
                };
            }
        }

        public static WarmupGame Init(IEnumerable<string> lines)
        {
            int space1 = ParseInt(lines.ElementAt(0)[28..]);
            int space2 = ParseInt(lines.ElementAt(1)[28..]);
            return new(new(space1), new(space2));
        }
    }

    public class DeterministicDie
    {
        private int value;

        public int RollCount { get; private set; }

        public int Roll()
        {
            ++RollCount;
            return value = value == 100 ? 1 : (value + 1);
        }

        public int Roll(int count)
        {
            return Enumerable.Range(0, count).Select(_ => Roll()).Sum();
        }
    }
}
