using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day20
    {
        public static void Run()
        {
            var lines = new List<string>();
            while (true)
            {
                var a = Console.ReadLine();
                if (a == "") { break; }
                lines.Add(a);
            }

            var start = (-1, -1);
            var end = (-1, -1);
            var map = new char[lines[0].Length, lines.Count];
            for (var y = 0; y < lines.Count; y++)
            {
                var ca = lines[y].ToCharArray();
                for (var x = 0; x < ca.Length; x++)
                {
                    map[x,y] = ca[x];

                    if (map[x, y] == 'S')
                    {
                        start = (x, y);
                        map[x, y] = '.';
                    }

                    if (map[x, y] == 'E')
                    {
                        end = (x, y);
                        map[x, y] = '.';
                    }
                }
            }

            var shortestPathNoCheat = ShortestPathLength(map, start, end, Int32.MaxValue);
            Console.WriteLine($"No cheat length: {shortestPathNoCheat}");

            var legalLength = shortestPathNoCheat - 99;
            var possibleCheats = PathsWithCheat(map, start, end, legalLength);

            // Sort - just to double check input
            possibleCheats.Sort();
            var count = possibleCheats.Count(c => c.length < legalLength);
            Console.WriteLine($"Cheats saving: {count}");
        }

        public static List<(int length, int startx, int starty, int endx, int endy)> PathsWithCheat(char[,] map, (int x, int y) start, (int x, int y) end, int lengthLimit)
        {
            var retVal = new List<(int length, int startx, int starty, int endx, int endy)>();

            // Find all blocks reachable within 'lengthlimit' steps
            var reachable = new Dictionary<(int x, int y), int>();
            var toVisit = new List<(int x, int y, int currentLength)>();
            var dir = new List<(int x, int y)>() { (0, 1), (1, 0), (0, -1), (-1, 0) };

            reachable.Add(start, 0);
            toVisit.Add((start.x, start.y, 0));

            while (toVisit.Count > 0)
            {
                var cur = toVisit[0];
                toVisit.RemoveAt(0);
                foreach (var d in dir)
                {
                    try
                    {
                        var nextGrid = (cur.x + d.x, cur.y + d.y);
                        if (reachable.ContainsKey(nextGrid)) { continue; }

                        // Impossible to reach given current restraints
                        var distanceFromEnd = Math.Abs(nextGrid.Item1 - end.x) + Math.Abs(nextGrid.Item2 - end.y);
                        if (distanceFromEnd + cur.currentLength > lengthLimit) { continue; }

                        var charAt = map[nextGrid.Item1, nextGrid.Item2];
                        if (charAt != '.') { continue; }

                        toVisit.Add((nextGrid.Item1, nextGrid.Item2, cur.currentLength + 1));
                        reachable[nextGrid] = cur.currentLength + 1;
                    }
                    // Do nothing - out of bounds
                    catch { }
                }
            }

            Console.WriteLine($"Found {reachable.Count} points reachable.");
            var pointsAnalyzed = 0;

            // For each of these points, find a cheat
            var distanceToEnd = new Dictionary<(int x, int y), int>();
            distanceToEnd[start] = lengthLimit;
            distanceToEnd[end] = 0;
            var dirCheat = new List<(int x, int y)>() { (0, 2), (2, 0), (0, -2), (-2, 0) };
            foreach (var r in reachable)
            {
                Console.WriteLine($"Progress:{pointsAnalyzed++}");
                var point = r.Key;
                var distanceFromStart = r.Value;
                var depthLimit = lengthLimit - distanceFromStart - 2;

                foreach (var d in dirCheat)
                {
                    try
                    {
                        var nextGrid = (point.x + d.x, point.y + d.y);
                        var charAt = map[nextGrid.Item1, nextGrid.Item2];
                        if (charAt != '.') { continue; }

                        if (distanceToEnd.ContainsKey(nextGrid))
                        {
                            if (distanceToEnd[nextGrid] < 0) { continue; }

                            var totalDistance = distanceFromStart + distanceToEnd[nextGrid] + 2;
                            retVal.Add((totalDistance, point.x, point.y, nextGrid.Item1, nextGrid.Item2));
                            continue;
                        }

                        var distanceFromPoint = ShortestPathLength(map, nextGrid, end, depthLimit);
                        if (distanceFromPoint > 0)
                        {
                            var totalDistance = distanceFromStart + distanceFromPoint + 2;
                            retVal.Add((totalDistance, point.x, point.y, nextGrid.Item1, nextGrid.Item2));
                            distanceToEnd[nextGrid] = distanceFromPoint;
                        }
                    }
                    // Do nothing - out of bounds
                    catch { }
                }
            }

            return retVal;
        }

        public static int ShortestPathLength(char[,] map, (int x, int y) start, (int x, int y) end, int depthLimit)
        {
            var visited = new HashSet<(int x, int y)>();

            var toVisit = new List<(int x, int y, int currentLength)>();
            toVisit.Add((start.x, start.y, 0));

            var dir = new List<(int x, int y)>() { (0, 1), (1, 0), (0, -1), (-1, 0) };

            while (toVisit.Count > 0)
            {
                var cur = toVisit[0];

                if (cur.currentLength > depthLimit) { return -1; }
                if ((cur.x, cur.y) == end) { return cur.currentLength; }

                toVisit.RemoveAt(0);
                foreach (var d in dir)
                {
                    try
                    {
                        var nextGrid = (cur.x + d.x, cur.y + d.y);
                        if (visited.Contains(nextGrid)) { continue; }

                        var charAt = map[nextGrid.Item1, nextGrid.Item2];
                        if (charAt != '.') { continue; }

                        toVisit.Add((nextGrid.Item1, nextGrid.Item2, cur.currentLength + 1));
                        visited.Add(nextGrid);
                    }
                    // Do nothing - out of bounds
                    catch { }
                }
            }

            return -1;
        }
    }
}
