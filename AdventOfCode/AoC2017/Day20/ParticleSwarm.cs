using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.AoC2017.Day20
{
    public class Particle
    {
        public int Id { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 Position { get; set; }
    }

    public class Intersection
    {
        public Particle ParticleA { get; set; }
        public Particle ParticleB { get; set; }
        public double IntersectionTime { get; set; }
    }

    public class ParticleSwarm
    {
        public List<Particle> Particles { get; set; } = new();

        public ParticleSwarm(string input)
        {
            input = input.Replace(">", null)
            .Replace("p=<", null)
            .Replace("v=<", null)
            .Replace("a=<", null);

            int id = 0;
            foreach (string row in input.Split(Environment.NewLine))
            {
                string[] data = row.Split(", ");
                Particle particle = new() { Id = id };
                particle.Position = GetVector3(data[0]);
                particle.Velocity = GetVector3(data[1]);
                particle.Acceleration = GetVector3(data[2]);
                id++;
                Particles.Add(particle);
            }
        }

        private static Vector3 GetVector3(string values)
        {
            string[] data = values.Split(',');
            Vector3 vector3 = new();
            vector3.X = int.Parse(data[0]);
            vector3.Y = int.Parse(data[1]);
            vector3.Z = int.Parse(data[2]);
            return vector3;
        }

        public int NearestParticle()
        {
            Particle nearestParticle = Particles[0];
            foreach (var particle in Particles)
                if (particle.Acceleration.Length() < nearestParticle.Acceleration.Length()) nearestParticle = particle;

            return nearestParticle.Id;
        }

        public int ParticlesCountAfterIntersections()
        {
            /*
            List<Intersection> intersections = new();
            for (int i = 0; i < Particles.Count; i++)
            {
                Particle particle = Particles[i];
                for (int k = 0; k < Particles.Count; k++)
                {
                    if (k == i) continue;
                    Particle otherParticle = Particles[k];
                    Intersection intersection = CalculateIntersections(particle, otherParticle);
                    if (intersection != null) intersections.Add(intersection);
                }
            }
            
            intersections.Sort((x, y) => x.IntersectionTime < y.IntersectionTime ? -1 : 1);
            
            List<Particle> possibleParticles = new();
            foreach (Intersection intersection in intersections)
            {
                if (!possibleParticles.Contains(intersection.ParticleA)) possibleParticles.Add(intersection.ParticleA);
                if (!possibleParticles.Contains(intersection.ParticleB)) possibleParticles.Add(intersection.ParticleB);
            }
*/
            for (int tick = 0; tick < 10000; tick++)
            {
                foreach (Particle particle in Particles)
                {
                    particle.Velocity += particle.Acceleration;
                    particle.Position += particle.Velocity;
                }

                for (int i = 0; i < Particles.Count; i++)
                {
                    var particle = Particles[i];
                    var intersects = Particles.Where(p => p.Position == particle.Position).ToList();
                    if (intersects.Count() > 1) foreach (Particle p in intersects) Particles.Remove(p);
                }
            }

            return Particles.Count;
        }

        public Intersection CalculateIntersections(Particle a, Particle b)
        {
            Vector3 t1;
            Vector3 t2;

            //Matrix4x4 matrix = new();

            //matrix.Translation = a.Position;

            //Vector3 crossA = Vector3.Normalize(Vector3.Cross(a.Acceleration, a.Velocity));

            //  Vector3 crossB = Vector3.Normalize(Vector3.Cross(b.Acceleration, b.Velocity));

            //Console.WriteLine(crossB.Length());
            //if (Vector3.Distance(crossA, crossB) < 0.1) Console.WriteLine(crossA + " - " + crossB);// return null;
            // else return null;

            if (!SecondGradeEquation(a.Position.X - b.Position.X, a.Velocity.X - b.Velocity.X, (a.Acceleration.X - b.Acceleration.X) / 2, out t1.X, out t2.X)) return null;
            if (!SecondGradeEquation(a.Position.Y - b.Position.Y, a.Velocity.Y - b.Velocity.Y, (a.Acceleration.Y - b.Acceleration.Y) / 2, out t1.Y, out t2.Y)) return null;
            if (!SecondGradeEquation(a.Position.Z - b.Position.Z, a.Velocity.Z - b.Velocity.Z, (a.Acceleration.Z - b.Acceleration.Z) / 2, out t1.Z, out t2.Z)) return null;

            /*
            t1.X = MathF.Round(t1.X, 4);
            t1.Y = MathF.Round(t1.Y, 4);
            t1.Z = MathF.Round(t1.Z, 4);
            t2.X = MathF.Round(t2.X, 4);
            t2.Y = MathF.Round(t2.Y, 4);
            t2.Z = MathF.Round(t2.Z, 4);
            */

            Intersection intersection = new();
            float threshold = 0.001f;
            if (t1.X > 0 && Math.Abs(t1.X - t1.Y) < threshold && Math.Abs(t1.X - t1.Z) < threshold) intersection.IntersectionTime = t1.X;
            else if (t2.X > 0 && Math.Abs(t2.X - t1.Y) < threshold && Math.Abs(t2.X - t1.Z) < threshold) intersection.IntersectionTime = t2.X;
            else return null;

            intersection.ParticleA = a;
            intersection.ParticleB = b;

            return intersection;
        }

        private static bool SecondGradeEquation(float a, float b, float c, out float x1, out float x2)
        {
            if (a == 0)
            {
                if (c == 0)
                {
                    x1 = x2 = float.NaN;
                    return false;
                }
                x1 = x2 = -b / c;
                return true;
            }

            float delta = b * b - 4 * a * c;
            if (delta >= 0)
            {
                x1 = (-b - MathF.Sqrt(delta)) / (2 * a);
                x2 = (-b + MathF.Sqrt(delta)) / (2 * a);
                return true;
            }
            else
            {
                x1 = x2 = float.NaN;
                return false;
            }
        }
    }
}