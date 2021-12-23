namespace Aoc2021;

public class Day21 : IDay
{
    public override string Day { get; } = nameof(Day21)[3..];

    public override string Result1()
    {
        return WarmupGame
            .Init(InputLines)
            .Play()
            .Last()
            .Result1
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return DiracGame
            .Init(InputLines)
            .Play()
            .Last()
            .MasterOfTheMultiverseCount
            .ToString(CultureInfo.InvariantCulture);
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

    public record DiracGame
    {
        public long MasterOfTheMultiverseCount => Math.Max(WinsCount.player1, WinsCount.player2);

        private Dictionary<(Player player1, Player player2), long> Multiverse { get; init; }

        private (long player1, long player2) WinsCount { get; init; } = (0L, 0L);

        private bool IsPlayer1Active { get; init; } = true;

        private DiracGame()
        {
            Multiverse = new();
        }

        public DiracGame(Player player1, Player player2)
        {
            Multiverse = new()
            {
                [(player1, player2)] = 1,
            };
        }

        public IEnumerable<DiracGame> Play()
        {
            return play(this);

            static IEnumerable<DiracGame> play(DiracGame game)
            {
                while (game.Multiverse.Count != 0)
                {
                    game = game.Multiverse
                        .Aggregate(
                            game with { Multiverse = new() },
                            (game, universe) =>
                                MovesCount.Aggregate(game, (game, movesCount) =>
                                    rollDiracDice(game, movesCount, universe)))
                    with
                    { IsPlayer1Active = !game.IsPlayer1Active };

                    yield return game;
                }

                DiracGame rollDiracDice(
                    DiracGame game,
                    KeyValuePair<int, int> rolls,
                    KeyValuePair<(Player player1, Player player2), long> universe)
                {
                    ((Player, Player) players, long gameCount) = universe;
                    Player currentPlayer = game.IsPlayer1Active ? players.Item1 : players.Item2;

                    (int movesCount, int movesFactor) = rolls;
                    long newGameCount = gameCount * movesFactor;

                    int space = ((currentPlayer.Space + movesCount - 1) % 10) + 1;
                    int score = currentPlayer.Score + space;
                    bool isFinished = score >= 21;

                    (long, long) winsCount = isFinished
                        ? incrementActiveWinsCountBy(newGameCount, game.WinsCount)
                        : game.WinsCount;

                    Dictionary<(Player, Player), long> multiverse = game.Multiverse;
                    if (!isFinished)
                    {
                        (Player, Player) newPlayers = makeNewPlayersWithActive(new(space, score), players);
                        incrementGameCount(multiverse, newPlayers, newGameCount);
                    }

                    return game with { WinsCount = winsCount };
                }

                (long, long) incrementActiveWinsCountBy(long value, (long, long) winsCount)
                {
                    return game.IsPlayer1Active
                        ? (winsCount.Item1 + value, winsCount.Item2)
                        : (winsCount.Item1, winsCount.Item2 + value);
                }

                void incrementGameCount(Dictionary<(Player, Player), long> multiverse, (Player, Player) players, long count)
                {
                    if (!multiverse.TryAdd(players, count))
                    {
                        multiverse[players] += count;
                    }
                }

                (Player, Player) makeNewPlayersWithActive(Player newPlayer, (Player, Player) players)
                {
                    return game.IsPlayer1Active
                        ? (newPlayer, players.Item2)
                        : (players.Item1, newPlayer);
                }
            }
        }


        private static readonly Dictionary<int, int> MovesCount = new()
        {
            [3] = 1,
            [4] = 3,
            [5] = 6,
            [6] = 7,
            [7] = 6,
            [8] = 3,
            [9] = 1,
        };

        public static DiracGame Init(IEnumerable<string> lines)
        {
            int space1 = ParseInt(lines.ElementAt(0)[28..]);
            int space2 = ParseInt(lines.ElementAt(1)[28..]);
            return new(new(space1), new(space2));
        }
    }
}
