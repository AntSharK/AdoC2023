using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day24
    {
        static Dictionary<string, (string, string)> from = new Dictionary<string, (string, string)>();
        static Dictionary<string, bool> vals = new Dictionary<string, bool>();
        static Dictionary<string, string> ops = new Dictionary<string, string>();
        public static void Run()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") { break; }

                var sp = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                vals[sp[0]] = sp[1] == " 1";
            }

            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") { break; }

                var sp = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                // sp[0] is the first gate
                // sp[1] is the operator
                // sp[2] is the second gate
                // sp[4] is the result

                from[sp[4]] = (sp[0], sp[2]);
                ops[sp[4]] = sp[1];
            }

            foreach (var f in from)
            {
                FindVal(f.Key);
            }

            var zGates = new List<(string, bool)>();
            foreach (var f in from)
            {
                if (f.Key.StartsWith('z'))
                {
                    Console.WriteLine($"{f.Key}:{vals[f.Key]}");
                    zGates.Add((f.Key, vals[f.Key]));
                }
            }

            zGates.Sort();
            var total = 0l;
            var dex = 1l;
            foreach (var z in zGates)
            {
                if (z.Item2)
                {
                    total = total + dex;
                }

                dex = dex * 2l;
            }

            Console.WriteLine(total);
        }

        public static bool FindVal(string gate)
        {
            if (vals.ContainsKey(gate)) { return vals[gate]; }

            (var gate1, var gate2) = from[gate];
            if (!vals.ContainsKey(gate1))
            {
                vals[gate1] = FindVal(gate1);
            }

            if (!vals.ContainsKey(gate2))
            {
                vals[gate2] = FindVal(gate2);
            }

            var retVal = false;
            switch (ops[gate])
            {
                case "XOR":
                    retVal = vals[gate1] ^ vals[gate2];
                    break;
                case "OR":
                    retVal = vals[gate1] || vals[gate2];
                    break;
                case "AND":
                    retVal = vals[gate1] && vals[gate2];
                    break;
                default:
                    throw new Exception("Invalid operation");
            }

            vals[gate] = retVal;
            return retVal;
        }
    }
}
