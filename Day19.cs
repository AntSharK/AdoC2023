using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day19
    {
        static Dictionary<string, Rule> rules = new Dictionary<string, Rule>();
        public static void Run()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") break;
                var rule = new Rule();
                rule.Name = line.Substring(0, line.IndexOf('{'));
                var allRules = line.Substring(line.IndexOf('{') + 1, line.IndexOf('}') - line.IndexOf('{') - 1).Split(',');
                foreach (var r in allRules)
                {
                    var c = new Condition();
                    if (r.Contains(':'))
                    {
                        c.Result = r.Substring(r.IndexOf(':') + 1);
                        if (r.Contains('>'))
                        {
                            c.Operation = ">";
                        }
                        else
                        {
                            c.Operation = "<";
                        }

                        c.ElementNumber = (r.Substring(0, r.IndexOf(c.Operation)), int.Parse(r.Substring(r.IndexOf(c.Operation) + 1, r.IndexOf(':') - r.IndexOf(c.Operation) - 1)));
                    }
                    else
                    {
                        c.Result = r;
                    }

                    rule.Conditions.Add(c);
                }

                rules[rule.Name] = rule;
            }

            var entryPoint = rules["in"];
            var range = new Dictionary<string, bool[]>();
            range["x"] = new bool[4001];
            range["m"] = new bool[4001];
            range["a"] = new bool[4001];    
            range["s"] = new bool[4001];    
                
            var evaluatedRange = EvaluateRange(entryPoint, range);

            /* PART A
            var parts = new List<Dictionary<string, int>>();
            line = Console.ReadLine();
            while (true)
            {
                var splitline = line.Trim('{').Trim('}').Split(',');
                var d = new Dictionary<string, int>();
                foreach (var c in splitline)
                {
                    var k = c.Split('=');
                    d[k[0]] = int.Parse(k[1]);
                }

                parts.Add(d);
                line = Console.ReadLine();

                if (line == "") { break; }
            }

            var sum = new Dictionary<string, int>();
            sum["x"] = 0;
            sum["m"] = 0;
            sum["a"] = 0;
            sum["s"] = 0;
            foreach (var part in parts)
            {
                var accepted = Eval(part, rules);
                Console.WriteLine(accepted);

                if (accepted)
                {
                    foreach (var p in part)
                    {
                        sum[p.Key] += p.Value;
                    }
                }
            }

            Console.WriteLine(sum.Values.Sum());
            */
        }

        private static Dictionary<string, bool[]> EvaluateRange(Rule rule, Dictionary<string, bool[]> range)
        {
            throw new NotImplementedException();
        }

        private static Dictionary<string, bool[]> EvaluateCondition(Condition condition, Dictionary<string, bool[]> range)
        {
            var newRange = new Dictionary<string, bool[]> { { "x", new bool[4001] }, { "m", new bool[4001] }, { "a", new bool[4001] }, { "s", new bool[4001] } };
            if (condition.Operation == "ALWAYS")
            {
                if (condition.Result == "R") { return newRange; }
                if (condition.Result == "A") { return range; }

                return EvaluateRange(rules[condition.Result], range);
            }

            Array.Copy(range["x"], newRange["x"], 4001);
            Array.Copy(range["m"], newRange["m"], 4001);
            Array.Copy(range["a"], newRange["a"], 4001);
            Array.Copy(range["s"], newRange["s"], 4001);

            if (condition.Operation == "<")
            {
                for(var i = 0; i < condition.ElementNumber.Item2; i++)
                {
                    newRange[condition.ElementNumber.Item1][i] = true;
                }
            }

            if (condition.Operation == ">")
            {
                for (var i = condition.ElementNumber.Item2 + 1; i <= 4000; i++)
                {
                    newRange[condition.ElementNumber.Item1][i] = true;
                }
            }

            return EvaluateRange(rules[condition.Result], newRange);
        }

        private static bool Eval(Dictionary<string, int> part, Dictionary<string, Rule> rules)
        {
            var currentFlow = rules["in"];
            while (true)
            {
                foreach (var rule in currentFlow.Conditions)
                {
                    var applyRule = false;
                    if (rule.Operation == "ALWAYS")
                    {
                        applyRule = true;
                    }

                    if (!applyRule)
                    {
                        var elementToCompare = rule.ElementNumber.Item1;
                        var number = rule.ElementNumber.Item2;
                        if (rule.Operation == ">")
                        {
                            if (part[elementToCompare] > number)
                            {
                                applyRule = true;
                            }
                        }
                        else if (rule.Operation == "<")
                        {
                            if (part[elementToCompare] < number)
                            {
                                applyRule = true;
                            }
                        }
                    }

                    if (applyRule)
                    {
                        if (rule.Result == "R") { return false; }
                        if (rule.Result == "A") { return true; }
                        currentFlow = rules[rule.Result];
                        break;
                    }
                }
            }
        }

        private class Rule
        {
            public string Name;
            public List<Condition> Conditions = new List<Condition>();
        }

        private class Condition
        {
            public (string, int) ElementNumber;
            public string Operation = "ALWAYS";
            public string Result;
        }
    }
}
