using System.Text.RegularExpressions;

namespace TwentyFifteen;

internal class Day15 : IDay
{
    public uint Index => 15;
    public string PartOne => HighestScoringCookie.ToString();
    public string PartTwo => HighestScoringCookieOfExactly500Calories.ToString();
    public string PartOneDescription => "Score of highest-scoring cookie";
    public string PartTwoDescription => "Score of highest-scoring cookie of exactly 500 calories";
    private long HighestScoringCookie { get; set; } = uint.MinValue;
    private long HighestScoringCookieOfExactly500Calories { get; set; } = uint.MinValue;

    private record Ingredient(int Capacity, int Durability, int Flavor, int Texture, int Calories);

    private record Recipe(Ingredient Ingredient1, int Ingredient1Quantity, 
                          Ingredient Ingredient2, int Ingredient2Quantity, 
                          Ingredient Ingredient3, int Ingredient3Quantity, 
                          Ingredient Ingredient4, int Ingredient4Quantity)
    {
        public long GetScore()
        {
            long capacity = (Ingredient1.Capacity * Ingredient1Quantity)
                            + (Ingredient2.Capacity * Ingredient2Quantity)
                            + (Ingredient3.Capacity * Ingredient3Quantity)
                            + (Ingredient4.Capacity * Ingredient4Quantity);
            if (capacity < 0) capacity = 0;
            long durability = (Ingredient1.Durability * Ingredient1Quantity)
                              + (Ingredient2.Durability * Ingredient2Quantity)
                              + (Ingredient3.Durability * Ingredient3Quantity)
                              + (Ingredient4.Durability * Ingredient4Quantity);
            if (durability < 0) durability = 0;
            long flavor = (Ingredient1.Flavor * Ingredient1Quantity)
                          + (Ingredient2.Flavor * Ingredient2Quantity)
                          + (Ingredient3.Flavor * Ingredient3Quantity)
                          + (Ingredient4.Flavor * Ingredient4Quantity);
            if (flavor < 0) flavor = 0;
            long texture = (Ingredient1.Texture * Ingredient1Quantity)
                           + (Ingredient2.Texture * Ingredient2Quantity)
                           + (Ingredient3.Texture * Ingredient3Quantity)
                           + (Ingredient4.Texture * Ingredient4Quantity);
            if (texture < 0) texture = 0;

            long score = capacity * durability * flavor * texture;

            return score;
        }

        public long GetCalories()
        {
            return (Ingredient1.Calories * Ingredient1Quantity)
                   + (Ingredient2.Calories * Ingredient2Quantity)
                   + (Ingredient3.Calories * Ingredient3Quantity)
                   + (Ingredient4.Calories * Ingredient4Quantity);
        }
    }


    private readonly Dictionary<string, Ingredient> _ingredients = new();

    public void Process(string inputFile)
    {
        List<string> lines = File.ReadLines(inputFile).ToList();

        Regex decoder = new(@"(?<ingredient>\w+): capacity (?<capacity>-?\d+), durability (?<durability>-?\d+), flavor (?<flavor>-?\d+), texture (?<texture>-*\d+), calories (?<calories>-*\d+)");
        foreach (string s in lines)
        {
            Match m = decoder.Match(s);
            Ingredient ingredient = new(int.Parse(m.Groups["capacity"].Value), 
                int.Parse(m.Groups["durability"].Value), 
                int.Parse(m.Groups["flavor"].Value), 
                int.Parse(m.Groups["texture"].Value),
                int.Parse(m.Groups["calories"].Value));
            _ingredients.Add(m.Groups["ingredient"].Value, ingredient);
        }

        for (int sprinkles = 1; sprinkles < 100; sprinkles++)
        {
            for (int peanutButter = 1; peanutButter < 100 - sprinkles; peanutButter++)
            {
                for (int frosting = 1; frosting < 100 - sprinkles - peanutButter; frosting++)
                {
                    int sugar = 100 - sprinkles - peanutButter - frosting;

                    Recipe r = new (_ingredients["Sprinkles"], sprinkles,
                        _ingredients["PeanutButter"], peanutButter,
                        _ingredients["Frosting"], frosting,
                        _ingredients["Sugar"], sugar);
                    if (r.GetScore() > HighestScoringCookie) HighestScoringCookie = r.GetScore();

                    if (r.GetCalories() == 500 && r.GetScore() > HighestScoringCookieOfExactly500Calories)
                    {
                        HighestScoringCookieOfExactly500Calories = r.GetScore();
                    }
                }
            }
        }
    }
}
