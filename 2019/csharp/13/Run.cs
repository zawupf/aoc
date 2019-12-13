using System;
using System.Linq;
using System.Collections.Generic;

namespace Aoc._2019._13
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var game = Game.Load(ReadAllText("13/input1.txt"));
            game.Start();
            var blockCount = game.Count(Game.Obstacle.Block);
            return blockCount.ToString();
        }

        public string Job2()
        {
            var game = Game.Load(ReadAllText("13/input1.txt"));
            game.AutoPlay();
            return game.Score.ToString();
        }
    }

    public class Game
    {
        public enum Obstacle
        {
            Empty = 0,
            Wall,
            Block,
            HPaddle,
            Ball,
        }

        public enum Joystick
        {
            Neutral = 0,
            Left = -1,
            Right = 1,
        }

        private _05.Computer computer;
        private Dictionary<(int, int), Obstacle> tiles = new Dictionary<(int, int), Obstacle>();

        public long Score { get; private set; }

        public Game(long[] code)
        {
            computer = new _05.Computer(code);
        }

        public static Game Load(string sourceCode)
        {
            return new Game(_05.Computer.Compile(sourceCode));
        }

        public void Start()
        {
            List<long> outputs;
            computer.Exec(new List<long>(), out outputs);
            HandleOutput(outputs);
        }

        public void AutoPlay()
        {
            Score = 0;
            computer.Reset();
            computer.Patch(0, 2);
            var joystick = Joystick.Neutral;
            while (true)
            {
                var outputs = new List<long>();
                computer.Continue((long)joystick, outputs);

                tiles.Clear();
                HandleOutput(outputs);

                if (computer.IsHalted)
                    break;

                var ballPosition = -1;
                var paddlePosition = -1;
                foreach (var tile in tiles)
                {
                    if (tile.Value == Obstacle.Ball)
                        ballPosition = tile.Key.Item1;
                    if (tile.Value == Obstacle.HPaddle)
                        paddlePosition = tile.Key.Item1;
                }

                var distance = ballPosition - paddlePosition;
                if (distance > 0)
                    joystick = Joystick.Right;
                else if (distance < 0)
                    joystick = Joystick.Left;
                else
                    joystick = Joystick.Neutral;
            }
        }

        private void HandleOutput(List<long> tilesData)
        {
            for (int i = 0; i < tilesData.Count; i += 3)
            {
                var x = (int)tilesData[i + 0];
                var y = (int)tilesData[i + 1];
                if (x == -1 && y == 0)
                {
                    Score = tilesData[i + 2];
                }
                else
                {
                    var id = tilesData[i + 2] switch
                    {
                        0 => Obstacle.Empty,
                        1 => Obstacle.Wall,
                        2 => Obstacle.Block,
                        3 => Obstacle.HPaddle,
                        4 => Obstacle.Ball,
                        _ => throw new Exception($"Invalid obstacle: {tilesData[i + 2]}"),
                    };

                    tiles[(x, y)] = id;
                }
            }
        }

        public int Count(Obstacle id)
        {
            var idCount = (
                from value in tiles.Values
                where value == id
                select value
            ).Count();
            return idCount;
        }
    }
}
