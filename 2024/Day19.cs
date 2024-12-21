using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day19
    {
        public static Dictionary<string, long> Cache = new Dictionary<string, long>();

        public static void Run()
        {
            var line = Console.ReadLine();
            Console.ReadLine();
            var seqs = line.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            var possible = 0l;
            while (line != "")
            {
                line = Console.ReadLine();
                if (line.Length > 0) {
                    var num = NumPossible(line, seqs);
                    possible += num;
                    Console.WriteLine($"Possibilities: {num}");
                }
            }

            Console.WriteLine(possible);
        }

        public static long NumPossible(string seq, string[] patterns)
        {
            if (seq.Length == 0) {return 0; }

            if (Cache.ContainsKey(seq)) { 
                Console.WriteLine($"Cache hit for {seq}");
                return Cache[seq]; 
            }

            var totals = 0l;
            var pp = 0;
            foreach (var p in patterns)
            {
                pp++;
                if (seq == p)
                {
                    totals = totals + 1;
                    continue;
                }

                if (seq.StartsWith(p))
                {
                    var nump = NumPossible(seq.Substring(p.Length), patterns);
                    totals = totals + nump;
                }
            }

            Cache[seq] = totals;
            return totals;
        }
    }
}
