using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day21
{
    public class Rule
    {
        public bool[,] Start { get; set; }
        public bool[,] Result { get; set; }

        public Rule(bool[,] start, bool[,] result)
        {
            Start = start;
            Result = result;
        }

        public bool FollowsTheRule(bool[,] array)
        {
            for (int i = 0; i < 4; i++)
            {
                if (AreEquals(array, Start) ||
                    AreEquals(LinearTransformation.FlipX.Transform(array), Start) ||
                    AreEquals(LinearTransformation.FlipY.Transform(array), Start)) return true;

                array = LinearTransformation.Rotate90DegreesClockwise.Transform(array);
            }
            return false;
        }

        private static bool AreEquals(bool[,] a, bool[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1)) return false;
            for (int x = 0; x < a.GetLength(0); x++)
                for (int y = 0; y < a.GetLength(1); y++)
                    if (a[x, y] != b[x, y]) return false;
            return true;
        }
    }

    public class FractalArt
    {
        public List<Rule> Rules { get; set; } = new();

        public FractalArt(string input)
        {
            foreach (string row in input.Split(Environment.NewLine))
            {
                string[] data = row.Split(" => ");
                Rules.Add(new(StringToArray(data[0]), StringToArray(data[1])));
            }
        }

        public bool[,] Generate(bool[,] array, int iterations)
        {
            int size = array.GetLength(0);

            for (int i = 0; i < iterations; i++)
            {
                bool[,] clone = (bool[,])array.Clone();
                int newSize;
                int offset;
                int nextOffset;

                if (size % 2 == 0)
                {
                    offset = 2;
                    nextOffset = 3;
                    newSize = size / offset * nextOffset;
                }
                else
                {
                    offset = 3;
                    nextOffset = 4;
                    newSize = size / offset * nextOffset;
                }

                array = new bool[newSize, newSize];
                for (int y = 0; y < size; y += offset)
                    for (int x = 0; x < size; x += offset)
                    {
                        bool[,] subArray = SubdivideArray(clone, x, x + offset, y, y + offset);
                        subArray = (bool[,])Rules.First(r => r.FollowsTheRule(subArray)).Result.Clone();
                        AssignSubArray(array, x / offset * nextOffset, y / offset * nextOffset, subArray);
                    }
                size = newSize;
            }

            return array;
        }

        public static int PixelsOn(bool[,] array)
        {
            int count = 0;
            foreach (var element in array)
                if (element) count++;
            return count;
        }

        private static bool[,] SubdivideArray(bool[,] array, int startX, int endX, int startY, int endY)
        {
            bool[,] subArray = new bool[endX - startX, endY - startY];

            for (int x = startX; x < endX; x++)
                for (int y = startY; y < endY; y++)
                    subArray[x - startX, y - startY] = array[x, y];

            return subArray;
        }

        private static void AssignSubArray(bool[,] array, int startX, int startY, bool[,] subArray)
        {
            for (int x = 0; x < subArray.GetLength(0); x++)
                for (int y = 0; y < subArray.GetLength(1); y++)
                    array[x + startX, y + startY] = subArray[x, y];
        }

        public static bool[,] StringToArray(string input)
        {
            string[] rows = input.Split('/');
            bool[,] array = new bool[rows.Length, rows.Length];

            for (int y = 0; y < rows.Length; y++)
                for (int x = 0; x < rows.Length; x++)
                    array[x, y] = rows[x][y] == '#';

            return array;
        }
    }

    public class LinearTransformation
    {
        public Vector2 I { get; set; }
        public Vector2 J { get; set; }

        public static LinearTransformation FlipX { get; } = new(new(-1, 0), new(0, 1));
        public static LinearTransformation FlipY { get; } = new(new(1, 0), new(0, -1));
        public static LinearTransformation Rotate90DegreesClockwise { get; } = new(new(0, -1), new(1, 0));
        public static LinearTransformation Rotate90DegreesCounterClockwise { get; } = new(new(0, 1), new(-1, 0));

        public LinearTransformation(Vector2 i, Vector2 j)
        {
            I = i;
            J = j;
        }

        public Vector2 Transform(Vector2 v)
            => new(v.X * I.X + v.Y * J.X, v.X * I.Y + v.Y * J.Y);

        public Vector2 Transform(Vector2 v, Vector2 origin)
            => Transform(v - origin) + origin;

        public T[,] Transform<T>(T[,] array)
        {
            if (array.GetLength(0) != array.GetLength(1)) throw new Exception();
            int size = array.GetLength(0);

            Vector2 origin = new((size - 1) / 2f, (size - 1) / 2f);
            var clone = (T[,])array.Clone();

            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    Vector2 transformedPosition = Transform(new Vector2(x, y), origin);
                    clone[(int)transformedPosition.X, (int)transformedPosition.Y] = array[x, y];
                }

            return clone;
        }
    }
}