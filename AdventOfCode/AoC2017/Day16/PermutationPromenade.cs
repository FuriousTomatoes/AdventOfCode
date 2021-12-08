using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day16
{
    public class PermutationPromenade
    {
        private static List<char> Programs { get; set; } = new();
        public static string Part1(string input)
        {

            for (int i = 0; i < 16; i++) Programs.Add((char)('a' + i));

            foreach (string instruction in input.Split(','))
            {
                if (instruction[0] == 's')
                {
                    int count = int.Parse(instruction[1..]);
                    Programs = Programs.TakeLast(count).Concat(Programs.Take(Programs.Count - count)).ToList();
                }
                else if (instruction[0] == 'x')
                {
                    string[] data = instruction[1..].Split('/');
                    int start = int.Parse(data[0]);
                    int end = int.Parse(data[1]);
                    SwapChars(Programs, start, end);
                }
                else
                    SwapChars(Programs, Programs.IndexOf(instruction[1]), Programs.IndexOf(instruction[3]));
            }

            return new(Programs.ToArray());
        }

        public static void Part1Void(string[] instructions)
        {
            foreach (string instruction in instructions)
            {
                if (instruction[0] == 's')
                {
                    int count = int.Parse(instruction[1..]);
                    Programs = Programs.TakeLast(count).Concat(Programs.Take(Programs.Count - count)).ToList();
                }
                else if (instruction[0] == 'x')
                {
                    string[] data = instruction[1..].Split('/');
                    int start = int.Parse(data[0]);
                    int end = int.Parse(data[1]);
                    SwapChars(Programs, start, end);
                }
                else SwapChars(Programs, Programs.IndexOf(instruction[1]), Programs.IndexOf(instruction[3]));
            }
        }

        static bool AreEquals(List<char> a, List<char> b)
        {
            for (int i = 0; i < 16; i++)
                if (a[i] != b[i]) return false;
            return true;
        }

        public static string Part2(string input, int n)
        {
            for (int i = 0; i < 16; i++) Programs.Add((char)('a' + i));
            var first = new List<char>(Programs);
            string[] instructions = input.Split(',');

            int reminder = n % 24;
            for (int i = 0; i < reminder; i++)
            {
                Part1Void(instructions);
            }

            /*   List<string> otherInstructions = new();
               char[] charMap = new char[16];
               for (int i = 0; i < 16; i++) charMap[i] = (char)('a' + i);

               for (int i = 0; i < instructions.Length; i++)
                   if (instructions[i][0] == 'p')
                       SwapChars(charMap, charMap.ToList().IndexOf(instructions[i][1]), charMap.ToList().IndexOf(instructions[i][3]));
                   else
                       otherInstructions.Add(instructions[i]);

               for (int i = 0; i < 16; i++) Programs.Add((char)('a' + i));
               Part1Void(otherInstructions.ToArray());

               var copy = new List<char>(Programs);
               for (int i = 0; i < 16; i++)
               {
                   Programs[i] = (char)(copy.IndexOf(charMap[i]) +'a');
               }
               /*

               for (int i = 0; i < 16; i++) Programs.Add((char)('a' + i));
               Part1Void(input.Split(','));
               byte[] map = new byte[16];
               for (byte i = 0; i < 16; i++) map[i] = (byte)Programs.IndexOf((char)(i + 'a'));

               Console.WriteLine(new string(Programs.ToArray()));
               Part1Void(input.Split(','));
               Console.WriteLine(new string(Programs.ToArray()));
               Part1Void(input.Split(','));
               Console.WriteLine(new string(Programs.ToArray()));

               byte[] output = new byte[16];
               for (byte i = 0; i < 16; i++) output[i] = i;
               for (int i = 0; i < n; i++)
               {
                   byte[] temp = (byte[])output.Clone();
                   for (byte element = 0; element < 16; element++)
                       output[map[element]] = temp[element];
               }

               //Console.WriteLine(new string(output.ToList().ConvertAll(n => (char)(n + 'a')).ToArray()));
               */

            return new(Programs.ToArray()); //new(output.ToList().ConvertAll(n => (char)(n + 'a')).ToArray());
        }


        private static void SwapChars(List<char> programs, int start, int end)
        {
            char startChar = programs[start];
            programs[start] = programs[end];
            programs[end] = startChar;
        }

        private static void SwapChars(char[] programs, int start, int end)
        {
            char startChar = programs[start];
            programs[start] = programs[end];
            programs[end] = startChar;
        }
    }
}