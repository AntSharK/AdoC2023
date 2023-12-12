using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day11
    {
        public static void Run()
        {
            var galaxy = new List<List<bool>>();

            var line = "";
            while (true)
            {
                line = Console.ReadLine();
                if (line.Length == 0) { break; }
                var row = new List<bool>();

                foreach (var c in line)
                {
                    switch (c)
                    {
                        case '.':
                            row.Add(false);
                            break;
                        case '#':
                            row.Add(true);
                            break;
                        default:
                            break;
                    }
                }

                galaxy.Add(row);
            }

            // Expand rows
            var colLength = galaxy[0].Count;
            List<int> rowsToExpand = new List<int>();
            for (var i = 0; i < galaxy.Count; i++)
            {
                var row = galaxy[i];
                if (!row.Contains(true))
                {
                    rowsToExpand.Add(i);
                }
            }
            /* Part 1 logic
            var emptyRow = new List<bool>();
            for (var i = rowsToExpand.Count - 1; i >= 0; i--)
            {
                var rowAt = rowsToExpand[i];
                galaxy.Insert(rowAt, emptyRow);
            }*/

            // Expand columns
            var emptyCol = new List<int>();
            for (var i = 0; i < colLength; i++)
            {
                var isEmpty = true;
                foreach (var r in galaxy)
                {
                    if (r.Count > 0 && r[i] == true)
                    {
                        isEmpty = false;
                        continue;
                    }
                }

                if (isEmpty)
                {
                    emptyCol.Add(i);
                }
            }

            /* Part 1 logic
            for (var i = emptyCol.Count - 1; i >= 0; i--)
            {
                var c = emptyCol[i];
                foreach (var r in galaxy)
                {
                    if (r.Count > 0)
                    {
                        r.Insert(c, false);
                    }
                }
            }

            Print(galaxy);
            */

            var galaxies = new List<(int, int)>();
            for (var y = 0; y < galaxy.Count; y++)
            {
                var row = galaxy[y];
                if (row.Count > 0)
                {
                    for (var x = 0; x < colLength; x++)
                    {
                        if (row[x])
                        {
                            galaxies.Add((x, y));
                        }
                    }
                }
            }

            /* Part 1 logic
            foreach (var g in galaxies)
            {
                Console.Write($"({g.Item1},{g.Item2}),");
            }
            Console.WriteLine("");

            var distSum = 0;
            var numO = 0;
            for(var i = 0; i < galaxies.Count; i++)
            {
                for(var j = i + 1; j < galaxies.Count; j++)
                {
                    var dist = Math.Abs(galaxies[i].Item1 - galaxies[j].Item1) + Math.Abs(galaxies[i].Item2 - galaxies[j].Item2);

                    Console.WriteLine($"{galaxies[i].Item1},{galaxies[i].Item2} - {galaxies[j].Item1},{galaxies[j].Item2} - {dist}.");
                    distSum += dist;
                    numO++;
                }
            }

            Console.WriteLine(numO);
            Console.WriteLine(distSum);
            */

            // Now for part 2, see how many empty rows and columns gets passed
            var distSum = 0d;
            for (var i = 0; i < galaxies.Count; i++)
            {
                for (var j = i + 1; j < galaxies.Count; j++)
                {
                    double dist = Math.Abs(galaxies[i].Item1 - galaxies[j].Item1) + Math.Abs(galaxies[i].Item2 - galaxies[j].Item2);

                    var emptyColumns = 0;
                    var emptyRows = 0;
                    foreach (var c in emptyCol)
                    {
                        var lower = galaxies[i].Item1;
                        var higher = galaxies[j].Item1;
                        if (lower > higher)
                        {
                            lower = galaxies[j].Item1;
                            higher = galaxies[i].Item1;
                        }

                        if (c > lower && c < higher)
                        {
                            emptyColumns++;
                        }
                    }
                    foreach (var r in rowsToExpand)
                    {
                        var lower = galaxies[i].Item2;
                        var higher = galaxies[j].Item2;
                        if (lower > higher)
                        {
                            lower = galaxies[j].Item2;
                            higher = galaxies[i].Item2;
                        }

                        if (r > lower && r < higher)
                        {
                            emptyRows++;
                        }
                    }

                    const int EXPANSIONFACTOR = 1000000;
                    dist += (EXPANSIONFACTOR - 1) * (emptyRows + emptyColumns);

                    Console.WriteLine($"{galaxies[i].Item1},{galaxies[i].Item2} - {galaxies[j].Item1},{galaxies[j].Item2} - {dist} + {emptyRows} empty rows + {emptyColumns} empty cols.");
                    distSum += dist;
                }
            }

            Console.WriteLine(distSum);
        }

        public static void Print(List<List<bool>> glx)
        {
            foreach (var i in glx)
            {
                foreach (var j in i)
                {
                    Console.Write(j ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

    }
}
