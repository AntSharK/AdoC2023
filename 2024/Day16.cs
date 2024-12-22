using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day16
    {
        public static void Run()
        {
            var line = "dummy";
            var inLines = new List<string>();
            while (line != "")
            {
                line = Console.ReadLine();
                if (line.Length > 0)
                {
                    inLines.Add(line);
                }
            }

            var map = new char[inLines[0].Length, inLines.Count];
            var start = (-1, -1);
            var end = (-1, -1);

            for (var y = 0; y < inLines.Count; y++)
            {
                var tca = inLines[y].ToCharArray();
                for (var x = 0; x < tca.Length; x++)
                {
                    map[x, y] = tca[x];
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

            foreach (var path in FindPaths(map, start, end))
            {
                Console.WriteLine($"Cost: {path.Item1}, Path: {path.Item2}");
                // Mark occupied nodes

                foreach (var point in path.Item2.Split('|', StringSplitOptions.RemoveEmptyEntries))
                {
                    var ps = point.Split(',');
                    map[Int32.Parse(ps[0]), Int32.Parse(ps[1])] = 'O';
                }
            }

            Print(map);
        }

        public static void Print(char[,] map)
        {
            var count = 0;
            var sb = new StringBuilder();
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (var x = 0; x < map.GetLength(0); x++)
                {
                    sb.Append(map[x, y]);
                    if (map[x,y] == 'O')
                    {
                        count++;
                    }
                }
                sb.Append('\n');
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine($"Occupied: {count}");
        }

        public static List<(int, string)> FindPaths(char[,] map, (int, int) start, (int, int) end)
        {
            var minCostToDest = -1;
            var retVal = new List<(int, string)>();

            var directions = new List<(int, int)>()
            {
                (1, 0), (0, 1), (-1, 0), (0, -1)
            };

            var startDir = 0; // 0 meaning facing east

            var encountered = new HashSet<(int x, int y, int facing)>();
            var lowestCostPathToPoint = new Dictionary<(int x, int y, int facing), int>();
            var pathsToPoint = new Dictionary<(int x, int y, int facing), List<string>>();

            var stack = new List<(int x, int y, int cost, int facing, string currentPath)>();

            stack.Add((start.Item1, start.Item2, 0, 0, $"{start.Item1},{start.Item2}"));
            encountered.Add((start.Item1, start.Item2, 0));
            lowestCostPathToPoint[(start.Item1, start.Item2, 0)] = 0;
            pathsToPoint[(start.Item1, start.Item2, 0)] = new List<string>() { "" };

            while (stack.Count > 0)
            {
                var nextNode = stack.First();
                if (minCostToDest > 0 && nextNode.cost > minCostToDest) { break; }

                stack.RemoveAt(0);

                //Console.WriteLine($"Exploring: ({nextNode.x},{nextNode.y}), Cost:{nextNode.cost},Facing:{nextNode.facing},Path:{nextNode.currentPath}");
                Console.WriteLine($"Exploring: ({nextNode.x},{nextNode.y}), Cost:{nextNode.cost}");
                if ((nextNode.x, nextNode.y) == end)
                {
                    if (minCostToDest < 0)
                    {
                        minCostToDest = nextNode.cost;
                    }

                    retVal.Add((nextNode.cost, nextNode.currentPath));
                }

                // Add in front
                var curDir = directions[nextNode.facing];
                var rightFace = (nextNode.facing + 1) % 4;
                var rightDir = directions[rightFace];
                var leftFace = (nextNode.facing + 3) % 4;
                var leftDir = directions[leftFace];
                var frontSpace = (nextNode.x + curDir.Item1, nextNode.y + curDir.Item2);
                var rightSpace = (nextNode.x + rightDir.Item1, nextNode.y + rightDir.Item2);
                var leftSpace = (nextNode.x + leftDir.Item1, nextNode.y + leftDir.Item2);

                var spaceDirCost = new List<((int x, int y) space, int dir, int cost)>
                {
                    (frontSpace, nextNode.facing, nextNode.cost + 1),
                    (rightSpace, rightFace, nextNode.cost + 1001),
                    (leftSpace, leftFace, nextNode.cost + 1001),
                };

                foreach (var sdc in spaceDirCost)
                {
                    if (map[sdc.space.x, sdc.space.y] == '.')
                    {
                        var newPath = nextNode.currentPath + $"|{sdc.space.x},{sdc.space.y}";
                        var nextEntry = (sdc.space.x, sdc.space.y, sdc.dir);
                        if (!encountered.Contains(nextEntry))
                        {
                            if (!lowestCostPathToPoint.ContainsKey(nextEntry))
                            {
                                lowestCostPathToPoint[nextEntry] = sdc.cost;
                                pathsToPoint[nextEntry] = new List<string> { newPath };
                            }

                            var lowestCostPath = lowestCostPathToPoint[nextEntry];
                            if (lowestCostPath <= sdc.cost)
                            {
                                stack.Add((sdc.space.x, sdc.space.y, sdc.cost, sdc.dir, newPath));
                                encountered.Add(nextEntry);
                            }
                        }
                    }
                }

                // Evaluate from lowest cost
                stack.Sort((x, y) => x.cost - y.cost);
            }

            return retVal;
        }
    }
}
