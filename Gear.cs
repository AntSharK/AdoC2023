using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Gear
    {
        public static void Run()
        {
            const int width = 140;
            const int height = 141;
            //const int height = 4;

            var nums = new List<double>();

            var sum = 0d;
            var input = new char[width, height];
            for (int i = 0; i < height; i++)
            {
                var line = Console.ReadLine();
                var j = 0;
                foreach (var c in line)
                {
                    input[j, i] = c;
                    j++;
                }
            }

            for (var j = 0; j < height; j++)
            {
                var digitStart = -1;
                var digitEnd = -1;
                bool parsing = false;
                for (var i = 0; i < width + 1; i++)
                {
                    var c = '.';
                    if (i != width)
                    {
                        c = input[i, j];
                    }
                    if (char.IsDigit(c))
                    {
                        if (!parsing)
                        {
                            parsing = true;
                            digitStart = i;
                            digitEnd = i;
                        }
                        else
                        {
                            digitEnd = i;
                        }
                    }
                    else
                    {
                        if (parsing)
                        {
                            parsing = false;
                            var num = 0d;
                            for (var x = digitStart; x <= digitEnd; x++)
                            {
                                num = num * 10 + int.Parse(input[x, j].ToString());
                            }

                            // Find Symbols
                            var isPart = false;
                            for (var p = j - 1; p <= j + 1; p++)
                            {
                                for (var q = digitStart - 1; q <= digitEnd + 1; q++)
                                {
                                    if (p >= 0 && p <= height - 1
                                        && q >= 0 && q < width)
                                    {
                                        var cc = input[q, p];
                                        if (!Char.IsDigit(cc)
                                            && cc != '.')
                                        {
                                            isPart = true;
                                        }
                                    }
                                }
                            }

                            if (isPart)
                            {
                                nums.Add(num);
                            }
                        }
                    }
                }
            }

            foreach (var k in nums)
            {
                Console.Write(k + ", ");
            }
            Console.Write(nums.Sum());
        }
    }
}
