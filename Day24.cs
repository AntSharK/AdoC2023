using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day24
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
                    origin = (double.Parse(position[0]), double.Parse(position[1]), double.Parse(position[2])),
                    gradient = (double.Parse(gradient[0]), double.Parse(gradient[1]), double.Parse(gradient[2])),
                    name = lineNum.ToString(),
                });
            }

            foreach (var line in lines)
            {
                line.Solve2d();
            }

            var countIntersect = 0;
            var nonIntersect = 0;
            var intersectInPast = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                for (var j = i + 1; j < lines.Count; j++)
                {
                    var l1 = lines[i];
                    var l2 = lines[j];
                    var intersection = l1.Intersect2d(l2);

                    if (intersection.Item3 < 0 || intersection.Item4 < 0)
                    {
                        Console.WriteLine($"PAST - {l1.name} and {l2.name} intersect at ({intersection.Item1},{intersection.Item2}) at t1={intersection.Item3}, t2={intersection.Item4}.");
                        intersectInPast++;
                    }
                    else if (intersection.Item1 >= TESTMIN 
                        && intersection.Item1 <= TESTMAX
                        && intersection.Item2 >= TESTMIN
                        && intersection.Item2 <= TESTMAX)
                    {
                        Console.WriteLine($"YES - {l1.name} and {l2.name} intersect at ({intersection.Item1},{intersection.Item2}) at t1={intersection.Item3}, t2={intersection.Item4}.");
                        countIntersect++;
                    }
                    else
                    {
                        if (intersection == (0, 0, -1, -1))
                        {
                            Console.WriteLine($"PARALLEL - {l1.name} and {l2.name} are parallel.");
                        }
                        else
                        {
                            Console.WriteLine($"NO - {l1.name} and {l2.name} intersect at ({intersection.Item1},{intersection.Item2}) at t1={intersection.Item3}, t2={intersection.Item4}.");
                        }
                        nonIntersect++;
                    }
                }
            }

            // 26355 is too high (I forgot to count past intersections)
            // 20355 is too high (I typoed)
            // 20335 is too low (why?)
            // 20336 is the right answer - BUT I DON'T KNOW WHAT I MISSED
            Console.WriteLine($"Intersections: {countIntersect}, Non-intersections: {nonIntersect}, Past-intersections: {intersectInPast}.");
        }

        public class Line
        {
            public (double, double, double) origin;
            public (double, double, double) gradient;
            public string name;

            public double gradient2d;
            public double constant2d;
            public void Solve2d()
            {
                // y = ax + b
                this.gradient2d = gradient.Item2 / gradient.Item1;
                this.constant2d = origin.Item2 - this.gradient2d * origin.Item1;
            }

            public (double, double, double, double) Intersect2d(Line line)
            {
                // Assume lines are not identical. Check for parallel
                if (Math.Abs(line.gradient2d) == Math.Abs(this.gradient2d))
                {
                    return (0, 0, -1, -1); // Not within bounds, so this is ok
                }

                var intersectX = (line.constant2d - this.constant2d) / (this.gradient2d - line.gradient2d);
                var intersectY = this.gradient2d * intersectX + this.constant2d;

                var intersectTime1 = (intersectX - origin.Item1) / gradient.Item1;
                var intersectTime2 = (intersectX - line.origin.Item1) / line.gradient.Item1;

                return (intersectX, intersectY, intersectTime1, intersectTime2);
            }
        }
    }
}
