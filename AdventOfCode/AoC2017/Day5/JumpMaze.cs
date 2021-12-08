using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day5
{
    public class JumpMaze
    {
        public static int Part1(string input)
        {
            var maze = input.Split(Environment.NewLine).ToList().ConvertAll(int.Parse);

            int count = 1;
            for (int index = 0; ; count++)
            {
                int previousIndex = index;
                index += maze[index];
                if (index < 0 || index >= maze.Count) return count;
                maze[previousIndex]++;
            }
        }

        public static int Part2(string input)
        {
            var maze = input.Split(Environment.NewLine).ToList().ConvertAll(int.Parse);

            int count = 1;
            for (int index = 0; ; count++)
            {
                int previousIndex = index;
                index += maze[index];
                if (index < 0 || index >= maze.Count) return count;
                if (maze[previousIndex] >= 3) maze[previousIndex]--;
                else maze[previousIndex]++;
            }
        }
    }
}