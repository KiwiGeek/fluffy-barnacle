namespace TwentyFifteen;

internal interface IDay
{
    public uint Index {get;}
    public void Process(string inputFile);
    public string PartOne { get; }
    public string PartTwo { get; }
    public string PartOneDescription { get; }
    public string PartTwoDescription { get; }
}