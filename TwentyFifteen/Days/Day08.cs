
using System.Text.RegularExpressions;

namespace TwentyFifteen;


internal class Day08 : IDay
{
    public uint Index => 8;

    public string PartOne => UnescapedCharacterDelta.ToString();

    public string PartTwo => UltraEscapedCharacterDelta.ToString();

    public string PartOneDescription => "Unescaped character count delta";

    public string PartTwoDescription => "Ultra Escaped character count delta";

    public ushort UnescapedCharacterDelta { get; private set; }
    public ushort UltraEscapedCharacterDelta { get; private set; }

    public void Process(string inputFile)
    {
        List<string> input = File.ReadLines(inputFile).ToList<string>();

        ushort codeCharacterCount = 0;
        ushort unescapedCharacterCount = 0;
        ushort extraEscapedCharacterCount = 0;

        foreach (string s in input)
        {
            codeCharacterCount += (ushort)s.Length;
            unescapedCharacterCount += (ushort)((new Regex("\\\\x[0-9a-f]{2}").Replace(s[1..^1], " ").Replace("\\\"", "a").Replace("\\\\", "b")).Length);
            Regex r = new Regex("\\\"|\\\\");
            extraEscapedCharacterCount += (ushort)(s.Length + r.Matches(s).Count() + 2);
        }

        UnescapedCharacterDelta = (ushort)(codeCharacterCount - unescapedCharacterCount);
        UltraEscapedCharacterDelta = (ushort)(extraEscapedCharacterCount - codeCharacterCount);
    }
}

