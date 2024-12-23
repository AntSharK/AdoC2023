using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day23
    {
        public static void Run()
        {
            var conn = new Dictionary<string, List<string>>();

            while (true)
            {
                var line = Console.ReadLine();
                if (line.Length == 0) { break; }

                var sp = line.Split('-', StringSplitOptions.RemoveEmptyEntries);

                var comp1 = sp[0];
                var comp2 = sp[1];

                if (!conn.ContainsKey(comp1))
                {
                    conn[comp1] = new List<string>();
                }

                if (!conn.ContainsKey(comp2))
                {
                    conn[comp2] = new List<string>();
                }

                conn[comp1].Add(comp2);
                conn[comp2].Add(comp1);
            }

            // Find sets of 13
            // We just kept increasing this until we got an answer - why be smart when we can be dumb
            HashSet<string> multiWayConnections = new HashSet<string>();
            foreach (var c in conn)
            {
                var c1 = c.Key;
                if (c.Value.Count < 12) { continue; }
                for (var i = 0; i < c.Value.Count; i++)
                {
                    for (var j = i + 1; j < c.Value.Count; j++)
                    {
                        for (var k = j + 1; k < c.Value.Count; k++)
                        {
                            for (var m = k + 1; m < c.Value.Count; m++)
                            {
                                for (var n = m + 1; n < c.Value.Count; n++)
                                {
                                    for (var p = n + 1; p < c.Value.Count; p++)
                                    {
                                        for (var q = p + 1; q < c.Value.Count; q++)
                                        {
                                            for (var q1 = q + 1; q1 < c.Value.Count; q1++)
                                            {
                                                for (var q2 = q1 + 1; q2 < c.Value.Count; q2++)
                                                {
                                                    for (var q3 = q2 + 1; q3 < c.Value.Count; q3++)
                                                    {
                                                        for (var q4 = q3 + 1; q4 < c.Value.Count; q4++)
                                                        {
                                                            for (var q5 = q4 + 1; q5 < c.Value.Count; q5++)
                                                            {
                                                                var cs = new string[] {
                                                c.Value[i],
                                                c.Value[j],
                                                c.Value[k],
                                                c.Value[m],
                                                c.Value[n],
                                                c.Value[p],
                                                c.Value[q],
                                                c.Value[q1],
                                                c.Value[q2],
                                                c.Value[q3],
                                                c.Value[q4],
                                                c.Value[q5]
                                            };

                                                                var isConnected = true;
                                                                for (var x = 0; x < cs.Length; x++)
                                                                {
                                                                    for (var y = 0; y < cs.Length; y++)
                                                                    {
                                                                        if (x == y) { continue; }

                                                                        if (!conn[cs[x]].Contains(cs[y]))
                                                                        {
                                                                            isConnected = false;
                                                                        }
                                                                    }
                                                                }

                                                                if (isConnected)
                                                                {
                                                                    var sortConn = cs.ToList();
                                                                    sortConn.Add(c1);
                                                                    sortConn.Sort();
                                                                    multiWayConnections.Add(string.Join(',', sortConn));

                                                                    Console.WriteLine(string.Join(',', sortConn));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }                            
                    }
                }
            }

            foreach (var c in multiWayConnections)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine(multiWayConnections.Count);
        }
    }
}