using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day3
    {
        public static void Run()
        {
            const int width = 140;
            const int height = 140;
            //const int height = 4;

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

            // numbersAt[x, y]
            var numbersAt = GetNumbersAt(height, width, input);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var c = input[x, y];
                    if (c == '*')
                    {
                        var adjacentNums = new HashSet<double>(); // Just assume no duplicates hehe
                        for (var i = -1; i <= 1; i++)
                        {
                            for (var j = -1; j <= 1; j++)
                            {
                                try
                                {
                                    var number = numbersAt[x + i, y + j];
                                    if (number != -1)
                                    {
                                        adjacentNums.Add(number);
                                    }
                                }
                                catch
                                {
                                    // Swallow out of bounds exceptions
                                }
                            }
                        }
                        // Find adjacent numbers
                        if (adjacentNums.Count == 2)
                        {
                            var p = 1d;
                            foreach (var k in adjacentNums)
                            {
                                p *= k;
                            }
                            sum += p;
                        }
                    }
                }
            }

            Console.WriteLine(sum);
        }

        public static double[,] GetNumbersAt(int height, int width, char[,] input)
        {
            var retVal = new double[height, width];
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    retVal[i, j] = -1;
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

                            for (var x = digitStart; x <= digitEnd; x++)
                            {
                                retVal[x, j] = num;
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        public static List<double> GetNumbers(int height, int width, char[,] input)
        {
            var nums = new List<double>();

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

            return nums;
        }
    }
}
