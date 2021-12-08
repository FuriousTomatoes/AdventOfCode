using AdventOfCode.AoC2017.Day21;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day22
{
    public class SporificaVirus
    {
        public Vector2 Direction { get; set; } = new(0, 1);
        public Vector2 Position { get; set; }
        public int Infections { get; set; }
        public List<Vector2> InfectedPositions { get; set; } = new();
        public enum CellCondition { Clean, Weakened, Infected, Flagged }
        public Dictionary<Vector2, Cell> Positions { get; set; } = new();
        public DirectionManager.Direction DirectionName { get; set; } = 0;

        public class Cell
        {
            public CellCondition CellCondition { get; set; }
        }

        public SporificaVirus(string input)
        {
            string[] rows = input.Split(Environment.NewLine);
            SetMap(rows);
        }

        private Cell GetCell(Vector2 position)
        {
            if (!Positions.ContainsKey(position)) Positions.Add(position, new() { CellCondition = CellCondition.Clean });
            return Positions[position];
        }

        public void Infect(int bursts)
        {
            for (int i = 0; i < bursts; i++)
            {
                if (InfectedPositions.Contains(Position))
                {
                    Direction = LinearTransformation.Rotate90DegreesClockwise.Transform(Direction);
                    InfectedPositions.Remove(Position);
                }
                else
                {
                    Direction = LinearTransformation.Rotate90DegreesCounterClockwise.Transform(Direction);
                    Infections++;
                    InfectedPositions.Add(Position);
                }

                Position += Direction;
            }
        }

        public void InfectEvolved(int bursts)
        {
            for (int i = 0; i < bursts; i++)
            {
                Cell cell = GetCell(Position);
                if (cell.CellCondition == CellCondition.Clean)
                {
                    DirectionName = DirectionManager.RotateLeft(DirectionName);
                    cell.CellCondition = CellCondition.Weakened;
                }
                else if (cell.CellCondition == CellCondition.Infected)
                {

                    cell.CellCondition = CellCondition.Flagged;
                    DirectionName = DirectionManager.RotateRight(DirectionName);
                }
                else if (cell.CellCondition == CellCondition.Flagged)
                {
                    cell.CellCondition = CellCondition.Clean;
                    DirectionName = DirectionManager.Reverse(DirectionName);
                }
                else if (cell.CellCondition == CellCondition.Weakened)
                {
                    Infections++;
                    cell.CellCondition = CellCondition.Infected;
                }

                Position += DirectionManager.Vectors[(int)DirectionName];
            }
        }

        public void SetMap(string[] rows)
        {
            for (int y = 0; y < rows.Length; y++)
                for (int x = 0; x < rows.Length; x++)
                    if (rows[rows.Length - y - 1][x] == '#')
                    {
                        InfectedPositions.Add(new(x - rows.Length / 2, y - rows.Length / 2));
                        GetCell(new(x - rows.Length / 2, y - rows.Length / 2)).CellCondition = CellCondition.Infected;
                    }
        }
    }

    public class DirectionManager
    {
        public enum Direction { Top, Right, Bottom, Left }
        public static Vector2[] Vectors { get; } = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };

        public static Direction RotateRight(Direction direction)
            => (Direction)((int)direction switch
            {
                0 => 1,
                1 => 2,
                2 => 3,
                3 => 0,
            });

        public static Direction RotateLeft(Direction direction)
            => (Direction)((int)direction switch
            {
                0 => 3,
                1 => 0,
                2 => 1,
                3 => 2,
            });

        public static Direction Reverse(Direction direction)
            => (Direction)((int)direction switch
            {
                0 => 2,
                1 => 3,
                2 => 0,
                3 => 1,
            });
    }
}