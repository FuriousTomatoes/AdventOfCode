using System;
using System.Drawing;

namespace AdventOfCode.AoC2017.Day3
{
    public class SpiralMemory
    {
        public static int Part1(int input)
        {
            Point spiralToPoint = SpiralToPoint(input);
            return Math.Abs(spiralToPoint.X) + Math.Abs(spiralToPoint.Y);
        }

        public static int Part12(int input)
        {
            Point spiralToPoint = SpiralToPoint(input);
            return Math.Abs(spiralToPoint.X) + Math.Abs(spiralToPoint.Y);
        }

        /*
        public static Point SpiralToPoint(int input)
        {
            int minCommonRoot = (int)Math.Sqrt(input);
            if (minCommonRoot % 2 == 0)
            {
                int n = (input - minCommonRoot * minCommonRoot) / minCommonRoot + 1;
                return SpiralToPoint(input - n * minCommonRoot);
            }

            Point position = new(minCommonRoot / 2, -minCommonRoot / 2);
            int division = Math.DivRem(input - minCommonRoot * minCommonRoot, minCommonRoot, out int reminder);

            Point relativePosition = new();
            if (division == 0)
            {
                if (reminder > 0) relativePosition = new(1, reminder - 1);
            }
            else if (division == 1)
            {
                if (reminder == 0) relativePosition = new Point(1, minCommonRoot - 1);
                else relativePosition = new Point(2 - reminder, minCommonRoot);
            }
            else return new(1, 1);

            return new(relativePosition.X + position.X, relativePosition.Y + position.Y);
        }
        */

        public static Point SpiralToPoint(int input)
        {
            Point point = new();
            for (int sideLength = 0; ; sideLength++)
            {
                bool isSideLengthEven = sideLength % 2 == 0;

                for (int i = 0; i < sideLength; i++)
                {
                    if (--input == 0) return point;
                    point.X += isSideLengthEven ? 1 : -1;
                }

                for (int i = 0; i < sideLength; i++)
                {
                    if (--input == 0) return point;
                    point.Y += isSideLengthEven ? 1 : -1;
                }
            }
        }


        public static int Part2(int input)
        {
            int squareSideLength = 1001;
            int[,] spiral = new int[squareSideLength, squareSideLength];
            int arrayCenter = squareSideLength / 2;

            spiral[arrayCenter, arrayCenter] = 1;

            for (int i = 1; ; i++)
            {
                Point point = SpiralToPoint(i);
                point.X += arrayCenter;
                point.Y += arrayCenter;

                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        spiral[point.X, point.Y] += spiral[point.X + x, point.Y + y];
                    }

                if (spiral[point.X, point.Y] > input)
                    return spiral[point.X, point.Y];
            }
        }
    }
}