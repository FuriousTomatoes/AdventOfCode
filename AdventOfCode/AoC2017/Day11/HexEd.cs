using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AdventOfCode.AoC2017.Day11
{
    public static class HexagonRelations
    {
        public static Point Nord { get; } = new(0, 2);
        public static Point NordEast { get; } = new(1, 1);
        public static Point NordWest { get; } = new(-1, 1);
        public static Point South { get; } = new(0, -2);
        public static Point SouthEast { get; } = new(1, -1);
        public static Point SouthWest { get; } = new(-1, -1);

        public static void Add(this ref Point point, Point otherPoint)
        {
            point.X += otherPoint.X;
            point.Y += otherPoint.Y;
        }

        public static Point FromString(string s)
            => s switch
            {
                "n" => Nord,
                "ne" => NordEast,
                "nw" => NordWest,
                "s" => South,
                "se" => SouthEast,
                "sw" => SouthWest,
            };
    }

    public class HexEd
    {
        public static int Part1(string input)
        {
            Point p = new();

            foreach (string direction in input.Split(','))
                p.Add(HexagonRelations.FromString(direction));

            int steps = 0;
            if (Math.Abs(p.X) == Math.Abs(p.Y)) steps = p.X;
            else if (Math.Abs(p.X) < Math.Abs(p.Y))
            {
                if ((p.Y - p.X) % 2 != 0) steps += 2;
                steps += p.X + (p.Y - p.X) / 2;
            }
            else
            {
                if (p.X % 2 == 0) steps++;
                steps += p.X;
            }

            return Math.Abs(steps);
        }


        public static int Part2(string input)
        {
            Point p = new();
            return input.Split(',').Max(direction =>
            {
                p.Add(HexagonRelations.FromString(direction));

                int steps = 0;
                if (Math.Abs(p.X) == Math.Abs(p.Y)) steps = p.X;
                else if (Math.Abs(p.X) < Math.Abs(p.Y))
                {
                    if ((p.Y - p.X) % 2 != 0) steps += 2;
                    steps += p.X + (p.Y - p.X) / 2;
                }
                else
                {
                    if (p.X % 2 == 0) steps++;
                    steps += p.X;
                }

                return Math.Abs(steps);
            });
        }
    }
}