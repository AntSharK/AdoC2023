using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day1
    {
        public static void Run()
        {
            var line = "";
            var elfNum = 0;
            var maxSum = -1d;
            var currSum = 0d;
            var e = 0;
            var elf = new List<double>();
            while (true)
            {
                line = Console.ReadLine();
                if (line == "end") { break; }
                if (line.Length == 0)
                {
                    if (currSum > maxSum)
                    {
                        maxSum = currSum;
                        elfNum = e;
                    }

                    elf.Add(currSum);
                    currSum = 0;
                    e++;
                }
                else
                {
                    var calorie = double.Parse(line);
                    currSum += calorie;
                }
            }

            elf.Sort();
            Console.WriteLine(elf);
        }
    }
}
