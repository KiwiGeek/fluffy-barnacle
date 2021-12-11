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
        totalPaper += paper;
        ushort shortestPerimeter = sidePerimeter;
        if (frontPerimeter < shortestPerimeter) { shortestPerimeter = frontPerimeter; }
        if (topPerimeter < shortestPerimeter) { shortestPerimeter = topPerimeter; }
        totalRibbon += (ulong)(shortestPerimeter + (depth * width * height));
    }

    return (totalPaper, totalRibbon);
}

(ulong year1, ulong year2) Day3()
{

    string input = File.ReadAllText("Assets\\day3.txt");
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

    return ((ulong)visited.Count, (ulong)altVisited.Count);

}


(long floor, ulong firstBasement) = Day1();
Console.WriteLine("Day 1");
Console.WriteLine($"  Part 1 - Final Floor: {floor}");
Console.WriteLine($"  Part 2 - First Step for Basement: {firstBasement}");
(ulong squareFeetOfPaper, ulong feetOfRibbon) = Day2();
Console.WriteLine("Day 2");
Console.WriteLine($"  Part 1 - Square Feet of Paper: {squareFeetOfPaper}");
Console.WriteLine($"  Part 2 - Feet of Ribbon: {feetOfRibbon}");
(ulong year1, ulong year2) = Day3();
Console.WriteLine("Day 3");
Console.WriteLine($"  Part 1 - Houses Delivered To Year 1: {year1}");
Console.WriteLine($"  Part 2 - Houses Delivered To Year 2: {year2}");