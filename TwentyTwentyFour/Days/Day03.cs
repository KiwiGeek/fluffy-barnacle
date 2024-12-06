using System.Text.RegularExpressions;

namespace TwentyTwentyFour;

public partial class Day03 : IDay
{
    public uint Index => 3;
    public void Process(string inputFile)
    {
        string[] input = File.ReadAllLines(inputFile);

        foreach (var line in input)
        {
            MatchCollection matches = PartOneRegex().Matches(line);
            // for part one, find all instances of the Regex
            foreach (Match match in matches)
            {
                uint first = uint.Parse(match.Groups[1].Value);
                uint second = uint.Parse(match.Groups[2].Value);
                _partOne += (first * second);
            }
        }

        List<Match> partTwoMatches = [];
        foreach (var line in input)
        {
            MatchCollection matches = PartTwoRegex().Matches(line);
            partTwoMatches.AddRange(matches);
        }

        bool isEnabled = true;
        foreach (Match match in partTwoMatches)
        {
            if (match.ToString() == "do()")
            {
                isEnabled = true;
                continue;
            }

            if (match.ToString() == "don't()")
            {
                isEnabled = false;
                continue;
            }

            if (!isEnabled) { continue; }
            
            uint first = uint.Parse(match.Groups[4].Value);
            uint second = uint.Parse(match.Groups[5].Value);
            _partTwo += (first * second);
            
        }
        
        
    }

    private uint _partOne;
    private uint _partTwo;
    public string PartOne => _partOne.ToString();
    public string PartTwo => _partTwo.ToString();
    public string PartOneDescription => "Sum of multiplications";
    public string PartTwoDescription => "Sum of multiplications with Do's and Don'ts";

    [GeneratedRegex(@"mul\((?<first>\d{1,3}),(?<second>\d{1,3})\)")]
    private static partial Regex PartOneRegex();
    
    [GeneratedRegex(@"(mul\\((?<first>\\d{1,3}),(?<second>\\d{1,3})\\))|(don't\\(\\))|(do\\(\\))")]
    private static partial Regex PartTwoRegex();
}