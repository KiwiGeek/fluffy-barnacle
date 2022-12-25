namespace TwentyFifteen;

internal class Day03 : IDay
{
    public uint Index => 3;
    public int Year1 { get; set; }
    public int Year2 { get; set; }
    public string PartOne => Year1.ToString();
    public string PartTwo => Year2.ToString();
    public string PartOneDescription => "Houses Delivered To Year 1";
    public string PartTwoDescription => "Houses Delivered To Year 2";

    private string? input;

    public void Process(string inputFile)
    {
        input = File.ReadAllText(inputFile);

        Dictionary<(int x, int y), bool> visited = new();
        Dictionary<(int x, int y), bool> altVisited = new();
        bool altTurn = false;

        // take care of the implicit first house
        (int x, int y) currentLocation = new(0, 0);
        (int x, int y) altOneLocation = new(0, 0);
        (int x, int y) altTwoLocation = new(0, 0);
        visited[currentLocation] = true;

        foreach (char c in input)
        {
            if (c == '^') { currentLocation.y--; }
            if (c == 'v') { currentLocation.y++; }
            if (c == '<') { currentLocation.x--; }
            if (c == '>') { currentLocation.x++; }

            if (altTurn)
            {
                if (c == '^') { altOneLocation.y--; }
                if (c == 'v') { altOneLocation.y++; }
                if (c == '<') { altOneLocation.x--; }
                if (c == '>') { altOneLocation.x++; }
            }
            else
            {
                if (c == '^') { altTwoLocation.y--; }
                if (c == 'v') { altTwoLocation.y++; }
                if (c == '<') { altTwoLocation.x--; }
                if (c == '>') { altTwoLocation.x++; }
            }
            altVisited[altOneLocation] = true;
            altVisited[altTwoLocation] = true;
            altTurn = !altTurn;

            visited[currentLocation] = true;
        }
        Year1 = visited.Count;
        Year2 = altVisited.Count;
    }
}