using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2017.Day19
{
    public class DiagramNode
    {
        public List<Point> Directions { get; set; } = new();
        public char? Letter { get; set; }
    }

    public class Tubes
    {
        //public DiagramNode[,] Diagram { get; set; }
        private string[] Rows { get; set; }

        public Tubes(string input)
        {
            Rows = input.Split(Environment.NewLine);
        }

        public string SetDiagram()
        {
            Point startingPoint = new(Rows[0].IndexOf('|'), 0);
            return SetDiagramRecursive(startingPoint, new(0, 1));
        }

        public int GetSteps()
        {
            Point startingPoint = new(Rows[0].IndexOf('|'), 0);
            return StepsRecursive(startingPoint, new(0, 1));
        }

        private string SetDiagramRecursive(Point point, Point direction, string s = "")
        {
            for (point = new(point.X + direction.X, point.Y + direction.Y); IsOnDiagram(point); point.X += direction.X, point.Y += direction.Y)
            {
                char c = Rows[point.Y][point.X];
                if (c == ' ') break;
                if (char.IsLetter(c))
                {
                    s += c;
                }
                else if (c == '+')
                {
                    if (direction != new Point(0, -1) && RelativePoint(point, 0, 1)) direction = new(0, 1);
                    else if (direction != new Point(0, 1) && RelativePoint(point, 0, -1)) direction = new(0, -1);
                    else if (direction != new Point(-1, 0) && RelativePoint(point, 1, 0)) direction = new(1, 0);
                    else if (direction != new Point(1, 0) && RelativePoint(point, -1, 0)) direction = new(-1, 0);

                    return SetDiagramRecursive(point, direction, s);
                }
            }

            return s;
        }

        private int StepsRecursive(Point point, Point direction, int steps = 0)
        {
            for (point = new(point.X + direction.X, point.Y + direction.Y); IsOnDiagram(point); point.X += direction.X, point.Y += direction.Y)
            {
                steps++;
                char c = Rows[point.Y][point.X];
                if (c == ' ') break;

                //if (char.IsLetter(c)) steps += c;
                if (c == '+')
                {
                    if (direction != new Point(0, -1) && RelativePoint(point, 0, 1)) direction = new(0, 1);
                    else if (direction != new Point(0, 1) && RelativePoint(point, 0, -1)) direction = new(0, -1);
                    else if (direction != new Point(-1, 0) && RelativePoint(point, 1, 0)) direction = new(1, 0);
                    else if (direction != new Point(1, 0) && RelativePoint(point, -1, 0)) direction = new(-1, 0);

                    return StepsRecursive(point, direction, steps);
                }
            }

            return steps;
        }


        private bool RelativePoint(Point p, int relativeX, int relativeY)
        {
            Point relativePoint = new(p.X + relativeX, p.Y + relativeY);
            return IsOnDiagram(relativePoint) &&
                (relativeY != 0 && (Rows[relativePoint.Y][relativePoint.X] == '|' || char.IsLetter(Rows[relativePoint.Y][relativePoint.X])) ||
                relativeX != 0 && (Rows[relativePoint.Y][relativePoint.X] == '-' || char.IsLetter(Rows[relativePoint.Y][relativePoint.X])));
        }

        private bool IsOnDiagram(Point p)
            => p.X >= 0 && p.Y >= 0 && p.X < Rows[0].Length && p.Y < Rows.Length;

        public void RunthroughDiagram()
        {

        }
    }
}