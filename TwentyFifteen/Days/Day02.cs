namespace TwentyFifteen;

internal class Day02 : IDay
{
    private ulong TotalPaper { get; set; }
    private ulong TotalRibbon { get; set; }

    public uint Index => 2;

    public string PartOne => TotalPaper.ToString();

    public string PartTwo => TotalRibbon.ToString();

    public string PartOneDescription => "Square Feet of Paper";

    public string PartTwoDescription => "Feet of Ribbon";

    private List<string>? _input;

    public void Process(string inputFile)
    {
        _input = File.ReadLines(inputFile).ToList();

        foreach (string s in _input)
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
            ushort smallestFace = side;
            if (front < smallestFace) { smallestFace = front; }
            if (top < smallestFace) { smallestFace = top; }
            ushort paper = (ushort)(side * 2 + front * 2 + top * 2 + smallestFace);
            TotalPaper += paper;
            ushort shortestPerimeter = sidePerimeter;
            if (frontPerimeter < shortestPerimeter) { shortestPerimeter = frontPerimeter; }
            if (topPerimeter < shortestPerimeter) { shortestPerimeter = topPerimeter; }
            TotalRibbon += (ulong)(shortestPerimeter + (depth * width * height));
        }
    }
}