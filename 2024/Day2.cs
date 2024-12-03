using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal static class Day2
    {
        public static void Run()
        {
            string line = "dummy";
            var safeLines = 0;
            while (line != "")
            {
                line = Console.ReadLine();
                var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (nums.Length > 1)
                {
                    List<int> gaps = new List<int>();
                    for (var i = 0; i < nums.Length - 1; i++)
                    {
                        var gap = Int32.Parse(nums[i + 1]) - Int32.Parse(nums[i]);
                        gaps.Add(gap);
                    }

                    if (IsSafe1(gaps))
                    {
                        safeLines++;
                        Console.WriteLine("Safe1");
                    }
                    else
                    {
                        // Part 2: Just remove one number and go
                        for (var k = 0; k < nums.Length; k++)
                        {
                            var newGaps = new List<int>();
                            var newNums = new List<int>();
                            for (var i = 0; i < nums.Length; i++)
                            {
                                // Lol are we actually copying things one by one into a list? YES! SCREW EFFICIENCY!
                                if (i != k)
                                {
                                    newNums.Add(Int32.Parse(nums[i]));
                                }
                            }

                            for (var i = 0; i < newNums.Count - 1; i++)
                            {
                                var gap = newNums[i + 1] - newNums[i];
                                newGaps.Add(gap);
                            }

                            if (IsSafe1(newGaps))
                            {
                                safeLines++;
                                Console.WriteLine("Safe1");
                                break;
                            }
                        }
                    }
                }
            }

            Console.WriteLine(safeLines);
        }

        internal static bool IsSafe1(List<int> Gaps)
        {
            bool? isIncreasing = null;
            foreach (var gap in Gaps)
            {
                if (gap < 0 && isIncreasing.HasValue && isIncreasing.Value)
                {
                    return false;
                }

                if (gap > 0 && isIncreasing.HasValue && !isIncreasing.Value)
                {
                    return false;
                }

                if (gap > 0) { isIncreasing = true; }
                else { isIncreasing = false; }

                if (Math.Abs(gap) < 1 || Math.Abs(gap) > 3)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
