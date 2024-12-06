using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day6
    {
        public static void Run()
        {
            var line = "dummy";
            var lines = new List<string>();
            while (true)
            {
                line = Console.ReadLine();
                if (line.Length <= 1)
                {
                    break;
                }

                lines.Add(line);
            }

            var map = new char[lines[0].Length, lines.Count];
            for(var y = 0; y < lines.Count; y++)
            {
                var k = lines[y].ToCharArray();
                for (var x = 0; x < k.Length; x++)
                {
                    map[x, y] = k[x];
                }
            }

            var guardLoc = (0, 0);
            var curDir = (0, -1);
            for (var x = 0; x < lines[0].Length; x++)
            {
                for (var y = 0; y < lines.Count; y++)
                {
                    if (map[x, y] == '^')
                    {
                        guardLoc = (x, y);
                        break;
                    }
                }
            }

            var hasNoPath = 0;
            for (var x1 = 0; x1 < lines[0].Length; x1++)
            {
                for (var y1 = 0; y1 < lines.Count; y1++)
                {
                    if (map[x1, y1] != '.')
                    {
                        continue;
                    }

                    map[x1, y1] = '#';
                    if (!HasPathOut(guardLoc, curDir, map, lines))
                    {
                        hasNoPath++;
                    }

                    // Reset the map
                    for (var x = 0; x < lines[0].Length; x++)
                    {
                        for (var y = 0; y < lines.Count; y++)
                        {
                            if (map[x, y] == 'X')
                            {
                                map[x, y] = '.';
                            }
                        }
                    }
                    map[x1, y1] = '.';
                }
            }

            Console.WriteLine(hasNoPath);
        }

        public static bool HasPathOut((int, int) guardLoc, (int, int) curDir, char[,] map, List<string> lines)
        {
            var encounteredNodes = new HashSet<(int, int, int, int)>();
            while (true)
            {
                if (encounteredNodes.Contains((guardLoc.Item1, guardLoc.Item2, curDir.Item1, curDir.Item2)))
                    { return false; }

                encounteredNodes.Add((guardLoc.Item1, guardLoc.Item2, curDir.Item1, curDir.Item2));

                map[guardLoc.Item1, guardLoc.Item2] = 'X';
                var nextLoc = (guardLoc.Item1 + curDir.Item1, guardLoc.Item2 + curDir.Item2);

                try
                {
                    var nextChar = map[nextLoc.Item1, nextLoc.Item2];
                    if (nextChar != '#')
                    {
                        guardLoc = nextLoc;
                    }
                    else if (nextChar == '#')
                    {
                        curDir = (-curDir.Item2, curDir.Item1);
                    }
                }
                // Array out of bounds exception means this is over
                catch
                {
                    break;
                }
            }

            /*
            var sum = 0;
            for (var x = 0; x < lines[0].Length; x++)
            {
                for (var y = 0; y < lines.Count; y++)
                {
                    if (map[x, y] == 'X')
                    {
                        sum++;
                    }
                }
            }

            Console.WriteLine(sum);*/
            return true;
        }
    }
}
