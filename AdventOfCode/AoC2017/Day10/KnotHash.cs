using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day10
{
    public class KnotHash
    {

        public static int Part1(string input)
        {
            List<int> numbers = new int[256].ToList();
            for (int i = 0; i < numbers.Count; i++) numbers[i] = i;
            List<int> lengths = input.Split(',').ToList().ConvertAll(int.Parse).Where(length => length < numbers.Count).ToList();
            int currentPosition = 0;
            int skipSize = 0;

            foreach (int length in lengths)
            {
                numbers = numbers.CircularTake(length, currentPosition).ToList();
                currentPosition += length + skipSize;
                if (currentPosition >= numbers.Count) currentPosition -= numbers.Count;

                skipSize++;
            }

            return numbers[0] * numbers[1];
        }

        public static string Calculate(string input)
        {
            List<int> numbers = new int[256].ToList();
            for (int i = 0; i < numbers.Count; i++) numbers[i] = i;
            List<int> lengths = input.ToList().ConvertAll(c => (int)c).Concat(new List<int> { 17, 31, 73, 47, 23 }).ToList();
            int currentPosition = 0;
            int skipSize = 0;

            for (int n = 0; n < 64; n++)
            {
                //for (int i = 0; 
                //   i < numbers.Count; i++) numbers[i] = i;
                foreach (int length in lengths)
                {
                    numbers = numbers.CircularTake(length, currentPosition).ToList();
                    currentPosition += length + skipSize;
                    if (currentPosition >= numbers.Count) currentPosition %= numbers.Count;

                    skipSize++;
                }
            }

            List<int> hashes = new();
            for (int hashIndex = 0; hashIndex < 16; hashIndex++)
            {
                int xor = numbers[hashIndex * 16];
                for (int i = 1; i < 16; i++)
                    xor ^= numbers[hashIndex * 16 + i];
                hashes.Add(xor);
            }

            return Convert.ToHexString(hashes.ConvertAll(n => (byte)n).ToArray()).ToLower();
        }
    }

    internal static class ListExtensions
    {
        public static IEnumerable<T> CircularTake<T>(this IEnumerable<T> enumerable, int count, int startPosition = 0)
        {
            int overflow = 0;
            int safeCount = count;
            if (count + startPosition > enumerable.Count())
            {
                overflow = startPosition + count - enumerable.Count();
                safeCount = count - overflow;
            }
            if (overflow > startPosition) throw new OverflowException();

            if (overflow == 0)
            {
                return enumerable
                    .Take(startPosition)
                    .Concat(enumerable
                        .Skip(startPosition)
                        .Take(safeCount)
                        .Reverse())
                    .Concat(enumerable.Skip(safeCount + startPosition));
            }
            else
            {
                var notReversedPart = enumerable.Skip(overflow).Take(startPosition - overflow);

                var reversedPart = enumerable.Skip(startPosition)
                    .Take(safeCount)
                    .Concat(enumerable.Take(overflow))
                    .Reverse();

                return reversedPart.TakeLast(overflow)
                    .Concat(notReversedPart)
                    .Concat(reversedPart.Take(safeCount));
            }
        }
    }
}