using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day1
    {
        public static void Run()
        {

            string line = "dummy";
            var leftList = new List<double>();
            var rightList = new List<double>();
            while (line != "")
            {
                line = Console.ReadLine();
                var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (nums.Length > 1)
                {
                    leftList.Add(Double.Parse(nums[0]));
                    rightList.Add(Double.Parse(nums[1]));
                }
            }

            leftList.Sort();
            rightList.Sort();

            var sum = 0d;
            for (var i = 0; i < leftList.Count; i++)
            {
                sum += Math.Abs(leftList[i] - rightList[i]);
            }

            Console.WriteLine(sum);

            // Begin Part 2
            var occurrences = new Dictionary<double, int>();
            foreach (var i in rightList)
            {
                if (occurrences.ContainsKey(i))
                {
                    occurrences[i]++;
                }
                else
                {
                    occurrences[i] = 1;
                }
            }

            var part2Sum = 0d;
            foreach (var i in leftList)
            {
                if (occurrences.ContainsKey(i))
                {
                    part2Sum += i * occurrences[i];
                }
            }

            Console.WriteLine(part2Sum);
        }
    }
}
