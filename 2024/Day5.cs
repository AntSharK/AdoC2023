using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day5
    {
        public static void Run()
        {

            string line = "dummy";

            var pages = new Dictionary<int, Node>();
            while (line != "")
            {
                line = Console.ReadLine();
                var linesplit = line.Split('|');
                if (linesplit.Length <= 1)
                {
                    break;
                }

                var page1 = Int32.Parse(linesplit[0]);
                var page2 = Int32.Parse(linesplit[1]);

                if (!pages.ContainsKey(page1))
                {
                    pages[page1] = new Node(page1);
                }
                if (!pages.ContainsKey(page2))
                {
                    pages[page2] = new Node(page2);
                }

                var n1 = pages[page1];
                var n2 = pages[page2];
                n1.Before.Add(n2);
            }

            List<Node> stack = new List<Node>();
            foreach (var n in pages.Values)
            {
                if (!stack.Contains(n))
                {
                    stack.Add(n);
                    n.DFS(stack);
                }
            }

            // The list is now topologically sorted
            Console.Write("Top sort finished.");

            // Now generate rules from bottom
            stack[0].GetBefores();

            var sum = 0;
            line = "dummy";
            while (line != "")
            {
                line = Console.ReadLine();
                var linesplit = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (linesplit.Length <= 1)
                {
                    break;
                }

                var originalList = new List<Node>();
                foreach (var s in linesplit)
                {
                    var i = Int32.Parse(s);
                    if (pages.ContainsKey(i))
                    {
                        originalList.Add(pages[i]);
                    }
                }

                var isValid = true;
                for (var i = 0; i < originalList.Count - 1; i++)
                {
                    var prevNum = originalList[i];
                    var nextNum = originalList[i + 1];

                    if (nextNum.Before.Contains(prevNum))
                    {
                        isValid = false;
                        Console.WriteLine("INVALID" + nextNum + " " + prevNum);
                        break;
                    }
                }

                if (isValid)
                {
                    var midNum = Int32.Parse(linesplit[linesplit.Length / 2]);
                    sum += midNum;
                    Console.WriteLine("VALID");
                }

                Console.WriteLine(sum);
            }
        }
    }

    public class Node
    {
        public HashSet<Node> Before = new HashSet<Node>();
        bool GetBeforeExecuted = false;
        public int Value;

        public Node(int v) {
            Value = v;
        }

        public void DFS(List<Node> stack)
        {
            foreach (var n in Before)
            {
                if (!stack.Contains(n))
                {
                    stack.Add(n);
                    n.DFS(stack);
                }
            }
        }

        public void GetBefores()
        {
            this.GetBeforeExecuted = true;

            var k = Before.ToArray();
            for (var i = 0; i < k.Length; i++)
            {
                var n = k[i];
                if (!n.GetBeforeExecuted)
                {
                    n.GetBefores();
                }

                foreach (var j in n.Before)
                {
                    this.Before.Add(j);
                }
            }
        }

        public override int GetHashCode()
        {
            return this.Value;
        }

        public override bool Equals(object? obj)
        {
            return (obj as Node).Value == Value;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this.Value);
            sb.Append(" [");
            foreach (var k in this.Before)
            {
                sb.Append(k.Value);
                sb.Append(',');
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
