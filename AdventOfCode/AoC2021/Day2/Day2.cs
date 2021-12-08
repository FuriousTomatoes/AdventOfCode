using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2021.Day2
{
    public class Day2
    {
        public static long Part1(string input)
        {
            Vector2 position = new();
            foreach (string instruction in input.Split(Environment.NewLine))
            {
                string[] data = instruction.Split(' ');
                Vector2 direction = data[0] switch
                {
                    "up" => new(0, 1),
                    "down" => new(0, -1),
                    "forward" => new(1, 0),
                    _ => throw new Exception()
                };
                position += direction * int.Parse(data[1]);
            }

            return (long)(position.X * position.Y);
        }

        public static long Part2(string input)
        {
            Vector2 position = new();
            int aim = 0;
            foreach (string instruction in input.Split(Environment.NewLine))
            {
                string[] data = instruction.Split(' ');
                int parameter = int.Parse(data[1]);
                switch (data[0])
                {
                    case "up": aim -= parameter; break;
                    case "down": aim += parameter; break;
                    case "forward":
                        position += new Vector2(parameter, aim * parameter);
                        break;
                }
            }

            return (long)position.X * (long)position.Y;
        }
    }
}