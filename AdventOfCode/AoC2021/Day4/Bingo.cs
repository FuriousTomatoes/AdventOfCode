using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2021.Day4
{
    public class Board
    {
        public int[,] Numbers { get; set; } = new int[5, 5];
        public bool[,] Marks { get; set; } = new bool[5, 5];

        public bool HasWon(Point lastPosition)
        {
            bool horizontal = true;
            bool vertical = true;
            for (int i = 0; i < 5; i++)
            {
                if (horizontal && !Marks[i, lastPosition.Y]) horizontal = false;
                if (vertical && !Marks[lastPosition.X, i]) vertical = false;
                if (!horizontal && !vertical) return false;
            }

             return horizontal || vertical;
        }

        public Point? Mark(int number)
        {
            for (int x = 0; x < Numbers.GetLength(0); x++)
                for (int y = 0; y < Numbers.GetLength(1); y++)
                    if (Numbers[x, y] == number)
                    {
                        Marks[x, y] = true;
                        return new(x, y);
                    }
            return null;
        }
    }

    public class Bingo
    {
        public Queue<int> DrawnNumbers { get; set; } = new();
        public List<Board> Boards { get; set; } = new();

        public Bingo(string input)
        {
            input = input.Replace("  ", " ");
            string[] rows = input.Split(Environment.NewLine);
            DrawnNumbers = new(rows[0].Split(',').ToList().ConvertAll(int.Parse));
            for (int boardIndex = 2; boardIndex < rows.Length; boardIndex += 6)
            {
                Board board = new();
                for (int y = 0; y < 5; y++)
                {
                    string[] values = rows[boardIndex + y].Trim().Split(' ');
                    for (int x = 0; x < 5; x++)
                        board.Numbers[x, y] = int.Parse(values[x]);
                }
                Boards.Add(board);
            }
        }

        public int WinnerScore()
        {
            while (DrawnNumbers.Count > 0)
            {
                int drawnNumber = DrawnNumbers.Dequeue();
                foreach (Board board in Boards)
                {
                    Point? markedPoint = board.Mark(drawnNumber);
                    if (markedPoint != null && board.HasWon(markedPoint.Value))
                    {
                        int score = 0;
                        for (int x = 0; x < 5; x++)
                            for (int y = 0; y < 5; y++)
                                if (!board.Marks[x, y]) score += board.Numbers[x, y];
                        score *= board.Numbers[markedPoint.Value.X, markedPoint.Value.Y];
                        return score;
                    }
                }
            }
            return 0;
        }

        public int LooserScore()
        {
            var remainingBoards = new List<Board>(Boards);
            while (DrawnNumbers.Count > 0)
            {
                int drawnNumber = DrawnNumbers.Dequeue();
                for (int i = 0; i < remainingBoards.Count; i++)
                {
                    Board board = remainingBoards[i];
                    Point? markedPoint = board.Mark(drawnNumber);
                    if (markedPoint != null && board.HasWon(markedPoint.Value))
                    {
                        if (remainingBoards.Count == 1)
                        {
                            int score = 0;
                            for (int x = 0; x < 5; x++)
                                for (int y = 0; y < 5; y++)
                                    if (!board.Marks[x, y]) score += board.Numbers[x, y];
                            score *= board.Numbers[markedPoint.Value.X, markedPoint.Value.Y];
                            return score;
                        }
                        remainingBoards.Remove(board);
                    }
                }
            }
            return 0;
        }
    }
}
