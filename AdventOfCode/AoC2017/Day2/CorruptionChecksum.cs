using System;
using System.Linq;

namespace AdventOfCode.AoC2017.Day2
{
    public class CorruptionChecksum
    {
        public static int Part1(string input)
            => input.Split(Environment.NewLine).ToList().Sum(row =>
            {
                var list = row.Split('\t').ToList().ConvertAll(int.Parse);
                return list.Max() - list.Min();
            });

        public static int Part2(string input)
            => input.Split(Environment.NewLine).ToList().Sum(row =>
            {
                var list = row.Split('\t').ToList().ConvertAll(int.Parse);

                foreach (int number in list)
                {
                    int otherNumber = list.FirstOrDefault(otherNumber => number != otherNumber && number % otherNumber == 0);
                    if (otherNumber != 0) return number / otherNumber;
                }
                return 0;
            });
    }
}