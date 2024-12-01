using System.Text.RegularExpressions;

namespace TwentyFifteen;

internal class Day07 : IDay
{
    public uint Index => 7;

    private ushort Signal1 { get; set; }
    private ushort Signal2 { get; set; }

    public string PartOne => Signal1.ToString();

    public string PartTwo => Signal2.ToString();

    public string PartOneDescription => "Signal on wire \"a\"";

    public string PartTwoDescription => $"Signal on wire \"a\" after overriding \"b\" with {PartOne}";

    private readonly Dictionary<string, ushort> _resolved = [];
    private readonly List<string> _unresolved = [];

    private string _inputFile = string.Empty;

    private void ResetEnumerables()
    {
        List<string> input = File.ReadLines(_inputFile).ToList();
        _unresolved.Clear();
        foreach (string s in input) { _unresolved.Add(s); }
        _resolved.Clear();
    }

    private ushort ProcessWiring()
    {
        do
        {
            foreach (string s in _unresolved.ToList())
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
                    if (_resolved.TryGetValue(opr1Name, out ushort value))
                    {
                        opr1Value = value;
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
                    if (_resolved.TryGetValue(opr2Name, out ushort value))
                    {
                        opr2Value = value;
                    }
                }

                string dst = m.Groups["dst"].Value;
                string instruction = m.Groups["instr"].Value;

                switch (instruction)
                {
                    case "" when opr2Value.HasValue:
                        // It's a literalToWire, or wireToWire
                        _resolved.Add(dst, opr2Value.Value);
                        _unresolved.Remove(s);
                        break;
                    case "AND" when opr1Value.HasValue && opr2Value.HasValue:
                        _resolved.Add(dst, (ushort)(opr1Value.Value & opr2Value.Value));
                        _unresolved.Remove(s);
                        break;
                    case "OR" when opr1Value.HasValue && opr2Value.HasValue:
                        _resolved.Add(dst, (ushort)(opr1Value.Value | opr2Value.Value));
                        _unresolved.Remove(s);
                        break;
                    case "LSHIFT" when opr1Value.HasValue && opr2Value.HasValue:
                        _resolved.Add(dst, (ushort)(opr1Value.Value << opr2Value.Value));
                        _unresolved.Remove(s);
                        break;
                    case "RSHIFT" when opr1Value.HasValue && opr2Value.HasValue:
                        _resolved.Add(dst, (ushort)(opr1Value.Value >> opr2Value.Value));
                        _unresolved.Remove(s);
                        break;
                    case "NOT" when opr2Value.HasValue:
                        _resolved.Add(dst, (ushort)(~opr2Value.Value));
                        _unresolved.Remove(s);
                        break;
                }

            }

        } while (_unresolved.Count > 0);

        return _resolved["a"];
    }

    public void Process(string input)
    {
        _inputFile = input;
        ResetEnumerables();
        Signal1 = ProcessWiring();

        ResetEnumerables();
        _resolved.Add("b", Signal1);
        _unresolved.RemoveAll(f=> f.EndsWith(" -> b"));
        Signal2 = ProcessWiring();
    }
}
