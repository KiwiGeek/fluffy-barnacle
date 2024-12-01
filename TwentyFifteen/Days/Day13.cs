using System.Text.RegularExpressions;

namespace TwentyFifteen;

internal class Day13 : IDay
{
    public uint Index => 13;
    public string PartOne => TotalOptimalChangeInHappiness.ToString();
    public string PartTwo => TotalOptimalChangeInHappinessWithMe.ToString();
    public string PartOneDescription => "Total change in happiness for the optimal seating";
    public string PartTwoDescription => "Total change in happiness for the optimal seating including me";
    private int TotalOptimalChangeInHappiness { get; set; } = int.MinValue;
    private int TotalOptimalChangeInHappinessWithMe { get; set; } = int.MinValue;
    
    private readonly Dictionary<(string Person, string Neighbor), int> _peopleHappinesses = new();
    private readonly List<string> _people = [];

    private int OptimizePermutations(IEnumerable<string[]> peoplePermutations)
    {
        int optimalHappiness = int.MinValue;

        foreach (var combos in peoplePermutations)
        {
            int happiness = 0;
            for (int i = 0; i < combos.Length; i++)
            {
                int rightNeighbor = i + 1 > combos.Length - 1 ? 0 : i + 1;
                int leftNeighbor = i - 1 < 0 ? combos.Length - 1 : i - 1;
                happiness += _peopleHappinesses
                                 .Single(f => (f.Key.Person == combos[i] && f.Key.Neighbor == combos[rightNeighbor]))
                                 .Value +
                             _peopleHappinesses.Single(f =>
                                 (f.Key.Person == combos[i] && f.Key.Neighbor == combos[leftNeighbor])).Value;
            }

            if (happiness > optimalHappiness)
            {
                optimalHappiness = happiness;
            }

        }

        return optimalHappiness;
    }

    public void Process(string inputFile)
    {
        Regex decoderRegex = new (@"(?<person>\w+) would (?<direction>[g|l][a|o][i|s][n|e]) (?<delta>\d+) happiness units by sitting next to (?<neighbor>\w+).");
        foreach (string s in File.ReadLines(inputFile).ToList())
        {
            GroupCollection g = decoderRegex.Match(s).Groups;
            _peopleHappinesses.Add((g["person"].Value, g["neighbor"].Value), (g["direction"].Value == "lose" ? -1 : 1) * int.Parse(g["delta"].ToString()));
            if (!_people.Contains(g["person"].Value)) { _people.Add(g["person"].Value); }
        }

        IEnumerable<string[]> peoplePermutations = _people.Permutate();
        TotalOptimalChangeInHappiness = OptimizePermutations(peoplePermutations);

        // Add myself as an option next to everyone, and everyone think I'm meh
        foreach (string p in _people)
        {
            _peopleHappinesses.Add(new ("me", p), 0);
            _peopleHappinesses.Add(new (p, "me"), 0);
        }
        _people.Add("me");

        peoplePermutations = _people.Permutate();
        TotalOptimalChangeInHappinessWithMe = OptimizePermutations(peoplePermutations);

    }


}
