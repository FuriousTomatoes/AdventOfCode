using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day17
{
    public class Spinlock
    {
        public static int Part1(int steps)
        {
            List<int> list = new() { 0 };

            int position = 0;
            for (int i = 1; i <= 2017; i++)
            {
                position = (position + steps) % list.Count + 1;
                list.Insert(position, i);
            }

            return list[(position + 1) % list.Count];
        }

        public static int Part2(int steps)
        {
            int position = 0;
            int secondValue = 0;
            for (int i = 1; i <= 50000000; i++)
            {
                position = (position + steps) % i + 1;
                if (position == 1) secondValue = i;
            }

            return secondValue;
        }
    }
}