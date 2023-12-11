using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day5
    {
        public static void Run()
        {
            // A bit of a pain - manually enter
            // From bottom to top
            var stacks = new List<List<char>>()
            {
                new List<char>() { 'B', 'P', 'N', 'Q', 'H', 'D', 'R', 'T' },
                new List<char>() { 'W', 'G', 'B', 'J', 'T', 'V' },
                new List<char>() { 'N', 'R', 'H', 'D', 'S', 'V', 'M', 'Q' },
                new List<char>() { 'P', 'Z', 'N', 'M', 'C' },
                new List<char>() { 'D', 'Z', 'B' },
                new List<char>() { 'V', 'C', 'W', 'Z' },
                new List<char>() { 'G', 'Z', 'N', 'C', 'V', 'Q', 'L', 'S' },
                new List<char>() { 'L', 'G', 'J', 'M', 'D', 'N', 'V' },
                new List<char>() { 'T', 'P', 'M', 'F', 'Z', 'C', 'G' },
            };

            var line = "";
            while (true)
            {
                line = Console.ReadLine();
                if (!line.StartsWith("move")) { break; }

                var amountMoved = int.Parse(line.Substring(5, line.IndexOf("from") - 5));
                var from = int.Parse(line.Substring(line.IndexOf("from") + 5, line.IndexOf("to") - line.IndexOf("from") - 5));
                var to = int.Parse(line.Substring(line.IndexOf("to") + 3));

                // Part 2
                var movedStack = new List<char>();
                var stackFrom = stacks[from - 1];
                for (var c = stackFrom.Count - amountMoved; c < stackFrom.Count; c++)
                {
                    movedStack.Add(stackFrom[c]);
                }

                stackFrom.RemoveRange(stackFrom.Count - amountMoved, amountMoved);
                stacks[to - 1].AddRange(movedStack);
                /* Part 1
                for (var c = 0; c < amountMoved; c++)
                {
                    var movedCrate = stacks[from - 1].Last();
                    stacks[to - 1].Add(movedCrate);
                    stacks[from - 1].RemoveAt(stacks[from - 1].Count - 1);
                }
                */
            }

            foreach (var c in stacks)
            {
                Console.Write(c.Last());
            }
        }
    }
}
