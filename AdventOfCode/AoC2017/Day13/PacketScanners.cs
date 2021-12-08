using System;
using System.Collections.Generic;

namespace AdventOfCode.AoC2017.Day13
{
    public class Scanner
    {
        public int Direction { get; private set; } = 1;
        public int Position { get; private set; }
        public int Depth { get; set; }
        public int Range { get; set; }

        public void Move()
        {
            int newPosition = Position + Direction;
            if (newPosition < 0 || newPosition >= Range) Direction = -Direction;

            Position += Direction;
        }

        public void Move(int times)
        {
            Position = (Position + Direction * times) % (Range * 2 - 2);

            if (Position >= Range)
            {
                Position = Range * 2 - 2 - Position;
                Direction = -Direction;
            }
        }
    }

    public class PacketScanners
    {
        private List<Scanner> Scanners { get; set; } = new();

        public void Populate(string input)
        {
            foreach (string scannerString in input.Split(Environment.NewLine))
            {
                string[] data = scannerString.Split(": ");
                Scanners.Add(new() { Depth = int.Parse(data[0]), Range = int.Parse(data[1]) });
            }
        }

        public int GetSeverity(int delay = 0)
        {
            Scanners = Scanners.ConvertAll(scanner => new Scanner() { Depth = scanner.Depth, Range = scanner.Range });
            Scanners.ForEach(scanner => scanner.Move(delay));

            int severity = 0;
            for (int scannerIndex = 0; scannerIndex < Scanners.Count; scannerIndex++)
            {
                int offset = 0;
                if (scannerIndex + 1 < Scanners.Count)
                    offset = Scanners[scannerIndex + 1].Depth - Scanners[scannerIndex].Depth;

                if (Scanners[scannerIndex].Position == 0)
                    severity += Scanners[scannerIndex].Depth * Scanners[scannerIndex].Range;

                Scanners.ForEach(scanner => scanner.Move(offset));
            }
            return severity;
        }


        public int GetTimesCaught(int delay = 0)
        {
            Scanners = Scanners.ConvertAll(scanner => new Scanner() { Depth = scanner.Depth, Range = scanner.Range });
            Scanners.ForEach(scanner => scanner.Move(delay));

            for (int scannerIndex = 0; scannerIndex < Scanners.Count; scannerIndex++)
            {
                int offset = 0;
                if (scannerIndex + 1 < Scanners.Count)
                    offset = Scanners[scannerIndex + 1].Depth - Scanners[scannerIndex].Depth;

                if (Scanners[scannerIndex].Position == 0)
                    return 1;

                Scanners.ForEach(scanner => scanner.Move(offset));
            }
            return 0;
        }

        public int DelayToZeroSeverity()
        {
            for (int i = 0; ; i++)
                if (GetTimesCaught(i) == 0) return i;
            //else if (i > 100000) Console.WriteLine("X: " + i);
        }

    }
}