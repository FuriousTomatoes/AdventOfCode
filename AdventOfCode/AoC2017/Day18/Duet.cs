using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day18
{
    public class Register
    {
        public char Name { get; set; }
        public BigInteger Value { get; set; }
    }

    public class Program
    {
        public Program OtherProgram { get; set; }

        public List<Register> Registers { get; set; } = new();
        public Queue<BigInteger> Buffer { get; set; } = new();
        public BigInteger Index { get; set; }
        public string[] Instructions { get; set; }
        public long TimesSent { get; set; }

        public Program(string[] instructions)
        {
            Instructions = instructions;
        }

        public Register GetRegister(char name)
        {
            Register register = Registers.FirstOrDefault(register => register.Name == name);
            if (register == null)
            {
                register = new() { Name = name };
                Registers.Add(register);
            }
            return register;
        }

        public bool ExecuteUntilReceive(out bool deadlock)
        {
            deadlock = false;
            bool isFirstTime = true;
            while (true)
            {
                if (Index >= Instructions.Length) return false;
                string[] data = Instructions[(int)Index].Split(' ');

                if (data[0] == "rcv")
                {
                    if (isFirstTime)
                    {
                        if (Buffer.Count == 0)
                        {
                            deadlock = true;
                            return false;
                        }
                        GetRegister(data[1][0]).Value = Buffer.Dequeue();
                    }
                    else if (Buffer.Count == 0) return true;
                    else GetRegister(data[1][0]).Value = Buffer.Dequeue();

                    isFirstTime = false;
                }
                else
                {
                    isFirstTime = false;
                    if (data[0] == "snd")
                    {
                        if (!BigInteger.TryParse(data[1], out BigInteger firstParameter))
                            firstParameter = GetRegister(data[1][0]).Value;

                        OtherProgram.Buffer.Enqueue(firstParameter);
                        TimesSent++;
                    }
                    else if (data[0] == "jgz")
                    {
                        if (!BigInteger.TryParse(data[1], out BigInteger firstParameter))
                            firstParameter = GetRegister(data[1][0]).Value;

                        if (!BigInteger.TryParse(data[2], out BigInteger secondParameter))
                            secondParameter = GetRegister(data[2][0]).Value;

                        if (firstParameter > 0)
                        {
                            Index += secondParameter;
                            continue;
                        }
                    }
                    else
                    {
                        if (!BigInteger.TryParse(data[2], out BigInteger parameter))
                            parameter = GetRegister(data[2][0]).Value;

                        if (data[0] == "mod") GetRegister(data[1][0]).Value %= parameter;
                        else if (data[0] == "set") GetRegister(data[1][0]).Value = parameter;
                        else if (data[0] == "add") GetRegister(data[1][0]).Value += parameter;
                        else if (data[0] == "mul") GetRegister(data[1][0]).Value *= parameter;
                    }
                }

                Index++;
            }
        }
    }

    public class Duet
    {
        public Dictionary<char, Register> Registers { get; set; } = new();

        /*
        private Register GetRegisterValue(Dictionary<char, Register> register, char key)
        {
            if (!register.ContainsKey(key))
                register.Add(key, new Register { Name = key });
            return register[key];
        }
        */
        /*
        public long RecoveryFrequency(string input)
        {
            long lastSound = 0;
            string[] instructions = input.Split(Environment.NewLine);
            for (long i = 0; i < instructions.Length;)
            {
                string[] data = instructions[i].Split(' ');
                if (data[0] == "snd")
                {
                    if (!long.TryParse(data[1], out long firstParameter))
                        firstParameter = GetRegisterValue(data[1][0]).Value;

                    lastSound = firstParameter;
                }
                else if (data[0] == "rcv")
                {
                    if (GetRegisterValue(data[1][0]).Value != 0)
                        return lastSound;
                }
                else if (data[0] == "jgz")
                {
                    if (!long.TryParse(data[1], out long firstParameter))
                        firstParameter = GetRegisterValue(data[1][0]).Value;

                    if (!long.TryParse(data[2], out long secondParameter))
                        secondParameter = GetRegisterValue(data[2][0]).Value;

                    if (firstParameter > 0)
                    {
                        i += secondParameter;
                        if (secondParameter > 0) i--;
                        continue;
                    }
                }
                else
                {
                    if (!long.TryParse(data[2], out long parameter))
                        parameter = GetRegisterValue(data[2][0]).Value;

                    if (data[0] == "mod") GetRegisterValue(data[1][0]).Value %= parameter;
                    else if (data[0] == "set") GetRegisterValue(data[1][0]).Value = parameter;
                    else if (data[0] == "add") GetRegisterValue(data[1][0]).Value += parameter;
                    else if (data[0] == "mul") GetRegisterValue(data[1][0]).Value *= parameter;
                }
                i++;
            }
            return 0;
        }
        */

        public static long TimesProgramSentAValue(string input)
        {
            string[] instructions = input.Split(Environment.NewLine);
            Program a = new(instructions);
            Program b = new(instructions);
            a.OtherProgram = b;
            b.OtherProgram = a;
            b.GetRegister('p').Value = 1;

            while (true)
            {
                bool aOk = a.ExecuteUntilReceive(out bool deadlockA);
                bool bOk = b.ExecuteUntilReceive(out bool deadlockB);
                if (deadlockA || deadlockB || !aOk && !bOk) return b.TimesSent;
            }
        }
    }
}