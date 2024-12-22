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

            var encountered = new HashSet<(int x, int y, int facing, string pathTo)>();
            var lowestCostPathToPoint = new Dictionary<(int x, int y, int facing), int>();

            var stack = new List<(int x, int y, int cost, int facing, string currentPath)>();

            stack.Add((start.Item1, start.Item2, 0, 0, $"{start.Item1},{start.Item2}"));
            encountered.Add((start.Item1, start.Item2, 0, $"{start.Item1},{start.Item2}"));
            lowestCostPathToPoint[(start.Item1, start.Item2, 0)] = 0;

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
                if (map[frontSpace.Item1, frontSpace.Item2] == '.')
                {
                    var newPath = nextNode.currentPath + $"|{frontSpace.Item1},{frontSpace.Item2}";
                    var newCost = nextNode.cost + 1;
                    if (!encountered.Contains((frontSpace.Item1, frontSpace.Item2, nextNode.facing, newPath)))
                    {
                        if (!lowestCostPathToPoint.ContainsKey((frontSpace.Item1, frontSpace.Item2, nextNode.facing)))
                        {
                            lowestCostPathToPoint[(frontSpace.Item1, frontSpace.Item2, nextNode.facing)] = newCost;
                        }

                        var lowestCostPath = lowestCostPathToPoint[(frontSpace.Item1, frontSpace.Item2, nextNode.facing)];
                        if (lowestCostPath <= newCost)
                        {
                            stack.Add((frontSpace.Item1, frontSpace.Item2, newCost, nextNode.facing, newPath));
                            encountered.Add((frontSpace.Item1, frontSpace.Item2, nextNode.facing, newPath));
                        }
                    }
                }

                // Add right
                var rightSpace = (nextNode.x + rightDir.Item1, nextNode.y + rightDir.Item2);
                if (map[rightSpace.Item1, rightSpace.Item2] == '.')
                {
                    var newPath = nextNode.currentPath + $"|{rightSpace.Item1},{rightSpace.Item2}";
                    var newCost = nextNode.cost + 1001;
                    if (!encountered.Contains((rightSpace.Item1, rightSpace.Item2, rightFace, newPath)))
                    {
                        if (!lowestCostPathToPoint.ContainsKey((rightSpace.Item1, rightSpace.Item2, rightFace)))
                        {
                            lowestCostPathToPoint[(rightSpace.Item1, rightSpace.Item2, rightFace)] = newCost;
                        }

                        var lowestCostPath = lowestCostPathToPoint[(rightSpace.Item1, rightSpace.Item2, rightFace)];
                        if (lowestCostPath <= newCost)
                        {
                            stack.Add((rightSpace.Item1, rightSpace.Item2, newCost, rightFace, newPath));
                            encountered.Add((rightSpace.Item1, rightSpace.Item2, rightFace, newPath));
                        }
                    }
                }

                // Add left
                var leftSpace = (nextNode.x + leftDir.Item1, nextNode.y + leftDir.Item2);
                if (map[leftSpace.Item1, leftSpace.Item2] == '.')
                {
                    var newPath = nextNode.currentPath + $"|{leftSpace.Item1},{leftSpace.Item2}";
                    var newCost = nextNode.cost + 1001;
                    if (!encountered.Contains((leftSpace.Item1, leftSpace.Item2, leftFace, newPath)))
                    {
                        if (!lowestCostPathToPoint.ContainsKey((leftSpace.Item1, leftSpace.Item2, leftFace)))
                        {
                            lowestCostPathToPoint[(leftSpace.Item1, leftSpace.Item2, leftFace)] = newCost;
                        }

                        var lowestCostPath = lowestCostPathToPoint[(leftSpace.Item1, leftSpace.Item2, leftFace)];
                        if (lowestCostPath <= newCost)
                        {
                            stack.Add((leftSpace.Item1, leftSpace.Item2, newCost, leftFace, newPath));
                            encountered.Add((leftSpace.Item1, leftSpace.Item2, leftFace, newPath));
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
