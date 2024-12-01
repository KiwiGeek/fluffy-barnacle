namespace TwentyFifteen;

internal class Day01 : IDay
{
    private long Floor { get; set; }
    private ulong FirstBasementIndex { get; set; }

    public string PartOne => Floor.ToString();

    public string PartTwo => FirstBasementIndex.ToString();

    public string PartOneDescription => "Final Floor";

    public string PartTwoDescription => "First Step for Basement";

    public uint Index => 1;

    private string? _input;

    public void Process(string inputFile)
    {
        _input = File.ReadAllText(inputFile);
        System.Diagnostics.Debug.Assert(_input != null);

        ulong index = 0;
        foreach (char c in _input)
        {
            index++;
            Floor += c == '(' ? 1 : -1;
            if (Floor < 0 && FirstBasementIndex == 0) { FirstBasementIndex = index; }
        }
    }
}