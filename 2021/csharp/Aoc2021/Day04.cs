namespace Aoc2021;

public class Day04 : IDay
{
    public override string Day { get; } = nameof(Day04)[3..];

    public override string Result1()
    {
        return Game
            .FromLines(InputLines)
            .PlayWinningBoard()
            .Score
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return Game
            .FromLines(InputLines)
            .PlayLoosingBoard()
            .Score
            .ToString(CultureInfo.InvariantCulture);
    }

    public record Game(List<int> Numbers, List<Board> Boards)
    {
        public Board PlayWinningBoard()
        {
            foreach (int number in Numbers)
            {
                foreach (Board board in Boards)
                {
                    if (board.Play(number))
                    {
                        return board;
                    }
                }
            }

            throw new ApplicationException("No Winner!");
        }

        public Board PlayLoosingBoard()
        {
            Board? lastWinningBoard = null;

            foreach (int number in Numbers)
            {
                List<int> winners = new();
                for (int i = 0; i < Boards.Count; i++)
                {
                    Board board = Boards[i];
                    if (board.Play(number))
                    {
                        winners.Add(i);
                        lastWinningBoard = board;
                    }
                }

                winners.Reverse();
                foreach (int i in winners)
                {
                    Boards.RemoveAt(i);
                }
            }

            return lastWinningBoard ?? throw new ApplicationException("No Winner!");
        }

        public static Game FromLines(IEnumerable<string> lines)
        {
            List<int> numbers = lines.First().Split(',').Select(ParseInt).ToList();
            List<Board> boards =
                lines.Skip(1)
                .Chunk(6)
                .Select(chunk => Board.FromLines(chunk.Skip(1)))
                .ToList();
            return new(numbers, boards);
        }
    }

    public record Board(List<Cell> Cells)
    {
        public int[] RowCounts { get; } = { 0, 0, 0, 0, 0 };
        public int[] ColumnCounts { get; } = { 0, 0, 0, 0, 0 };
        public int Score { get; private set; }

        public bool Play(int number)
        {
            int index = Cells.FindIndex(cell => cell.Value == number);
            if (index < 0)
            {
                return false;
            }

            Cells[index] = Cells[index] with { Marked = true };

            int row = index / 5;
            RowCounts[row] += 1;
            int column = index % 5;
            ColumnCounts[column] += 1;
            bool bingo = RowCounts[row] == 5 || ColumnCounts[column] == 5;
            if (bingo)
            {
                int sum = Cells.Sum(cell => cell.Marked ? 0 : cell.Value);
                Score = number * sum;
            }

            return bingo;
        }

        public static Board FromLines(IEnumerable<string> lines)
        {
            List<Cell> cells =
                lines
                .SelectMany(line => line
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(value => new Cell(ParseInt(value))))
                .ToList();
            return new(cells);
        }
    }

    public record Cell(int Value, bool Marked = false);
}
