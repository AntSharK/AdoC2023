using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day14
    {
        public static void Run()
        {
            const int WIDTH = 1200;
            var grid = new char[WIDTH, 600];
            for (var y = 0; y < 600; y++)
            {
                for (var x = 0; x < WIDTH; x++)
                {
                    grid[x, y] = '.';
                }
            }

            var maxY = 0;
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") break;

                var points = line.Split("->", StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < points.Length - 1; i++)
                {
                    var from = points[i].Split(',').Select(z => int.Parse(z)).ToArray();
                    var to = points[i + 1].Split(',').Select(z => int.Parse(z)).ToArray();

                    if (to[1] > maxY) { maxY = to[1]; }

                    if (from[0] == to[0])
                    {
                        if (from[1] < to[1])
                        {
                            for (var k = from[1]; k <= to[1]; k++)
                            {
                                grid[from[0], k] = '#';
                            }
                        }
                        else
                        {
                            for (var k = from[1]; k >= to[1]; k--)
                            {
                                grid[from[0], k] = '#';
                            }
                        }
                    }

                    else if (from[1] == to[1])
                    {
                        if (from[0] < to[0])
                        {
                            for (var k = from[0]; k <= to[0]; k++)
                            {
                                grid[k, from[1]] = '#';
                            }
                        }
                        else
                        {
                            for (var k = from[0]; k >= to[0]; k--)
                            {
                                grid[k, from[1]] = '#';
                            }
                        }
                    }
                }
            }

            // Draw the floor
            for (var i = 0; i < WIDTH; i++)
            {
                grid[i, maxY + 2] = '#';
            }

            var grainsOfSand = 0;
            while (true)
            {
                var sandPos = (500, 0);
                while (sandPos.Item2 < 599)
                {
                    var thingBelow = grid[sandPos.Item1, sandPos.Item2 + 1];
                    if (thingBelow == '.')
                    {
                        sandPos = (sandPos.Item1, sandPos.Item2 + 1);
                    }
                    else
                    {
                        var thingDownLeft = grid[sandPos.Item1 - 1, sandPos.Item2 + 1];
                        if (thingDownLeft == '.')
                        {
                            sandPos = (sandPos.Item1 - 1, sandPos.Item2 + 1);
                        }
                        else
                        {
                            var thingDownRight = grid[sandPos.Item1 + 1, sandPos.Item2 + 1];
                            if (thingDownRight == '.')
                            {
                                sandPos = (sandPos.Item1 + 1, sandPos.Item2 + 1);
                            }
                            else
                            {
                                grid[sandPos.Item1, sandPos.Item2] = 'o';
                                grainsOfSand++;
                                break;
                            }
                        }
                    }
                }

                if (sandPos == (500, 0)) break;
            }

            Console.WriteLine(grainsOfSand);
            
            /*for(var y = 0; y < 15; y++)
            {
                for (var x = 490; x < 505; x++)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }*/
        }
    }
}
