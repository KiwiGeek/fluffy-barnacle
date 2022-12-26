using System.Text.RegularExpressions;

namespace TwentyFifteen;

internal class Day13 : IDay
{
    public uint Index => 13;
    public string PartOne => TotalOptimalChangeInHappiness.ToString();
    public string PartTwo => TotalOptimalChangeInHappinessWithMe.ToString();
    public string PartOneDescription => "Total change in happiness for the optimal seating";
    public string PartTwoDescription => "Total change in happiness for the optimal seating including me";
    public int TotalOptimalChangeInHappiness { get; private set; } = Int32.MinValue;
    public int TotalOptimalChangeInHappinessWithMe { get; private set; } = Int32.MinValue;

    private class PersonHappiness
    {
        public string Person { get; set; }
        public string Neighbour { get; set; }
        public int Change { get; set; }
    }

    private Dictionary<(string Person, string Neighbor), int> peopleHappinesses = new();
    private List<string> people = new();

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
                happiness += peopleHappinesses
                                 .Single(f => (f.Key.Person == combos[i] && f.Key.Neighbor == combos[rightNeighbor]))
                                 .Value +
                             peopleHappinesses.Single(f =>
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
            peopleHappinesses.Add((g["person"].Value, g["neighbor"].Value), (g["direction"].Value == "lose" ? -1 : 1) * int.Parse(g["delta"].ToString()));
            if (!people.Contains(g["person"].Value)) { people.Add(g["person"].Value); }
        }

        IEnumerable<string[]> peoplePermutations = people.Permutate<string>();
        TotalOptimalChangeInHappiness = OptimizePermutations(peoplePermutations);

        // Add myself as an option next to everyone, and everyone think I'm meh
        foreach (string p in people)
        {
            peopleHappinesses.Add(new ("me", p), 0);
            peopleHappinesses.Add(new (p, "me"), 0);
        }
        people.Add("me");

        peoplePermutations = people.Permutate<string>();
        TotalOptimalChangeInHappinessWithMe = OptimizePermutations(peoplePermutations);

    }


}
