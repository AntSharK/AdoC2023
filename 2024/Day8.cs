using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day8
    {
        public static void Run()
        {
            var line = "dummy";
            var lines = new List<string>();
            while (line != "")
            {
                line = Console.ReadLine();

                if (line.Length > 0)
                {
                    lines.Add(line);
                }
            }

            // Initialize as false and read the antinodes
            var antennas = new Dictionary<char, List<(int, int)>>();
            var map = new bool[lines[0].Length, lines.Count];
            for (var y = 0; y < lines.Count; y++)
            {
                var ca = lines[y].ToCharArray();
                for (var x = 0; x < ca.Length; x++)
                {
                    map[x, y] = false;

                    var c = ca[x];
                    if (c != '.')
                    {
                        if (!antennas.ContainsKey(c))
                        {
                            antennas[c] = new List<(int, int)>();
                        }

                        antennas[c].Add((x, y));
                    }
                }
            }

            // For each pair, drawa line and extract points
            foreach (var antenna in antennas.Values)
            {
                for (var i = 0; i < antenna.Count; i++)
                {
                    for (var j = i + 1; j < antenna.Count; j++)
                    {
                        /* Part 1
                        var p1 = (antenna[i].Item1 + 2 * (antenna[j].Item1 - antenna[i].Item1), antenna[i].Item2 + 2 * (antenna[j].Item2 - antenna[i].Item2));
                        var p2 = (antenna[j].Item1 + 2 * (antenna[i].Item1 - antenna[j].Item1), antenna[j].Item2 + 2 * (antenna[i].Item2 - antenna[j].Item2));

                        // Cheap bounds checking
                        try
                        {
                            map[p1.Item1, p1.Item2] = true;
                        }
                        catch { }
                        try
                        {
                            map[p2.Item1, p2.Item2] = true;
                        }
                        catch { }
                        */

                        // Part 2
                        var dx = (antenna[j].Item1 - antenna[i].Item1);
                        var dy = (antenna[j].Item2 - antenna[i].Item2);

                        // Find the greatest common factor, and divide by that number
                        var hcf = HCF(Math.Abs(dx), Math.Abs(dy));
                        dx = dx / hcf;
                        dy = dy / hcf;

                        // Just do until out of bounds
                        var startPoint = (antenna[i].Item1, antenna[i].Item2);
                        var increments = 0;
                        while (true)
                        {
                            try
                            {
                                map[startPoint.Item1 + increments * dx, startPoint.Item2 + increments * dy] = true;
                                increments++;
                            }
                            // Out of bounds
                            catch
                            {
                                break;
                            }
                        }

                        increments = -1;
                        while (true)
                        {
                            try
                            {
                                map[startPoint.Item1 + increments * dx, startPoint.Item2 + increments * dy] = true;
                                increments--;
                            }
                            // Out of bounds
                            catch
                            {
                                break;
                            }
                        }
                    }
                }
            }           

            var sum = 0;
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (var x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y]) { 
                        Console.Write('#');
                        sum++;
                    } else { Console.Write('.'); }
                }
                Console.WriteLine();
            }

            Console.WriteLine(sum);
        }
        private static int HCF(int greater, int lesser)
        {
            if (lesser > greater)
            {
                var t = greater;
                greater = lesser;
                lesser = t;
            }

            if (greater % lesser == 0)
            {
                return lesser;
            }

            return HCF(lesser, greater % lesser);
        }
    }
}
