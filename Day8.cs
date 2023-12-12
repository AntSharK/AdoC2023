using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day8
    {
        public static void Run()
        {
            var instruction = "LRLRRLLRRLRRRLRLRRRLLRRLLLLRRRLRRRLRRLRRLRRRLRRRLLRRLRLRRLRRRLLLRRLRRLLRLLRRRLRRRLLRLRRRLRLLRRLLLRLRRRLRRRLRRRLLRLRRRLLRRLRLRLLRRLRRRLRRLRLLRLRRRLRRLRLRLRRLRRRLRRRLRRRLRRLRRRLLRRLRRLLRRRLLRLRLRLRLLLRRLRLRRLRRLRRLRRLRRRLRRRLRLRRRLRLRRRLRRLRLLRLRRLRLRLLLRLLLRRRLRRLLLRLRRRR";
            //var instruction = "LR";

            var line = "dummy";
            var nodes = new Dictionary<string, Node>();
            while (line != "")
            {
                line = Console.ReadLine();
                if (line == "") break;

                var s1 = line.Split('=');
                var nodeName = s1[0].Trim();

                var s2 = s1[1].Split(',');
                var leftName = s2[0].Substring(2);
                var rightName = s2[1].Trim();
                rightName = rightName.Substring(0, rightName.Length - 1);

                if (!nodes.ContainsKey(nodeName)) nodes.Add(nodeName, new Node() { Name = nodeName });
                if (!nodes.ContainsKey(leftName)) nodes.Add(leftName, new Node() { Name = leftName });
                if (!nodes.ContainsKey(rightName)) nodes.Add(rightName, new Node() { Name = rightName });

                nodes[nodeName].Links['L'] = nodes[leftName];
                nodes[nodeName].Links['R'] = nodes[rightName];
            }

            var currentNodes = new List<Node>();
            var startNodes = new List<string>();
            foreach (var node in nodes)
            {
                if (node.Key.EndsWith('A'))
                {
                    currentNodes.Add(node.Value);
                    startNodes.Add(node.Key);
                }
            }

            foreach (var node in nodes) 
            {
                node.Value.FindNextStep(instruction);
            }

            foreach (var nodeName in startNodes)
            {
                var startNode = nodes[nodeName];
                var currentNode = startNode;
                for (var i = 1; i <= nodes.Count+1; i++)
                {
                    currentNode = currentNode.NextStep;
                    if (currentNode.Name.EndsWith('Z'))
                    {
                        startNode.StepsToTerminate.Add(i);
                    }
                }
            }

            // Write out the cycles
            foreach (var nodeName in startNodes)
            {
                var startNode = nodes[nodeName];
                var lasti = 0d;
                foreach (var i in startNode.StepsToTerminate)
                {
                    Console.Write(i + " (" + (i-lasti) + "), ");
                    lasti = i;
                }

                Console.WriteLine();
            }
            // All these are prime
            var lcm = 47d * 71d * 59d * 73d * 53d * 67d;
            Console.WriteLine(lcm);
            Console.WriteLine(instruction.Length);
            // LCM is 51036601909, length of instruction is 271
            Console.WriteLine((lcm) * 271d); // 51,036,601,909 * 271
        }
    }

    public class Node
    {
        public string Name;
        public Dictionary<char, Node> Links = new Dictionary<char, Node>();
        public Node NextStep;
        public List<double> StepsToTerminate = new List<double>();

        public void FindNextStep(string instruction)
        {
            var currentNode = this;
            foreach (var c in instruction)
            {
                currentNode = currentNode.Links[c];
            }

            this.NextStep = currentNode;
        }
    }
}
