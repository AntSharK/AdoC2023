using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day5DumbWay
    {
        public static void Run()
        {
            // Just apply every rule to every pair
            var line = "dummy";
            var rules = new List<(int, int)>();
            while (line != "")
            {
                line = Console.ReadLine();
                var sp = line.Split('|');
                if (sp.Length <= 1)
                {
                    break;
                }

                rules.Add((Int32.Parse(sp[0]), Int32.Parse(sp[1])));
            }

            line = "dummy";
            var validSum = 0;
            var badSum = 0;
            while (line != "")
            {
                line = Console.ReadLine();
                var isValid = true;
                var sp = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (sp.Length <= 0)
                {
                    break;
                }

                var k = sp.Select(x => Int32.Parse(x)).ToArray();

                isValid = IsValid(k, rules);

                if (!isValid)
                {
                    MakeValid(k, rules);
                    Console.WriteLine("MADE: " + string.Join(',', k));
                    var midValue = k[k.Count() / 2];
                    badSum += midValue;
                    Console.WriteLine(badSum);
                }

                if (isValid)
                {
                    var midValue = Int32.Parse(sp[sp.Length / 2]);
                    validSum += midValue;
                    Console.WriteLine(midValue);
                }
            }

            Console.WriteLine(badSum);
        }

        public static void MakeValid(int[] k, List<(int, int)> rules)
        {
            for (var i = 0; i < k.Length; i++)
            {
                for (var j = i + 1; j < k.Length; j++)
                {
                    var n1 = k[i];
                    var n2 = k[j];

                    // Check every rule
                    foreach (var r in rules)
                    {
                        if (r.Item1 == n2 && r.Item2 == n1)
                        {
                            // SWAP AND RESTART
                            k[i] = n2;
                            k[j] = n1;

                            i = 0;
                            j = i + 1;
                        }
                    }
                }
            }
        }

        public static bool IsValid(int[] k, List<(int, int)> rules)
        {
            for (var i = 0; i < k.Length; i++)
            {
                for (var j = i + 1; j < k.Length; j++)
                {
                    var n1 = k[i];
                    var n2 = k[j];

                    // Check every rule
                    foreach (var r in rules)
                    {
                        if (r.Item1 == n2 && r.Item2 == n1)
                        {
                            Console.WriteLine("Violation: " + n1 + " - " + n2);
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
