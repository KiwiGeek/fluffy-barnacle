namespace TwentyFifteen;

internal class Day01 : IDay
{
    public long Floor { get; set; }
    public ulong FirstBasementIndex { get; set; }

    public string PartOne => Floor.ToString();

    public string PartTwo => FirstBasementIndex.ToString();

    public string PartOneDescription => "Final Floor";

    public string PartTwoDescription => "First Step for Basement";

    public uint Index => 1;

    private string? input;

    public void Process(string inputFile)
    {
        input = File.ReadAllText(inputFile);
        System.Diagnostics.Debug.Assert(input != null);

        ulong index = 0;
        foreach (char c in input)
        {
            index++;
            Floor += c == '(' ? 1 : -1;
            if (Floor < 0 && FirstBasementIndex == 0) { FirstBasementIndex = index; }
        }
    }
}