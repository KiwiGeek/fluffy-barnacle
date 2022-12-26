using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace TwentyFifteen;

internal class Day12 : IDay
{
    public uint Index => 12;

    public string PartOne => SumOfAllNumbers.ToString();

    public string PartTwo => SumOfAllNumbersIgnoringRed.ToString();

    public string PartOneDescription => "Sum of all numbers";

    public string PartTwoDescription => "Sum of all numbers ignoring reds";

    public int SumOfAllNumbers { get; private set; }

    public long SumOfAllNumbersIgnoringRed { get; private set; }

    private List<int> AllNumbersInString(string input)
    {
        List<int> results = new();

        foreach (Match match in Regex.Matches(input, @"-?\d+"))
        {
            results.Add(int.Parse(match.Value));
        }
        return results;
    }

    long SumNumbers(JToken token)
    {
        long sum = 0;

        switch (token.Type)
        {
            case JTokenType.Array:
            case JTokenType.Property:
                foreach (JToken t in token)
                {
                    sum += SumNumbers(t);
                }
                break;
            case JTokenType.Integer:
                sum = token.Value<int>();
                break;
            case JTokenType.Object:
                if (!token
                        .Any(f => f.Type == JTokenType.Property
                                  && f.First().Type != JTokenType.Array
                                  && f.First().Type != JTokenType.Object
                                  && f.First().Value<string>() == "red"))
                {
                    foreach (JToken t in token)
                    {
                        sum += SumNumbers(t);
                    }
                }

                break;
        }

        return sum;
    }



    private long SumNonRedNumbers(string inputFile)
    {
        string input = File.ReadAllText(inputFile);
        // input = @"[{""d"":""red"",""e"":[1,2,3,4],""f"":5}]";
        JArray root = JArray.Parse(input);
        return SumNumbers(root);
    }

    public void Process(string inputFile)
    {
        string input = File.ReadAllText(inputFile);
        var numbers = AllNumbersInString(input);
        SumOfAllNumbers = numbers.Sum();

        SumOfAllNumbersIgnoringRed = SumNonRedNumbers(inputFile);
    }
}