using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day13
    {
        public static void Run()
        {
            var sum = 0;
            while (true)
            {
                var buttonAt = Console.ReadLine();
                var buttonBt = Console.ReadLine();
                var prizet = Console.ReadLine();

                var ax = Int32.Parse(buttonAt.Substring(buttonAt.IndexOf('X') + 2, buttonAt.IndexOf(',') - buttonAt.IndexOf('X') - 2));
                var ay = Int32.Parse(buttonAt.Substring(buttonAt.IndexOf('Y') + 2));
                var bx = Int32.Parse(buttonBt.Substring(buttonBt.IndexOf('X') + 2, buttonBt.IndexOf(',') - buttonBt.IndexOf('X') - 2));
                var by = Int32.Parse(buttonBt.Substring(buttonBt.IndexOf('Y') + 2));
                var px = Int32.Parse(prizet.Substring(prizet.IndexOf('X') + 2, prizet.IndexOf(',') - prizet.IndexOf('X') - 2));
                var py = Int32.Parse(prizet.Substring(prizet.IndexOf('Y') + 2));

                var minWay = FindMinWay((ax, ay), (bx, by), (px, py));
                if (minWay >= 0)
                {
                    sum += minWay;
                }
                Console.WriteLine($"Min way is {minWay}, sum:{sum}");

                var blank = Console.ReadLine();
                if (blank.Length > 0) { break; }
            }
        }

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
