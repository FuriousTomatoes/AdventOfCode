using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day9
{
    public class StreamProcessing
    {
        public static string ClearFromGarbage(string input)
        {
            while (input.Contains('!'))
                input = input.Remove(input.IndexOf('!'), 2);

            while (input.Contains('<'))
            {
                int startIndex = input.IndexOf('<');
                input = input.Remove(startIndex, input.IndexOf('>', startIndex) - startIndex);
            }

            input = new(input.ToList().Where(c => "{},".Contains(c)).ToArray());

            while (input.Contains("{,") || input.Contains(",}"))
            {
                input = input.Replace("{,", "{");
                input = input.Replace(",}", "}");
            }

            return input;
        }

        public static int CanceledCharacters(string input)
        {
            int count = 0;
            while (input.Contains('!'))
                input = input.Remove(input.IndexOf('!'), 2);

            while (input.Contains('<'))
            {
                int startIndex = input.IndexOf('<');
                int endIndex = input.IndexOf('>', startIndex) + 1;
                input = input.Remove(startIndex, endIndex - startIndex);
                count += endIndex - startIndex - 2;
            }

            return count;
        }

        public static int FindGroupScore(string groupString, int score = 1)
        {
            if (groupString.Length <= 2) return score;
            groupString = groupString[1..^1];

            int sum = score;
            //int nestedLevel = 0;
            int startIndex = groupString.IndexOf(',');

            foreach (string subGroup in SubGroups(groupString))
            {
                sum += FindGroupScore(subGroup, score + 1);
            }

            return sum;
        }

        private static List<string> SubGroups(string groupContent)
        {
            List<string> subGropus = new();

            int commaIndex = groupContent.IndexOf(',');
            int startIndex = -1;
            if (commaIndex == -1 || groupContent.Length == 2) return new() { groupContent };

            var list = groupContent.ToList();
            while (true)
            {
                int levelBeforeComma = list.Skip(startIndex + 1).Take(commaIndex - startIndex - 1).Sum(c => c == '{' ? 1 : c == '}' ? -1 : 0);

                if (levelBeforeComma == 0)
                {
                    if (commaIndex - startIndex - 1 == 2) subGropus.Add("");
                    else subGropus.Add(groupContent[(startIndex + 1)..commaIndex]);


                    startIndex = commaIndex;
                }

                if (commaIndex == groupContent.Length) break;
                commaIndex = groupContent.IndexOf(',', commaIndex + 1);
                if (commaIndex == -1)
                {
                    commaIndex = groupContent.Length;
                }
            }

            return subGropus;
        }
    }
}