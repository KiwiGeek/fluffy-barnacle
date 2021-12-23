using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text.RegularExpressions;

#if !SKIP
(long floor, ulong firstBasementStep) Day1()
{
    string input = File.ReadAllText("./Assets/day1.txt");
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
    List<string> input = File.ReadLines("./Assets/day2.txt").ToList<string>();

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

    string input = File.ReadAllText("./Assets/day3.txt");
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

(uint fiveChar, uint sixChar) Day4()
{

    string CreateMD5Hash(string input)
    {
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }

    string secretKey = File.ReadAllText("./Assets/day4.txt");
    uint fiveCharHash = 0;
    uint sixCharHash = 0;
    string hashed = string.Empty;

    while (hashed.Length < 1 || hashed[..6] != "000000")
    {
        sixCharHash++;
        string source = $"{secretKey}{sixCharHash}";
        hashed = CreateMD5Hash(source);
        if (fiveCharHash == 0 && hashed[..5] == "00000") { fiveCharHash = sixCharHash; }
    }

    return (fiveCharHash, sixCharHash);
}

(ulong niceStrings1, ulong niceStrings2) Day5()
{

    bool HasAtLeastThreeVowels(string s)
    {
        System.Text.RegularExpressions.Regex threeVowels = new("[aeiou].*[aeiou].*[aeiou]");
        return threeVowels.Match(s).Success;
    }

    bool HasRepeatedLetters(string s)
    {
        System.Text.RegularExpressions.Regex doubleCharacters = new("(\\w)\\1{1}");
        return doubleCharacters.Match(s).Success;
    }

    bool NoBadStrings(string s)
    {
        System.Text.RegularExpressions.Regex doubleCharacters = new("ab|cd|pq|xy");
        return !doubleCharacters.Match(s).Success;
    }

    bool HasRepeatedPairOfCharacters(string s)
    {
        System.Text.RegularExpressions.Regex doubleCharacters = new("(\\w\\w).*\\1");
        return doubleCharacters.Match(s).Success;
    }

    bool HasRepeatedCharacterWithOneInbetween(string s)
    {
        System.Text.RegularExpressions.Regex doubleCharacters = new("(\\w).\\1");
        return doubleCharacters.Match(s).Success;
    }

    ulong niceStrings1 = 0;
    ulong niceStrings2 = 0;
    List<string> input = File.ReadLines("./Assets/day5.txt").ToList<string>();

    foreach (string s in input)
    {
        if (HasAtLeastThreeVowels(s) && HasRepeatedLetters(s) && NoBadStrings(s)) { niceStrings1++; }
        if (HasRepeatedPairOfCharacters(s) && HasRepeatedCharacterWithOneInbetween(s)) { niceStrings2++; }
    }

    return (niceStrings1, niceStrings2);

}

(ulong lightsOn, ulong lightBrightness) Day6()
{

    bool[,] lights = new bool[1000, 1000];
    uint[,] lightBrightness = new uint[1000, 1000];

    void setBlock(uint x1, uint y1, uint x2, uint y2, string instruction)
    {

        void setRow(uint x1, uint x2, uint y, string instruction)
        {

            void setLight(uint x, uint y, string instruction)
            {
                if (instruction == "turn on")
                {
                    lights[x, y] = true;
                    lightBrightness[x, y]++;
                }
                if (instruction == "turn off")
                {
                    lights[x, y] = false;
                    if (lightBrightness[x, y] > 0) { lightBrightness[x, y]--; }
                }
                if (instruction == "toggle")
                {
                    lights[x, y] = !lights[x, y];
                    lightBrightness[x, y] += 2;
                }
            }

            for (uint i = x1; i <= x2; i++)
            {
                setLight(i, y, instruction);
            }
        }

        for (uint i = y1; i <= y2; i++)
        {
            setRow(x1, x2, i, instruction);
        }
    }

    List<string> input = File.ReadLines("./Assets/day6.txt").ToList<string>();

    foreach (string s in input)
    {
        System.Text.RegularExpressions.Regex instructionDecoder = new("^(?<instr>(?:turn on)|(?:turn off)|(?:toggle)) (?<x1>\\d*),(?<y1>\\d*) through (?<x2>\\d*),(?<y2>\\d*)$");
        System.Text.RegularExpressions.Match match = instructionDecoder.Match(s);

        string instr = match.Groups["instr"].Value;
        uint x1 = uint.Parse(match.Groups["x1"].Value);
        uint x2 = uint.Parse(match.Groups["x2"].Value);
        uint y1 = uint.Parse(match.Groups["y1"].Value);
        uint y2 = uint.Parse(match.Groups["y2"].Value);

        setBlock(x1, y1, x2, y2, instr);
    }

    // count lights on
    ulong totallightsOn = 0;
    ulong totalBrightness = 0;

    for (int y = 0; y <= 999; y++)
    {
        for (int x = 0; x <= 999; x++)
        {
            if (lights[x, y]) totallightsOn++;
            totalBrightness += lightBrightness[x, y];
        }
    }

    return (totallightsOn, totalBrightness);

}



ushort Day7(string wireToReport, bool partTwo)
{

    List<string> input = File.ReadLines("./Assets/day7.txt").ToList<string>();

    Dictionary<string, ushort> resolved = new();
    List<string> unresolved = new();

    foreach (string s in input) { unresolved.Add(s); }

    if (partTwo)
    {
        resolved.Add("b", 16076);
        unresolved.Remove("19138 -> b");
    }

    do
    {
        foreach (string s in unresolved.ToList())
        {
            // parse the current instruction.
            Regex instructionDecoder = new("^(?:(?<opr_1>.*?) *(?<instr>AND|OR|RSHIFT|NOT|LSHIFT) )?(?<opr_2>.*?) -> (?<dst>.*?)$");
            Match m = instructionDecoder.Match(s);

            // decode any potential operands
            string opr1Name = m.Groups["opr_1"].Value;
            ushort? opr1Value = null;
            // if oppr1Name is a number, then it's a literal.
            // if oppr1Name is a string, then it's a wire. If resolved doesn't contain that wire, then value should be null.
            if (ushort.TryParse(opr1Name, out _))
            {
                // it's a literal.
                opr1Value = ushort.Parse(opr1Name);
            }
            else
            {
                // it's a wire.
                if (resolved.ContainsKey(opr1Name))
                {
                    opr1Value = resolved[opr1Name];
                }
            }

            string opr2Name = m.Groups["opr_2"].Value;
            ushort? opr2Value = null;
            // if oppr1Name is a number, then it's a literal.
            // if oppr1Name is a string, then it's a wire. If resolved doesn't contain that wire, then value should be null.
            if (ushort.TryParse(opr2Name, out _))
            {
                // it's a literal.
                opr2Value = ushort.Parse(opr2Name);
            }
            else
            {
                // it's a wire.
                if (resolved.ContainsKey(opr2Name))
                {
                    opr2Value = resolved[opr2Name];
                }
            }

            string dst = m.Groups["dst"].Value;
            string instruction = m.Groups["instr"].Value;

            switch (instruction)
            {
                case "" when opr2Value.HasValue:
                    // It's a literalToWire, or wireToWire
                    resolved.Add(dst, opr2Value.Value);
                    unresolved.Remove(s);
                    break;
                case "AND" when opr1Value.HasValue && opr2Value.HasValue:
                    resolved.Add(dst, (ushort)(opr1Value.Value & opr2Value.Value));
                    unresolved.Remove(s);
                    break;
                case "OR" when opr1Value.HasValue && opr2Value.HasValue:
                    resolved.Add(dst, (ushort)(opr1Value.Value | opr2Value.Value));
                    unresolved.Remove(s);
                    break;
                case "LSHIFT" when opr1Value.HasValue && opr2Value.HasValue:
                    resolved.Add(dst, (ushort)(opr1Value.Value << opr2Value.Value));
                    unresolved.Remove(s);
                    break;
                case "RSHIFT" when opr1Value.HasValue && opr2Value.HasValue:
                    resolved.Add(dst, (ushort)(opr1Value.Value >> opr2Value.Value));
                    unresolved.Remove(s);
                    break;
                case "NOT" when opr2Value.HasValue:
                    resolved.Add(dst, (ushort)(~opr2Value.Value));
                    unresolved.Remove(s);
                    break;
            }

        }

    } while (unresolved.Count > 0);

    return resolved[wireToReport];
}

(ushort part1, ushort Part2) Day8()
{
    List<string> input = File.ReadLines("./Assets/day8.txt").ToList<string>();

    ushort codeCharacterCount = 0;
    ushort unescapedCharacterCount = 0;
    ushort extraEscapedCharacterCount = 0;

    foreach (string s in input)
    {
        codeCharacterCount += (ushort)s.Length;
        unescapedCharacterCount += (ushort)((new Regex("\\\\x[0-9a-f]{2}").Replace(s[1..^1], " ").Replace("\\\"", "a").Replace("\\\\", "b")).Length);
        Regex r = new Regex("\\\"|\\\\");
        extraEscapedCharacterCount += (ushort)(s.Length + r.Matches(s).Count() + 2);
    }

    return ((ushort)(codeCharacterCount - unescapedCharacterCount), (ushort)(extraEscapedCharacterCount - codeCharacterCount));
}

#endif

(ushort minimum, ushort maximum) Day9()
{
    
    List<string> cities = new ();
    var cityDistances = new Dictionary<(string first, string second), ushort>();

    // Parse the obnoxious file.
    List<string> input = File.ReadLines("./Assets/day9.txt").ToList<string>();
    foreach (string s in input) 
    {
        Match mc = new Regex("^(?<first>\\w*) to (?<second>\\w*) = (?<distance>\\d*)$").Match(s);
        if (!cities.Any(f=> f == mc.Groups["first"].Value)) { cities.Add(mc.Groups["first"].Value);}
        if (!cities.Any(f=> f == mc.Groups["second"].Value)) { cities.Add(mc.Groups["second"].Value);}
        cityDistances.Add((mc.Groups["first"].Value, mc.Groups["second"].Value), ushort.Parse(mc.Groups["distance"].Value));
    }

    var cityPermutations = cities.Permutate<string>();

    ushort minimumDistance = ushort.MaxValue;
    ushort maximumDisance = ushort.MinValue;

    foreach (var cp in cityPermutations) 
    {
        ushort workingDistance = 0;
        for (int i=1; i < cp.Count(); i++) 
        {
            workingDistance += cityDistances.Single(f => (f.Key.first == cp[i] || f.Key.second == cp[i]) 
                                                    && (f.Key.first == cp[i-1] || f.Key.second == cp[i-1])
                                                    ).Value;
        }
        if (workingDistance > maximumDisance) {maximumDisance = workingDistance;}
        if (workingDistance < minimumDistance) {minimumDistance = workingDistance;}
    }

    return (minimumDistance, maximumDisance);
}

#if !SKIP
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
(uint hash5, uint hash6) = Day4();
Console.WriteLine("Day 4");
Console.WriteLine($"  Part 1 - Lowest 5 Character hash: {hash5}");
Console.WriteLine($"  Part 2 - Lowest 6 Character hash: {hash6}");
(ulong niceStrings1, ulong niceStrings2) = Day5();
Console.WriteLine("Day 5");
Console.WriteLine($"  Part 1 - Number of nice strings (method 1): {niceStrings1}");
Console.WriteLine($"  Part 2 - Number of nice strings (method 2): {niceStrings2}");
(ulong lightsOn, ulong lightBrightness) = Day6();
Console.WriteLine("Day 6");
Console.WriteLine($"  Part 1 - Lights on: {lightsOn}");
Console.WriteLine($"  Part 2 - Lights brightness: {lightBrightness}");
(ushort wireSignal1, ushort wireSignal2) = (Day7("a", false), Day7("a", true));
Console.WriteLine("Day 7");
Console.WriteLine($"  Part 1 - Signal on wire \"a\": {wireSignal1}");
Console.WriteLine($"  Part 2 - Signal on wire \"a\" after overriding \"b\" with {wireSignal1}: {wireSignal2}");
(ushort unescapedCharacterDelta, ushort ultraEscapedCharactedDelta) = Day8();
Console.WriteLine("Day 8");
Console.WriteLine($"  Part 1 - Unescaped character count delta: {unescapedCharacterDelta}");
Console.WriteLine($"  Part 2 - Ultra Escaped character count delta: {ultraEscapedCharactedDelta}");
#endif
(ushort minimumDistance, ushort maximumDisance) = Day9();
Console.WriteLine("Day 9");
Console.WriteLine($"  Part 1 - Minimum Distance: {minimumDistance}");
Console.WriteLine($"  Part 2 - Maximum Distance: {maximumDisance}");