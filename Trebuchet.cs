using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Trebuchet
    {
        public static void Run()
        {
            string line = "dummy";
            var sum = 0;
            while (line != "")
            {
                line = Console.ReadLine();
                var firstDigit = -1;
                var lastDigit = -1;

                line = line.Replace("zero", "zero0zero");
                line = line.Replace("one", "one1one");
                line = line.Replace("two", "two2two");
                line = line.Replace("three", "three3three");
                line = line.Replace("four", "four4four");
                line = line.Replace("five", "five5five");
                line = line.Replace("six", "six6six");
                line = line.Replace("seven", "seven7seven");
                line = line.Replace("eight", "eight8eight");
                line = line.Replace("nine", "nine9nine");

                foreach (var c in line)
                {
                    if (Char.IsDigit(c))
                    {
                        lastDigit = int.Parse(c.ToString());
                        if (firstDigit == -1)
                        {
                            firstDigit = lastDigit;
                        }
                    }
                }

                if (firstDigit != -1 && lastDigit != -1)
                {
                    sum += firstDigit * 10 + lastDigit;
                }
            }

            Console.WriteLine(sum);
        }
    }
}
