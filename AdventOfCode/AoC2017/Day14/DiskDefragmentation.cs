using AdventOfCode.AoC2017.Day10;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day14
{
    public class Square
    {
        public Point Position { get; set; }
        public List<Square> ConnectedSquares { get; set; } = new();
    }

    public class DiskDefragmentation
    {
        private Dictionary<Point, Square> UsedSquares { get; set; } = new();

        public static int Part1(string input)
        {
            bool[][] bytes = new bool[128][];

            for (int i = 0; i < 128; i++)
                bytes[i] = HashToBoolArray(input + $"-{i}");

            return bytes.Sum(column => column.Count(b => b));
        }

        public static bool[] HashToBoolArray(string input)
        {
            string binary = string.Join(string.Empty, KnotHash.Calculate(input).Select(c =>
                Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            return binary.ToList().ConvertAll(c => c == '1').ToArray();
        }

        public int RegionsCount(string input)
        {
            for (int x = 0; x < 128; x++)
            {
                bool[] occupedSquares = HashToBoolArray(input + $"-{x}");
                for (int y = 0; y < 128; y++)
                    if (occupedSquares[y])
                        LinkToNearestPoints(new() { Position = new(x, y) });
            }

            int regionsCount = 0;
            while (UsedSquares.Count > 0)
            {
                regionsCount++;
                UsedSquares = UsedSquares.ToList().ConvertAll(pair => pair.Value).
                    Except(GetRegion(UsedSquares.ToList()[0].Value))
                    .ToDictionary(square => square.Position);
            }
            return regionsCount;
        }

        public List<Square> GetRegion(Square square, List<Square> alreadyConnectedSquares = null)
        {
            if (alreadyConnectedSquares == null) alreadyConnectedSquares = new();
            if (!alreadyConnectedSquares.Contains(square)) alreadyConnectedSquares.Add(square);

            foreach (Square connectedSquare in square.ConnectedSquares.Except(alreadyConnectedSquares))
                alreadyConnectedSquares.AddRange(GetRegion(connectedSquare, alreadyConnectedSquares).Except(alreadyConnectedSquares));

            return alreadyConnectedSquares;
        }


        private void LinkToNearestPoints(Square square)
        {
            if (!UsedSquares.ContainsKey(square.Position)) UsedSquares.Add(square.Position, square);
            List<Point> list = new() { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
            list.ForEach(relativePoint =>
            {
                Point position = new(square.Position.X + relativePoint.X, square.Position.Y + relativePoint.Y);
                if (UsedSquares.ContainsKey(position))
                {
                    Square nearSquare = UsedSquares[position];
                    square.ConnectedSquares.Add(nearSquare);
                    nearSquare.ConnectedSquares.Add(square);
                }
            });
        }
    }
}