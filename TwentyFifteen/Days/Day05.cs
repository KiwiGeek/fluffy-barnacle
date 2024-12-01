namespace TwentyFifteen;

internal class Day05 : IDay
{
    public uint Index => 5;
    public string PartOne => NiceStrings1.ToString();
    public string PartTwo => NiceStrings2.ToString();
    private uint NiceStrings1 { get; set; }
    private uint NiceStrings2 { get; set; }

    public string PartOneDescription => "Number of nice strings (method 1)";

    public string PartTwoDescription => "Number of nice strings (method 2)";


    bool HasAtLeastThreeVowels(string s)
    {
        System.Text.RegularExpressions.Regex threeVowels = new("[aeiou].*[aeiou].*[aeiou]");
        return threeVowels.Match(s).Success;
    }

    bool HasRepeatedLetters(string s)
    {
        System.Text.RegularExpressions.Regex doubleCharacters = new("(\\w)\\1{1}");
        return doubleCharacters.Match(s).Success;
    }

    bool NoBadStrings(string s)
    {
        System.Text.RegularExpressions.Regex doubleCharacters = new("ab|cd|pq|xy");
        return !doubleCharacters.Match(s).Success;
    }

    bool HasRepeatedPairOfCharacters(string s)
    {
        System.Text.RegularExpressions.Regex doubleCharacters = new("(\\w\\w).*\\1");
        return doubleCharacters.Match(s).Success;
    }

    bool HasRepeatedCharacterWithOneInbetween(string s)
    {
        System.Text.RegularExpressions.Regex doubleCharacters = new("(\\w).\\1");
        return doubleCharacters.Match(s).Success;
    }


    public void Process(string inputFile)
    {
        List<string> input = File.ReadLines(inputFile).ToList();

        foreach (string s in input)
        {
            if (HasAtLeastThreeVowels(s) && HasRepeatedLetters(s) && NoBadStrings(s)) { NiceStrings1++; }
            if (HasRepeatedPairOfCharacters(s) && HasRepeatedCharacterWithOneInbetween(s)) { NiceStrings2++; }
        }
    }
}