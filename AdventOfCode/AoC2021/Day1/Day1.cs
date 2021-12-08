using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2021.Day1
{
    public class Day1
    {
        public static int IncreasedTimes1(string input)
        {
            int increasedTimes = 0;
            int previousValue = int.MaxValue;

            foreach (string line in input.Split(Environment.NewLine))
            {
                int value = int.Parse(line);
                if (value > previousValue) increasedTimes++;
                previousValue = value;
            }

            return increasedTimes;
        }

        public static int IncreasedTimes2(string input)
        {
            int increasedTimes = 0;
            int previousValue = int.MaxValue;
            string[] lines = input.Split(Environment.NewLine);

            for (int i = 0; i < lines.Length - 2; i++)
            {
                int value = int.Parse(lines[i]) + int.Parse(lines[i + 1]) + int.Parse(lines[i + 2]);
                if (value > previousValue) increasedTimes++;
                previousValue = value;
            }

            return increasedTimes;
        }
    }
}