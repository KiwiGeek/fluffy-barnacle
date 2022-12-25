using System.Text.RegularExpressions;

namespace TwentyFifteen;

internal class Day07 : IDay
{
    public uint Index => 7;

    public ushort Signal1 { get; set; }
    public ushort Signal2 { get; set; }

    public string PartOne => Signal1.ToString();

    public string PartTwo => Signal2.ToString();

    public string PartOneDescription => "Signal on wire \"a\"";

    public string PartTwoDescription => $"Signal on wire \"a\" after overriding \"b\" with {PartOne}";

    Dictionary<string, ushort> resolved = new();
    List<string> unresolved = new();

    private string inputFile = string.Empty;

    private void ResetIEnumerables()
    {
        List<string> input = File.ReadLines(inputFile).ToList<string>();
        unresolved.Clear();
        foreach (string s in input) { unresolved.Add(s); }
        resolved.Clear();
    }

    internal ushort ProcessWiring()
    {
        do
        {
            foreach (string s in unresolved.ToList())
            {
                // parse the current instruction.
                Regex instructionDecoder = new("^(?:(?<opr_1>.*?) *(?<instr>AND|OR|RSHIFT|NOT|LSHIFT) )?(?<opr_2>.*?) -> (?<dst>.*?)$");
                Match m = instructionDecoder.Match(s);

                // decode any potential operands
                string opr1Name = m.Groups["opr_1"].Value;
                ushort? opr1Value = null;
                // if oppr1Name is a number, then it's a literal.
                // if oppr1Name is a string, then it's a wire. If resolved doesn't contain that wire, then value should be null.
                if (ushort.TryParse(opr1Name, out _))
                {
                    // it's a literal.
                    opr1Value = ushort.Parse(opr1Name);
                }
                else
                {
                    // it's a wire.
                    if (resolved.ContainsKey(opr1Name))
                    {
                        opr1Value = resolved[opr1Name];
                    }
                }

                string opr2Name = m.Groups["opr_2"].Value;
                ushort? opr2Value = null;
                // if oppr1Name is a number, then it's a literal.
                // if oppr1Name is a string, then it's a wire. If resolved doesn't contain that wire, then value should be null.
                if (ushort.TryParse(opr2Name, out _))
                {
                    // it's a literal.
                    opr2Value = ushort.Parse(opr2Name);
                }
                else
                {
                    // it's a wire.
                    if (resolved.ContainsKey(opr2Name))
                    {
                        opr2Value = resolved[opr2Name];
                    }
                }

                string dst = m.Groups["dst"].Value;
                string instruction = m.Groups["instr"].Value;

                switch (instruction)
                {
                    case "" when opr2Value.HasValue:
                        // It's a literalToWire, or wireToWire
                        resolved.Add(dst, opr2Value.Value);
                        unresolved.Remove(s);
                        break;
                    case "AND" when opr1Value.HasValue && opr2Value.HasValue:
                        resolved.Add(dst, (ushort)(opr1Value.Value & opr2Value.Value));
                        unresolved.Remove(s);
                        break;
                    case "OR" when opr1Value.HasValue && opr2Value.HasValue:
                        resolved.Add(dst, (ushort)(opr1Value.Value | opr2Value.Value));
                        unresolved.Remove(s);
                        break;
                    case "LSHIFT" when opr1Value.HasValue && opr2Value.HasValue:
                        resolved.Add(dst, (ushort)(opr1Value.Value << opr2Value.Value));
                        unresolved.Remove(s);
                        break;
                    case "RSHIFT" when opr1Value.HasValue && opr2Value.HasValue:
                        resolved.Add(dst, (ushort)(opr1Value.Value >> opr2Value.Value));
                        unresolved.Remove(s);
                        break;
                    case "NOT" when opr2Value.HasValue:
                        resolved.Add(dst, (ushort)(~opr2Value.Value));
                        unresolved.Remove(s);
                        break;
                }

            }

        } while (unresolved.Count > 0);

        return resolved["a"];
    }

    public void Process(string input)
    {
        inputFile = input;
        ResetIEnumerables();
        Signal1 = ProcessWiring();

        ResetIEnumerables();
        resolved.Add("b", Signal1);
        unresolved.RemoveAll(f=> f.EndsWith(" -> b"));
        Signal2 = ProcessWiring();
    }
}
