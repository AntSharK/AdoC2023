using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day18
    {
        public static void Run()
        {
            var points = new List<(int x, int y)>();
            while (true)
            {
                var line = Console.ReadLine();
                if (line.Length > 0)
                {
                    var a = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var x = Int32.Parse(a[0]);
                    var y = Int32.Parse(a[1]);
                    points.Add((x, y));
                }
                else { break; }
            }

            // I guess we binary search (with a bias for solving?)
            var lower = 1024;
            var higher = points.Count;

            // Some kind of binary search - we don't even need to do it properly
            /*
            while (lower < higher)
            {
                var midpoint = (lower + higher) / 2;
                Console.WriteLine($"Exploring {midpoint}.");
                var minPath = FindPath(points.GetRange(0, midpoint));
                Console.WriteLine($"MinPathLength for {midpoint} is {minPath}.");
                if (minPath > 0)
                {
                    lower = (lower + midpoint) / 2;
                }
                else
                {
                    higher = (midpoint + higher) / 2;
                }
            }

            Console.WriteLine($"Lower:{lower}, Higher:{higher}");
            */
            // Solution is somewhere below 2933 - we didn't bother with off by one
            Console.WriteLine($"2933 - {FindPath(points.GetRange(0, 2933))} - {points[2933]}");
            Console.WriteLine($"2934 - {FindPath(points.GetRange(0, 2934))} - {points[2934]}");
        }

        public static int FindPath(List<(int x, int y)> points)
        {
            Console.WriteLine("Calculating");
            const int SPACE = 70;
            var map = new bool[SPACE + 1, SPACE + 1];
            foreach (var p in points)
            {
                map[p.x, p.y] = true;
            }

            var encountered = new HashSet<(int x, int y)>();
            var explore = new List<(int x, int y, int path)>();
            explore.Add((0, 0, 0));

            var dirs = new List<(int x, int y)>() { (0, 1), (1, 0), (0, -1), (-1, 0) };
            while (explore.Count > 0)
            {
                var nextNode = explore[0];
                explore.RemoveAt(0);
                if ((nextNode.x, nextNode.y) == (SPACE, SPACE))
                {
                    return nextNode.path;
                }

                foreach (var d in dirs)
                {
                    try
                    {
                        var blocked = map[nextNode.x + d.x, nextNode.y + d.y];
                        if (blocked) continue;

                        if (!encountered.Contains((nextNode.x + d.x, nextNode.y + d.y)))
                        {
                            explore.Add((nextNode.x + d.x, nextNode.y + d.y, nextNode.path + 1));
                            encountered.Add((nextNode.x + d.x, nextNode.y + d.y));
                        }
                    }
                    catch { continue; }
                }
            }

            return -1;
        }
    }
}
