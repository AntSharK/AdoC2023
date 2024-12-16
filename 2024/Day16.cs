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

            (var minCost, var path) = FindPath(map, start, end);
            Console.WriteLine($"MinCost:{minCost}, Path:{path}");
        }

        public static (int, string) FindPath(char[,] map, (int, int) start, (int, int) end)
        {
            var directions = new List<(int, int)>()
            {
                (1, 0), (0, 1), (-1, 0), (0, -1)
            };

            var startDir = 0; // 0 meaning facing east

            var encountered = new HashSet<(int x, int y, int facing)>();
            var stack = new List<(int x, int y, int cost, int facing, string currentPath)>();

            stack.Add((start.Item1, start.Item2, 0, 0, ""));
            encountered.Add((start.Item1, start.Item2, 0));

            while (stack.Count > 0)
            {
                var nextNode = stack.First();
                stack.RemoveAt(0);

                Console.WriteLine($"Exploring: ({nextNode.x},{nextNode.y}), Cost:{nextNode.cost},Facing:{nextNode.facing},Path:{nextNode.currentPath}");
                if ((nextNode.x, nextNode.y) == end)
                {
                    return (nextNode.cost, nextNode.currentPath);
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
                    if (!encountered.Contains((frontSpace.Item1, frontSpace.Item2, nextNode.facing)))
                    {
                        stack.Add((frontSpace.Item1, frontSpace.Item2, nextNode.cost + 1, nextNode.facing, nextNode.currentPath + "."));
                        encountered.Add((frontSpace.Item1, frontSpace.Item2, nextNode.facing));
                    }
                }

                // Add right
                var rightSpace = (nextNode.x + rightDir.Item1, nextNode.y + rightDir.Item2);
                if (map[rightSpace.Item1, rightSpace.Item2] == '.')
                {
                    if (!encountered.Contains((rightSpace.Item1, rightSpace.Item2, rightFace)))
                    {
                        stack.Add((rightSpace.Item1, rightSpace.Item2, nextNode.cost + 1001, rightFace, nextNode.currentPath + "R"));
                        encountered.Add((rightSpace.Item1, rightSpace.Item2, rightFace));
                    }
                }

                // Add left
                var leftSpace = (nextNode.x + leftDir.Item1, nextNode.y + leftDir.Item2);
                if (map[leftSpace.Item1, leftSpace.Item2] == '.')
                {
                    if (!encountered.Contains((leftSpace.Item1, leftSpace.Item2, leftFace)))
                    {
                        stack.Add((leftSpace.Item1, leftSpace.Item2, nextNode.cost + 1001, leftFace, nextNode.currentPath + "L"));
                        encountered.Add((leftSpace.Item1, leftSpace.Item2, leftFace));
                    }
                }

                // Evaluate from lowest cost
                stack.Sort((x, y) => x.cost - y.cost);
            }

            return (-1, "");
        }
    }
}
