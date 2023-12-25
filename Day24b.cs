using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    internal class Day24b
    {
        const double TESTMAX = 400000000000000d;
        const double TESTMIN = 200000000000000d;
        //const double TESTMAX = 27d;
        //const double TESTMIN = 7d;
        public static void Run()
        {
            var lines = new List<Line>();
            var lineNum = 0;
            while (true)
            {
                lineNum++;
                var stringIn = Console.ReadLine();
                if (stringIn == "") { break; }

                var position = stringIn.Split('@')[0].Trim().Split(',');
                var gradient = stringIn.Split('@')[1].Trim().Split(',');

                lines.Add(new Line()
                {
                    origin = new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2])),
                    gradient = new Vector3(float.Parse(gradient[0]), float.Parse(gradient[1]), float.Parse(gradient[2])),
                    name = stringIn,
                });
            }

            // If 2 rocks have the same X, Y, or Z velocity, then the starting position must be a projection of their relative positions
            var parallel = lines.GroupBy(l => l.gradient);
            var parallelGroup = parallel.Where(g => g.Count() > 1);
            var sameVelocityX = lines.GroupBy(l => l.gradient.X);
            var xGroup = sameVelocityX.Where(g => g.Count() > 1);
            var sameVelocityY = lines.GroupBy(l => l.gradient.Y);
            var yGroup = sameVelocityY.Where(g => g.Count() > 1);
            var sameVelocityZ = lines.GroupBy(l => l.gradient.Z);
            var zGroup = sameVelocityZ.Where(g => g.Count() > 1);

            // Reddit solution is to inspect and narrow down velocities to integer values - I'm too lazy to do that

            // In theory, we now can find 2 planes, and find the line between 2 planes - and the rock must be thrown on that line
            var planes = new Dictionary<(int, int), Plane>();
            for (var i = 0; i < lines.Count; i++)
            {
                for (var j = i + 1; j < lines.Count; j++)
                {
                    var l1 = lines[i];
                    var l2 = lines[j];
                    var pbl = l1.PlaneBetweenLines(l2);
                    Console.WriteLine($"{l1.name} and {l2.name} form plane:{pbl}.");
                    planes.Add((i, j), pbl);
                }
            }

            var p1 = planes.First().Value;
            var p2 = planes.Last().Value;
            var p3 = planes.ElementAt(3).Value;
            var line1 = Vector3.Normalize(Vector3.Cross(p1.Normal, p2.Normal));
            var line2 = Vector3.Normalize(Vector3.Cross(p1.Normal, p3.Normal));
            var line3 = Vector3.Normalize(Vector3.Cross(p2.Normal, p3.Normal));
        }

        public class Line
        {
            public Vector3 origin;
            public Vector3 gradient;
            public string name;
            public Plane PlaneBetweenLines(Line line)
            {
                return Plane.CreateFromVertices(origin, line.origin, line.origin + 8*line.gradient);
            }

            public override string ToString()
            {
                return name;
            }
        }
    }
}
