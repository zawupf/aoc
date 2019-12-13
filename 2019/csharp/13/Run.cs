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
            throw new NotImplementedException();
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

        private _05.Computer computer;
        private Dictionary<(int, int), Obstacle> tiles = new Dictionary<(int, int), Obstacle>();

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
            BuildFields(outputs);
        }

        private void BuildFields(List<long> tilesData)
        {
            for (int i = 0; i < tilesData.Count; i += 3)
            {
                var x = (int)tilesData[i + 0];
                var y = (int)tilesData[i + 1];
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
