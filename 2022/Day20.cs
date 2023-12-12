using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day20
    {
        public static void Run()
        {
            var inputs = new List<NumberWrapper>();
            var order = new List<NumberWrapper>();
            const double DKEY = 811589153d;
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out var d))
                {
                    var nw = new NumberWrapper() { Val = d*DKEY };
                    inputs.Add(nw);
                    order.Add(nw);
                }
                else
                {
                    break;
                }
            }

            for (var i = 0; i < 10; i++)
            {
                foreach (var n in order)
                {
                    Move(inputs, n);
                }

                Console.WriteLine("MIXED:"+i);
            }

            /*
            foreach (var n in inputs)
            {
                Console.Write(n.Val + ", ");
            }
            */

            var zeroIdx = inputs.FindIndex(0, inputs.Count, n => n.Val == 0);
            var a1 = inputs[(zeroIdx + 1000) % inputs.Count].Val;
            var a2 = inputs[(zeroIdx + 2000) % inputs.Count].Val;
            var a3 = inputs[(zeroIdx + 3000) % inputs.Count].Val;

            Console.WriteLine();
            Console.WriteLine($"{a1} + {a2} + {a3} = {a1 + a2 + a3}");
        }

        private static void Move(List<NumberWrapper> inputs, NumberWrapper n)
        {
            var idx = inputs.IndexOf(n);
            inputs.RemoveAt(idx);

            var newidx = (n.Val + idx) % inputs.Count;
            if (newidx < 0) { newidx = newidx + inputs.Count; }

            inputs.Insert((int)newidx, n);
        }

        // So that there is a pass by reference instead of a value
        private class NumberWrapper
        {
            public double Val;
            public override string ToString()
            {
                return Val.ToString();
            }
        }
    }
}
