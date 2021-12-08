using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day25
{
    public class Procedure
    {
        public bool WritingContent { get; set; }
        public bool MoveRight { get; set; }
        public byte NextStateIndex { get; set; }
    }

    public class State
    {
        public Procedure On0 { get; set; }
        public Procedure On1 { get; set; }
    }

    public class HaltingProblem
    {
        private List<State> States { get; set; } = new();
        private int Steps { get; set; }
        private bool[] Tape { get; set; }
        private int TapeCenter { get; set; }

        public HaltingProblem(string input)
        {
            var rows = input.Split(Environment.NewLine);
            Steps = int.Parse(rows[1].Split(' ')[^2]);
            Tape = new bool[Steps * 2 + 1];
            TapeCenter = Tape.Length / 2;

            for (int stateIndex = 3; stateIndex < rows.Length; stateIndex += 10)
            {
                Procedure procedure0 = new()
                {
                    WritingContent = rows[stateIndex + 2][^2] == '1',
                    MoveRight = rows[stateIndex + 3].Split(' ')[^1] == "right.",
                    NextStateIndex = (byte)(rows[stateIndex + 4][^2] - 'A'),
                };
                Procedure procedure1 = new()
                {
                    WritingContent = rows[stateIndex + 6][^2] == '1',
                    MoveRight = rows[stateIndex + 7].Split(' ')[^1] == "right.",
                    NextStateIndex = (byte)(rows[stateIndex + 8][^2] - 'A'),
                };

                States.Add(new() { On0 = procedure0, On1 = procedure1 });
            }
        }

        public int GetDiagnosticChecksum()
        {
            byte stateIndex = 0;
            for (int i = 0, position = TapeCenter; i < Steps; i++)
            {
                bool moveRight;
                //Select right procedure
                Procedure procedure = Tape[position] ? States[stateIndex].On1 : States[stateIndex].On0;

                moveRight = procedure.MoveRight;
                Tape[position] = procedure.WritingContent;
                stateIndex = procedure.NextStateIndex;

                position += moveRight ? 1 : -1;
            }
            return Tape.Count(e => e);
        }
    }
}