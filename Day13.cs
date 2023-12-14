using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day13
    {
        public static void Run()
        {
            var totalSum = 0l;
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "end") break;

                var verticals = new List<string>();
                var horizontals = new List<string>();
                while (true)
                {
                    if (line == "") break;
                    verticals.Add(line);

                    line = Console.ReadLine();
                }

                if (verticals.Count == 0) break;
                for (var i = 0; i < verticals[0].Length; i++)
                {
                    var sb = new StringBuilder();
                    foreach (var x in verticals)
                    {
                        sb.Append(x.ElementAt(i));
                    }

                    horizontals.Add(sb.ToString());
                }

                var vertMirror = -1;
                var horMirror = -1;
                for (var i = 0; i < verticals.Count - 1; i++)
                {
                    if (IsMirror(verticals, i))
                    {
                        // VertMirror is actually a horizontal line
                        vertMirror = i + 1;
                        break;
                    }
                }

                if (vertMirror == -1)
                {
                    for (var i = 0; i < horizontals.Count - 1; i++)
                    {
                        if (IsMirror(horizontals, i))
                        {
                            // HorMirror is actually a vertical line
                            horMirror = i + 1;
                            break;
                        }
                    }
                }

                if (vertMirror > -1) { totalSum += 100 * vertMirror; }
                if (horMirror > -1) { totalSum += horMirror; }
                Console.WriteLine($"V:{vertMirror}, H:{horMirror}");
            }

            Console.WriteLine(totalSum);
        }

        private static bool IsMirror(List<string> strss, int i)
        {
            var left = i;
            var right = i + 1;

            var differingLeft = "";
            var differingRight = "";
            while (left >= 0 && right < strss.Count)
            {
                if (strss[left] != strss[right])
                {
                    if (differingLeft == "")
                    {
                        differingLeft = strss[left];
                        differingRight = strss[right];
                    }
                    else
                    {
                        return false;
                    }
                }
                left--;
                right++;
            }

            if (differingLeft == "")
                return false;

            var differed = false;
            for (var x = 0; x < differingLeft.Length; x++)
            {
                if (differingLeft[x] != differingRight[x])
                {
                    if (differed)
                    {
                        return false;
                    }
                    differed = true;
                }
            }

            return differed;
        }
    }
}
