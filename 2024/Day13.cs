using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day13
    {
        public static void Run()
        {
            var sum = 0L;
            while (true)
            {
                var buttonAt = Console.ReadLine();
                var buttonBt = Console.ReadLine();
                var prizet = Console.ReadLine();

                var ax = long.Parse(buttonAt.Substring(buttonAt.IndexOf('X') + 2, buttonAt.IndexOf(',') - buttonAt.IndexOf('X') - 2));
                var ay = long.Parse(buttonAt.Substring(buttonAt.IndexOf('Y') + 2));
                var bx = long.Parse(buttonBt.Substring(buttonBt.IndexOf('X') + 2, buttonBt.IndexOf(',') - buttonBt.IndexOf('X') - 2));
                var by = long.Parse(buttonBt.Substring(buttonBt.IndexOf('Y') + 2));
                var px = long.Parse(prizet.Substring(prizet.IndexOf('X') + 2, prizet.IndexOf(',') - prizet.IndexOf('X') - 2));
                var py = long.Parse(prizet.Substring(prizet.IndexOf('Y') + 2));

                var minWay = FindMinWay2((ax, ay), (bx, by), (px + 10000000000000L, py + 10000000000000L));
                //var minWay = FindMinWay2((ax, ay), (bx, by), (px, py));
                if (minWay >= 0)
                {
                    sum += minWay;
                }

                //sum:41388697173632 seems to be too low?!
                Console.WriteLine($"sum:{sum}");

                var blank = Console.ReadLine();
                if (blank.Length > 0) { break; }
            }
        }

        // Part 2
        private static long FindMinWay2((long x, long y) a, (long x, long y) b, (long x, long y) p)
        {
            var A = (double)(p.x * b.y - p.y * b.x) / (double)(a.x * b.y - a.y * b.x);
            var B = (double)(a.x * p.y - a.y * p.x) / (double)(a.x * b.y - a.y * b.x);

            if (A <= 0 && B <= 0) { return -1; }

            var aIsInt = false;
            var bIsInt = false;
            var retVal = -1L;
            // error tolerance
            if (Math.Abs(A - Math.Round(A)) < 0.0001)
            {
                A = Math.Round(A);
                aIsInt = true;
            }
            if (Math.Abs(B - Math.Round(B)) < 0.0001)
            {
                B = Math.Round(B);
                bIsInt = true;
            }

            if (aIsInt && bIsInt) { retVal = (long)(A * 3 + B); }

            // Only 2 solutions can exist - there's probably an analytical way
            // But we can binary search for it

            var gradientA = (double)a.y / (double)a.x;
            var gradientB = (double)b.y / (double)b.x;
            var gradientP = (double)p.y / (double)p.x;

            // Find the intersection of 2 lines
            // Project A from origin, project B from destination

            // Line for A: y = gradientA * x;
            // Line for B: y = gradientB * x + p.y - p.x/b.x * b.y

            // Intersecting point
            // gradientA * x = gradientB * x + p.y - p.x/b.x * b.y
            // gradientA * x - gradientB * x = p.y - p.x/b.x * b.y
            // (gradientA - gradientB) * x = p.y - p.x/b.x * b.y
            // x = (p.y - p.x/b.x * b.y) / (gradientA - gradientB)

            // Special case: lines are equal
            if (gradientA == gradientB)
            {
                throw new Exception("Lines are equal - if input actually has this, then calculate this.");
            }

            var xIntercept = ((double)p.y - (double)p.x / (double)b.x * (double)b.y) / (gradientA - gradientB);
            var yIntercept = gradientA * xIntercept;

            // error tolerance
            if (Math.Abs(xIntercept - Math.Round(xIntercept)) < 0.01)
            {
                xIntercept = Math.Round(xIntercept);
            }
            if (Math.Abs(yIntercept - Math.Round(yIntercept)) < 0.01)
            {
                yIntercept = Math.Round(yIntercept);
            }

            if (xIntercept < 0 || yIntercept < 0)
            {
                if (retVal != -1) { throw new Exception("Why did first method work??"); }
                return -1;
            }

            var xInterceptRemainder = xIntercept % a.x;
            var yInterceptRemainder = yIntercept % a.y;
            if (xIntercept % a.x == 0 && yIntercept % a.y == 0)
            {
                var bdx = (p.x - xIntercept, p.y - yIntercept);
                if (bdx.Item1 % b.x == 0 && bdx.Item2 % b.y == 0)
                {
                    var numB = bdx.Item1 / b.x;
                    var numA = xIntercept / a.x;

                    Console.WriteLine($"A:{numA}, B:{numB}");
                    return (long)(numA * 3 + numB);
                }
            }

            if (retVal != -1) { throw new Exception("Why did first method work??"); }
            return -1;
        }

        // Part 1 - BFS
        private static int FindMinWay((int x, int y) a, (int x, int y) b, (int x, int y) p)
        {
            var stack = new List<(int x, int y, int cost)>();
            var seen = new HashSet<(int x, int y)>();
            stack.Add((0, 0, 0));
            seen.Add((0, 0));

            while (stack.Count > 0)
            {
                var current = stack.First();
                stack.RemoveAt(0);

                // Press button A
                var pressA = (current.x + a.x, current.y + a.y, current.cost + 3);

                // Press button B
                var pressB = (current.x + b.x, current.y + b.y, current.cost + 1);

                foreach (var pr in new[]{ pressA, pressB })
                {
                    if ((pr.Item1, pr.Item2) == p)
                    {
                        return pr.Item3;
                    }

                    if (!seen.Contains((pr.Item1, pr.Item2))
                        && pr.Item1 <= p.x
                        && pr.Item2 <= p.y)
                    {
                        seen.Add((pr.Item1, pr.Item2));
                        stack.Add(pr);
                    }
                }

                stack.Sort((p1, p2) => p1.cost - p2.cost);
            }

            return -1;
        }
    }
}
