using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day9
    {
        public static void Run()
        {
            var line = "dummy";
            var sum = 0d;
            while (line != "")
            {
                line = Console.ReadLine();
                if (line == "")
                {
                    continue;
                }

                var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var input = new List<double>();
                foreach (var c in split)
                {
                    input.Add(double.Parse(c));
                }

                var diffList = GetAllDiffs(input);
                for(var i = diffList.Count - 1; i >= 1; i--)
                {
                    var listToCheck = diffList[i];
                    var listToFill = diffList[i - 1];

                    // Part A:
                    // listToFill.Add(listToFill.Last() + listToCheck.Last());

                    // Part B:
                    listToFill.Insert(0, listToFill.First() - listToCheck.First());
                }

                sum += diffList[0].First(); 
            }

            Console.WriteLine(sum);
        }

        public static List<double> ComputeDifference(List<double> nums)
        {
            var lastNum = -1d;
            var firstT = true;
            var retVal = new List<double>();
            foreach (var num in nums)
            {
                var diff = num - lastNum;
                if (!firstT)
                {
                    retVal.Add(diff);
                }

                firstT = false;
                lastNum = num;
            }

            return retVal;
        }

        public static List<List<double>> GetAllDiffs(List<double> nums)
        {
            var result = new List<List<double>>();
            var currentList = nums;
            while (!IsAllZero(currentList))
            {
                var lc = new List<double>();
                foreach (var c in currentList)
                {
                    lc.Add(c);
                }

                result.Add(lc);
                currentList = ComputeDifference(currentList);
            }

            result.Add(currentList);
            return result;
        }

        public static bool IsAllZero(List<double> nums)
        {
            foreach (var num in nums)
            {
                if (num != 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
