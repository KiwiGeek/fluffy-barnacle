using System.Text.RegularExpressions;

namespace TwentyFifteen;

internal class Day12 : IDay
{
    public uint Index => 12;

    public string PartOne => SumOfAllNumbers.ToString();

    public string PartTwo => string.Empty;

    public string PartOneDescription => "Sum of all numbers in document";

    public string PartTwoDescription => string.Empty;

    public int SumOfAllNumbers {get; private set;}

    private List<int> AllNumbersInString (string input)
    {
        List<int> results = new();

        foreach (Match match in Regex.Matches(input, @"-?\d+")) 
        {
            results.Add(int.Parse(match.Value));
        }
        return results;
    }

    public void Process(string inputFile)
    {
        string input = File.ReadAllText(inputFile);
        var numbers = AllNumbersInString(input);
        SumOfAllNumbers = numbers.Sum();
    }
}