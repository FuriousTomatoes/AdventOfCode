using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day23
{
    public class Register
    {
        public char Name { get; set; }
        public BigInteger Value { get; set; }
    }

    public class Coprocessor
    {
        public Register[] Registers { get; set; } = new Register[8];
        public string[] Instructions { get; set; }
        public string[][] Data { get; set; }
        public long Index { get; set; }
        //public long TimesMultiplied { get; set; }

        public Coprocessor(string instructions)
        {
            for (int i = 0; i < Registers.Length; i++)
            {
                Registers[i] = new();
            }
            Instructions = instructions.Split(Environment.NewLine);
            Data = new string[Instructions.Length][];
            for (int i = 0; i < Instructions.Length; i++)
                Data[i] = Instructions[i].Split(' ');
        }

        public long TimesMultiplied()
        {
            long timesMultiplied = 0;
            long maxIndex = 0;


            for (Index = 0; Index < Instructions.Length;)
            {
                if (!BigInteger.TryParse(Data[Index][2], out BigInteger parameter))
                    parameter = Registers[Data[Index][2][0] - 'a'].Value;

                if (Data[Index][0] == "set") Registers[Data[Index][1][0] - 'a'].Value = parameter;
                else if (Data[Index][0] == "sub")
                {
                    // if (Data[Index][2][0] < 'a')
                    //     parameter = -parameter;
                    Registers[Data[Index][1][0] - 'a'].Value -= parameter;
                }
                else if (Data[Index][0] == "mul")
                {
                    Registers[Data[Index][1][0] - 'a'].Value *= parameter;
                    timesMultiplied++;
                }
                else if (Data[Index][0] == "jnz")
                {
                    if (!BigInteger.TryParse(Data[Index][1], out BigInteger condition))
                        condition = Registers[Data[Index][1][0] - 'a'].Value;

                    if (condition != 0)
                    {
                        Index += (long)parameter;
                        continue;
                    }
                }
                if (maxIndex < Index)
                {
                    maxIndex = Index;
                    Console.WriteLine(maxIndex + " " + Instructions[Index]);
                }
                Index++;
            }
            return timesMultiplied;
        }

        public long SwitchDebugMode()
        {
            long b, c, d, e, f, h = 0;
            b = 108_400;
            c = 125_400;

            while (true)
            {
                f = 1;
                d = 2;

                do
                {
                    long division = Math.DivRem(b, d, out long reminder);
                    if (reminder == 0 && division >= 2 && division < d) f = 0;

                    d++;
                } while (d != b);

                if (f == 0) h++;
                if (b == c) return h;
                b += 17;
            }
        }
    }
}