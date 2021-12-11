(long floor, ulong firstBasement) = Day1();
Console.WriteLine("Day 1");
Console.WriteLine($"  Part 1 - Final Floor: {floor}");
Console.WriteLine($"  Part 2 - First Step for Basement: {firstBasement}");
(ulong squareFeetOfPaper, ulong feetOfRibbon) = Day2();
Console.WriteLine("Day 2");
Console.WriteLine($"  Part 1 - Square Feet of Paper: {squareFeetOfPaper}");
Console.WriteLine($"  Part 2 - Feet of Ribbon: {feetOfRibbon}");

(long floor, ulong firstBasementStep) Day1()
{
    string input = File.ReadAllText("Assets\\day1.txt");
    long floor = 0;
    ulong firstBasementIndex = 0;
    ulong index = 0;

    foreach (char c in input)
    {
        index++;
        floor += c == '(' ? 1 : -1;
        if (floor < 0 && firstBasementIndex == 0) { firstBasementIndex = index; }
    }

    return (floor, firstBasementIndex);
}

(ulong totalPaper, ulong totalRibbon) Day2() 
{
    ulong totalPaper = 0;
    ulong totalRibbon = 0;
    List<string> input = File.ReadLines("Assets\\day2.txt").ToList<string>();

    foreach(string s in input)
    {
        ushort depth=ushort.Parse(s.Split('x')[0]);
        ushort width=ushort.Parse(s.Split('x')[1]);
        ushort height=ushort.Parse(s.Split('x')[2]);
        ushort side=(ushort)(depth * height);
        ushort front=(ushort)(width * height);
        ushort top=(ushort)(width * depth);
        ushort sidePerimeter=(ushort)(depth*2 + height*2);
        ushort frontPerimeter=(ushort)(width*2 + height*2);
        ushort topPerimeter=(ushort)(width*2 + depth*2);
        ushort smallestface = side;
        if (front < smallestface) { smallestface = front; }
        if (top < smallestface) { smallestface = top; }
        ushort paper=(ushort)(side*2 + front*2 + top*2 + smallestface);
        totalPaper += paper;
        ushort shortestPerimeter = sidePerimeter;
        if (frontPerimeter < shortestPerimeter) { shortestPerimeter = frontPerimeter; }
        if (topPerimeter < shortestPerimeter) { shortestPerimeter = topPerimeter; }
        totalRibbon += (ulong)(shortestPerimeter + (depth * width * height));
    }

    return (totalPaper, totalRibbon);
}