using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day8
{
    public enum InstructionType { Increment = 1, Decrement = -1 }

    public class Instruction
    {
        public Register Register { get; set; }
        public InstructionType InstructionType { get; set; }
        public int Amount { get; set; }
        public Condition Condition { get; set; }
    }

    public static class Instructor
    {
        public static Func<int, int, bool> Greater { get; } = new((a, b) => a > b);
        public static Func<int, int, bool> GreaterOrEqual { get; } = new((a, b) => a >= b);
        public static Func<int, int, bool> Less { get; } = new((a, b) => a < b);
        public static Func<int, int, bool> LessOrEqual { get; } = new((a, b) => a <= b);
        public static Func<int, int, bool> Equal { get; } = new((a, b) => a == b);
        public static Func<int, int, bool> Different { get; } = new((a, b) => a != b);

        public static Func<int, int, bool> FromString(string s)
            => s switch
            {
                ">" => Greater,
                ">=" => GreaterOrEqual,
                "<" => Less,
                "<=" => LessOrEqual,
                "==" => Equal,
                "!=" => Different,
            };
    }

    public struct Condition
    {
        public Register Register { get; set; }
        public Func<int, int, bool> Instructor { get; set; }
        public bool Validate() => Instructor(Register.Value, Value);
        public int Value { get; set; }
    }

    public class Register
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class InstructionsExecutor
    {
        List<Instruction> Instructions { get; set; } = new();
        List<Register> Registers { get; set; } = new();

        public void FromAoCInput(string input)
        {
            foreach (string instructionString in input.Split(Environment.NewLine))
            {
                string[] data = instructionString.Split(' ');
                Instruction instruction = new();
                Instructions.Add(instruction);

                Register register = FindRegister(data[0]);
                instruction.Register = register;
                instruction.InstructionType = data[1] == "inc" ? InstructionType.Increment : InstructionType.Decrement;
                instruction.Amount = int.Parse(data[2]);

                instruction.Condition = new()
                {
                    Instructor = Instructor.FromString(data[5]),
                    Value = int.Parse(data[6]),
                    Register = FindRegister(data[4]),
                };
            }
        }

        public void Execute()
        {
            foreach (Instruction instruction in Instructions)
                if (instruction.Condition.Validate())
                    instruction.Register.Value += (int)instruction.InstructionType * instruction.Amount;
        }

        public int MaxValueEverExecution()
            => Instructions.Max(instruction =>
            {
                if (instruction.Condition.Validate())
                    instruction.Register.Value += (int)instruction.InstructionType * instruction.Amount;
                return instruction.Register.Value;
            });

        public int GreaterRegisterValue() => Registers.Max(register => register.Value);

        private Register FindRegister(string registerName)
        {
            Register register = Registers.FirstOrDefault(r => r.Name == registerName);
            if (register == null)
            {
                register = new() { Name = registerName };
                Registers.Add(register);
            }
            return register;
        }

    }
}