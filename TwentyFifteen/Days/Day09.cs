using System.Text.RegularExpressions;

namespace TwentyFifteen;

public class Day09 : IDay
{
    public uint Index => 9;

    public string PartOne => MinimumDistance.ToString();

    public string PartTwo => MaximumDistance.ToString();

    public ushort MinimumDistance { get; private set; } = ushort.MaxValue;
    public ushort MaximumDistance { get; private set; } = ushort.MinValue;

    public string PartOneDescription => "Minimum Distance";

    public string PartTwoDescription => "Maximum Distance";

    public void Process(string inputFile)
    {
        List<string> cities = new();
        var cityDistances = new Dictionary<(string first, string second), ushort>();

        // Parse the obnoxious file.
        List<string> input = File.ReadLines(inputFile).ToList<string>();
        foreach (string s in input)
        {
            Match mc = new Regex("^(?<first>\\w*) to (?<second>\\w*) = (?<distance>\\d*)$").Match(s);
            if (!cities.Any(f => f == mc.Groups["first"].Value)) { cities.Add(mc.Groups["first"].Value); }
            if (!cities.Any(f => f == mc.Groups["second"].Value)) { cities.Add(mc.Groups["second"].Value); }
            cityDistances.Add((mc.Groups["first"].Value, mc.Groups["second"].Value), ushort.Parse(mc.Groups["distance"].Value));
        }

        var cityPermutations = cities.Permutate<string>();

        foreach (var cp in cityPermutations)
        {
            ushort workingDistance = 0;
            for (int i = 1; i < cp.Count(); i++)
            {
                workingDistance += cityDistances.Single(f => (f.Key.first == cp[i] || f.Key.second == cp[i])
                                                        && (f.Key.first == cp[i - 1] || f.Key.second == cp[i - 1])
                                                        ).Value;
            }
            if (workingDistance > MaximumDistance) { MaximumDistance = workingDistance; }
            if (workingDistance < MinimumDistance) { MinimumDistance = workingDistance; }
        }

    }

}
