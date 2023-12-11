using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day10
    {
        public static void Run()
        {
            var line = "dummy";

            var currCycle = 1;
            var currX = 1;
            var changeLog = new List<(int, int)>() { (0, 1) };
            while (line != "")
            {
                line = Console.ReadLine();
                if (line == "") break;

                if (line.StartsWith("noop"))
                {
                    currCycle++;
                }
                else
                {
                    var xModify = int.Parse(line.Substring(5));
                    currCycle = currCycle + 2;
                    currX = currX + xModify;

                    changeLog.Add((currCycle, currX));
                }
            }

            for(var y = 0; y < 6; y++)
            {
                for(var x = 1; x <= 40; x++)
                {
                    var cycle = y * 40 + x;
                    var vat = FindValueAt(cycle, changeLog);
                    if (Math.Abs(vat - (x-1)) <= 1)
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }

            /* Part 1
            var findValueAt = new List<int> { 20, 60, 100, 140, 180, 220 };
            var s = 0d;
            foreach (var t in findValueAt)
            {
                var vat = FindValueAt(t, changeLog);
                s += t * vat;
                Console.WriteLine($"Value at {t} is {vat}");
            }

            Console.WriteLine(s);
            */
        }

        private static int FindValueAt(int time, List<(int, int)> changeLog)
        {
            var lastX = changeLog[0].Item2;
            foreach (var change in changeLog)
            {
                if (change.Item1 > time)
                {
                    return lastX;
                }

                lastX = change.Item2;
            }

            return -1;
        }
    }
}
