using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class CamelTree
    {
        public static void Run()
        {
            var instruction = "LRLRRLLRRLRRRLRLRRRLLRRLLLLRRRLRRRLRRLRRLRRRLRRRLLRRLRLRRLRRRLLLRRLRRLLRLLRRRLRRRLLRLRRRLRLLRRLLLRLRRRLRRRLRRRLLRLRRRLLRRLRLRLLRRLRRRLRRLRLLRLRRRLRRLRLRLRRLRRRLRRRLRRRLRRLRRRLLRRLRRLLRRRLLRLRLRLRLLLRRLRLRRLRRLRRLRRLRRRLRRRLRLRRRLRLRRRLRRLRLLRLRRLRLRLLLRLLLRRRLRRLLLRLRRRR";

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

            var currentNode = nodes["AAA"];
            var destNode = nodes["ZZZ"];
            var steps = 0;
            while (currentNode != destNode)
            {
                foreach (var c in instruction)
                {
                    currentNode = currentNode.Links[c];
                    steps++;
                }
            }

            Console.WriteLine(steps);
        }
    }

    public class Node
    {
        public string Name;
        public Dictionary<char, Node> Links = new Dictionary<char, Node>();
    }
}
