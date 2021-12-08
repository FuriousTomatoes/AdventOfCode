using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2021.Day3
{
    public class Day3
    {
        public static uint GetPowerConsumption(string input)
        {
            uint gammaRate = 0;
            string[] rows = input.Split(Environment.NewLine);
            for (int i = 0; i < rows[0].Length; i++)
            {
                int zeros = 0;
                int ones = 0;
                for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
                    if (rows[rowIndex][^(i + 1)] == '1') ones++;
                    else zeros++;
                if (ones > zeros) gammaRate += (uint)Math.Pow(2, i);
            }
            uint epsilonRate = (~gammaRate) << (32 - rows[0].Length) >> (32 - rows[0].Length);

            return epsilonRate * gammaRate;
        }

        private static byte MostCommonBit(List<int> values, int position)
        {
            int ones = 0, zeros = 0;
            for (int rowIndex = 0; rowIndex < values.Count; rowIndex++)
                if (Convert.ToString(values[rowIndex], 2).PadLeft(12)[position] == '1') ones++;
                else zeros++;

            return (byte)(ones >= zeros ? 1 : 0);
        }

        public static int GetOxygenGeneratorRating(string input)
        {
            string[] rows = input.Split(Environment.NewLine);
            List<int> values = rows.ToList().ConvertAll(s => Convert.ToInt32(s, 2));
            for (int i = 0; i < rows[0].Length; i++)
            {
                int bitmask = (int)Math.Pow(2, rows[0].Length - i - 1);
                int mostCommonBit = MostCommonBit(values, i);
                values = values.Where(value => ((value & bitmask) == 0) == (mostCommonBit == 0)).ToList();
                if (values.Count == 1) return values[0];
            }
            throw new Exception();
        }

        public static int GetC02ScrubberRating(string input)
        {
            string[] rows = input.Split(Environment.NewLine);
            List<int> values = rows.ToList().ConvertAll(s => Convert.ToInt32(s, 2));
            for (int i = 0; i < rows[0].Length; i++)
            {
                int bitmask = (int)Math.Pow(2, rows[0].Length - i - 1);
                int leastCommonBit = MostCommonBit(values, i) == 1 ? 0 : 1;
                values = values.Where(value => ((value & bitmask) == 0) == (leastCommonBit == 0)).ToList();
                if (values.Count == 1) return values[0];
            }
            throw new Exception();
        }
    }
}