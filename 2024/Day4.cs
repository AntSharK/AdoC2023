using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day4
    {
        public static void Run()
        {
            string line = "dummy";
            var lines = new List<string>();
            while (line != "")
            {
                line = Console.ReadLine();
                if (line.Length > 1)
                {
                    lines.Add(line);
                }
            }

            var grid = new char[lines.Count, lines[0].Count()];
            for (var i = 0; i < lines.Count; i++)
            {
                var dex = 0;
                foreach (var c in lines[i])
                {
                    grid[i, dex] = c;
                    dex++;
                }
            }

            // Part 1
            /*var directions = new List<(int, int)>()
            {
                (1, 0),
                (1, 1),
                (1, -1),
                (-1, 0),
                (-1, 1),
                (-1, -1),
                (0, 1),
                (0, -1)
            };

            var count = 0;
            for(var i = 0; i < lines.Count; i++)
            {
                for (var j = 0; j < lines[0].Count(); j++)
                {
                    foreach (var dir in directions)
                    {
                        // Screw out of bounds - just try/catch
                        try
                        {
                            var c1 = grid[i, j];
                            var c2 = grid[i + dir.Item1, j + dir.Item2];
                            var c3 = grid[i + dir.Item1 * 2, j + dir.Item2 * 2];
                            var c4 = grid[i + dir.Item1 * 3, j + dir.Item2 * 3];

                            if (c1 == 'X' && c2 == 'M' && c3 == 'A' && c4 == 'S')
                            {
                                count++;
                            }
                        }
                        catch(Exception ex) { continue; }
                    }
                }
            }*/

            // Part 2
            var count = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                for (var j = 0; j < lines[0].Count(); j++)
                {
                    if (grid[i, j] == 'A')
                    {
                        // Screw out of bounds - just try/catch
                        try
                        {
                            var topleft = grid[i - 1, j - 1];
                            var topright = grid[i + 1, j - 1];
                            var bottomleft = grid[i - 1, j + 1];
                            var bottomright = grid[i + 1, j + 1];

                            var criss = false;
                            var cross = false;
                            if ((topleft == 'M' && bottomright == 'S') ||
                                (topleft == 'S' && bottomright == 'M'))
                            {
                                criss = true;
                            }

                            if ((topright == 'M' && bottomleft == 'S') ||
                                (topright == 'S' && bottomleft == 'M'))
                            {
                                cross = true;
                            }

                            if (criss && cross)
                            {
                                count++;
                            }
                        }
                        catch (Exception ex) { continue; }
                    }
                }
            }

            Console.WriteLine(count);
        }
    }
}
