using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day15
{
    public class DuelingGenerators
    {
        public static int Part1(ulong generatorAValue, ulong generatorBValue)
        {
            int matches = 0;
            for (int i = 0; i < 40_000_000; i++)
            {
                generatorAValue = Next(generatorAValue, 16807);
                generatorBValue = Next(generatorBValue, 48271);

                if (Last16Bits(generatorAValue) == Last16Bits(generatorBValue)) matches++;
            }
            return matches;
        }

        public static int Part2(ulong generatorAValue, ulong generatorBValue)
        {
            int matches = 0;
            bool aOk = false, bOk = false;
            for (int i = 0; i < 5_000_000;)
            {
                if (!aOk) generatorAValue = Next(generatorAValue, 16807, 4, out aOk);
                if (!bOk) generatorBValue = Next(generatorBValue, 48271, 8, out bOk);

                if (aOk && bOk)
                {
                    i++;
                    if (Last16Bits(generatorAValue) == Last16Bits(generatorBValue)) matches++;

                    aOk = false;
                    bOk = false;
                }
            }
            return matches;
        }

        private static ulong Next(ulong previousValue, ulong factor, ulong multipleOf, out bool ok)
        {
            ulong number = factor * previousValue % int.MaxValue;
            ok = number % multipleOf == 0;
            return number;
        }

        private static ulong Next(ulong previousValue, ulong factor)
            => factor * previousValue % int.MaxValue;

        private static ulong Last16Bits(ulong number)
            => number << 48;


    }
}