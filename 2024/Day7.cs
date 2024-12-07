using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day7
    {
        public static void Run()
        {
            var sum = 0d;
            var line = "dummy";
            while (line != "")
            {
                line = Console.ReadLine();
                var nums = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (nums.Length < 1)
                {
                    break;
                }

                var target = Double.Parse(nums[0]);
                var ins = new List<Double>();
                for (var i = 1; i < nums.Length; i++)
                {
                    ins.Add(Double.Parse(nums[i]));
                }

                if (IsPossible(target, ins))
                {
                    Console.WriteLine("POSSIBLE");
                    sum += target;
                }
            }

            Console.WriteLine(sum);
        }

        private static bool IsPossible(double target, List<double> ins)
        {
            // Just evaluate from left to right
            var evaluated = new List<double>() { 1d };
            while (ins.Count != 0)
            {
                var nextNum = ins.First();
                ins.RemoveAt(0);
                var newEvaluated = new List<double>();
                foreach (var n in evaluated)
                {
                    var mult = n * nextNum;
                    if (mult <= target)
                    { 
                        newEvaluated.Add(mult);
                    }

                    var add = n + nextNum;
                    if (add <= target)
                    {
                        newEvaluated.Add(add);
                    }

                    // Part B
                    // POOR MAN'S CONCATENATION VIA STRING
                    var concat = Double.Parse(n.ToString() + nextNum.ToString());
                    if (concat <= target)
                    {
                        newEvaluated.Add(concat);
                    }
                }

                evaluated = newEvaluated;
            }

            return evaluated.Contains(target);
        }
    }
}
