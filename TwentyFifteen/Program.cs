(long floor, ulong firstBasement) = Day1();
Console.WriteLine("Day 1");
Console.WriteLine($"  Part 1 - Final Floor: {floor}");
Console.WriteLine($"  Part 2 - First Step for Basement: {firstBasement}");

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