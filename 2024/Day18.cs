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
                // Part 1 - only first 1024 things
                if (points.Count == 1024) { break; }
            }

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
            var minCost = -1;
            while (explore.Count > 0)
            {
                var nextNode = explore[0];
                explore.RemoveAt(0);
                if ((nextNode.x, nextNode.y) == (SPACE, SPACE))
                {
                    minCost = nextNode.path;
                    break;
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

            Console.WriteLine(minCost);
        }
    }
}
