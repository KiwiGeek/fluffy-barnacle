using System.Text;

namespace TwentyFifteen;

public class Day10 : IDay
{
    public uint Index => 10;

    public string PartOne => FortyIterations.ToString();

    public string PartTwo => FiftyIterations.ToString();

    public string PartOneDescription => "Length after 40 iterations";

    public string PartTwoDescription => "Length after 50 iterations";

    private uint FortyIterations { get; set; }
    private uint FiftyIterations { get; set; }

    string LookAndSay(string input)
    {
        ReadOnlySpan<char> characters = input.AsSpan();

        uint iterator = uint.MinValue;
        StringBuilder sb = new();

        while (iterator < characters.Length)
        {
            // read the character at the iterator
            char? currentCharacter = characters[(int)iterator];
            uint currentLength = 1;

            // now, keep reading subsequent characters until either we run out
            // of input, or the character we read is different
            while (iterator+currentLength < characters.Length
                && characters[(int)(iterator+currentLength)] == currentCharacter)
            { currentLength++; }
            sb.Append(currentLength.ToString());
            sb.Append(currentCharacter);
            iterator += currentLength;
        }
        return sb.ToString();

    }

    public void Process(string inputFile)
    {
        List<string> input = File.ReadLines(inputFile).ToList();
        string working = input[0];
        ushort iterations = ushort.Parse(input[1]);

        for (ushort i = 0; i < iterations; i++)
        {
            working = LookAndSay(working);
        }
        FortyIterations = (uint)working.Length;

        ushort iterations2 = ushort.Parse(input[2]);
        for (ushort i = iterations; i < iterations2; i++)
        {
            working = LookAndSay(working);
        }
        FiftyIterations = (uint)working.Length;

    }
}

