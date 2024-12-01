using System.Text.RegularExpressions;

namespace TwentyFifteen;

internal class Day16 : IDay
{
    public uint Index => 16;
    public string PartOne => AuntSue.ToString();
    public string PartTwo => FuzzyAuntSue.ToString();
    public string PartOneDescription => "Aunt sue who gave the gift";
    public string PartTwoDescription => "Fuzzy Aunt sue who gave the gift";
    private uint AuntSue { get; set; }
    private uint FuzzyAuntSue { get; set; }

    private record AuntSueFact
    {
        public uint? AuntSueNumber { get; set; }
        public uint? Children { get; set; }
        public uint? Cats { get; set; }
        public uint? Samoyeds { get; set; }
        public uint? Pomeranians { get; set; }
        public uint? Akitas { get; set; }
        public uint? Vizslas { get; set; }
        public uint? Goldfish { get; set; }
        public uint? Trees { get; set; }
        public uint? Cars { get; set; }
        public uint? Perfumes { get; set; }
    }

    private readonly List<AuntSueFact> _auntiesSue = new();


    public void Process(string inputFile)
    {
        List<string> lines = File.ReadLines(inputFile).ToList();
        Regex decoder = new(@"Sue (?<sue>\d+): (?<fact1>\w+): (?<value1>\d+), (?<fact2>\w+): (?<value2>\d+), (?<fact3>\w+): (?<value3>\d+)");
        foreach (string s in lines)
        {
            AuntSueFact asf = new ();
            Match m = decoder.Match(s);
            asf.AuntSueNumber = uint.Parse(m.Groups["sue"].Value);
            foreach ((string fact, uint value) in new[]
                     {
                         (m.Groups["fact1"].Value, uint.Parse(m.Groups["value1"].Value)),
                         (m.Groups["fact2"].Value, uint.Parse(m.Groups["value2"].Value)),
                         (m.Groups["fact3"].Value, uint.Parse(m.Groups["value3"].Value))
                     })
            {
                switch (fact)
                {
                    case "children":
                        asf.Children = value;
                        break;
                    case "cats":
                        asf.Cats = value;
                        break;
                    case "samoyeds":
                        asf.Samoyeds = value;
                        break;
                    case "pomeranians":
                        asf.Pomeranians = value;
                        break;
                    case "akitas":
                        asf.Akitas = value;
                        break;
                    case "vizslas":
                        asf.Vizslas = value;
                        break;
                    case "goldfish":
                        asf.Goldfish = value;
                        break;
                    case "trees":
                        asf.Trees = value;
                        break;
                    case "cars":
                        asf.Cars = value;
                        break;
                    case "perfumes":
                        asf.Perfumes = value;
                        break;
                }
            }
            _auntiesSue.Add(asf);
        }

        AuntSue = _auntiesSue.Single(f =>
            (!f.Children.HasValue || f.Children.Value == 3) &&
            (!f.Cats.HasValue || f.Cats.Value == 7) &&
            (!f.Samoyeds.HasValue || f.Samoyeds.Value == 2) &&
            (!f.Pomeranians.HasValue || f.Pomeranians.Value == 3) &&
            (!f.Akitas.HasValue || f.Akitas.Value == 0) &&
            (!f.Vizslas.HasValue || f.Vizslas.Value == 0) &&
            (!f.Goldfish.HasValue || f.Goldfish.Value == 5) &&
            (!f.Trees.HasValue || f.Trees.Value == 3) &&
            (!f.Cars.HasValue || f.Cars.Value == 2) &&
            (!f.Perfumes.HasValue || f.Perfumes.Value == 1)).AuntSueNumber!.Value;

        FuzzyAuntSue = _auntiesSue.Single(f =>
            (!f.Children.HasValue || f.Children.Value == 3) &&
            (!f.Cats.HasValue || f.Cats.Value > 7) &&
            (!f.Samoyeds.HasValue || f.Samoyeds.Value == 2) &&
            (!f.Pomeranians.HasValue || f.Pomeranians.Value < 3) &&
            (!f.Akitas.HasValue || f.Akitas.Value == 0) &&
            (!f.Vizslas.HasValue || f.Vizslas.Value == 0) &&
            (!f.Goldfish.HasValue || f.Goldfish.Value < 5) &&
            (!f.Trees.HasValue || f.Trees.Value > 3) &&
            (!f.Cars.HasValue || f.Cars.Value == 2) &&
            (!f.Perfumes.HasValue || f.Perfumes.Value == 1)).AuntSueNumber!.Value;

    }
}