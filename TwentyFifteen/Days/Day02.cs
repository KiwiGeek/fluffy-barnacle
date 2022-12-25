namespace TwentyFifteen;

internal class Day02 : IDay
{
    public ulong TotalPaper { get; set; }
    public ulong TotalRibbon { get; set; }

    public uint Index => 2;

    public string PartOne => TotalPaper.ToString();

    public string PartTwo => TotalRibbon.ToString();

    public string PartOneDescription => "Square Feet of Paper";

    public string PartTwoDescription => "Feet of Ribbon";

    private List<string>? input;

    public void Process(string inputFile)
    {
        input = File.ReadLines(inputFile).ToList<string>();

        foreach (string s in input)
        {
            ushort depth = ushort.Parse(s.Split('x')[0]);
            ushort width = ushort.Parse(s.Split('x')[1]);
            ushort height = ushort.Parse(s.Split('x')[2]);
            ushort side = (ushort)(depth * height);
            ushort front = (ushort)(width * height);
            ushort top = (ushort)(width * depth);
            ushort sidePerimeter = (ushort)(depth * 2 + height * 2);
            ushort frontPerimeter = (ushort)(width * 2 + height * 2);
            ushort topPerimeter = (ushort)(width * 2 + depth * 2);
            ushort smallestface = side;
            if (front < smallestface) { smallestface = front; }
            if (top < smallestface) { smallestface = top; }
            ushort paper = (ushort)(side * 2 + front * 2 + top * 2 + smallestface);
            TotalPaper += paper;
            ushort shortestPerimeter = sidePerimeter;
            if (frontPerimeter < shortestPerimeter) { shortestPerimeter = frontPerimeter; }
            if (topPerimeter < shortestPerimeter) { shortestPerimeter = topPerimeter; }
            TotalRibbon += (ulong)(shortestPerimeter + (depth * width * height));
        }
    }
}