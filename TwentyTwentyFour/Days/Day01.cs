namespace TwentyTwentyFour;

public class Day01 : IDay
{
    public uint Index => 1;

    private List<uint> _list1 = [];
    private List<uint> _list2 = [];
    public void Process(string inputFile)
    {
        string[]? input = File.ReadAllLines(inputFile);
        System.Diagnostics.Debug.Assert(input != null);

        foreach (string s in input)
        {
            string[] tokens = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            _list1.Add(uint.Parse(tokens[0]));
            _list2.Add(uint.Parse(tokens[1]));
        }
        
        // sort the lists.
        _list1 = _list1.OrderBy(x => x).ToList();
        _list2 = _list2.OrderBy(x => x).ToList();
    }

    public string PartOne
    {
        get
        {
            uint difference = 0;
            for (int i = 0; i < _list1.Count; i++)
            {
                difference += Convert.ToUInt32(Math.Abs((int)_list1[i] - (int)_list2[i]));
            }
            return difference.ToString();
        }
    }

    public string PartTwo
    {
        get
        {
            uint similarity = 0;
            foreach (uint value1 in _list1)
            {
                // count how often that value appears in list2
                uint value2 = value1;
                uint occurrences = Convert.ToUInt32(_list2.Count(f => f.Equals(value2)));

                similarity += (occurrences * value1);
            }
            
            return similarity.ToString();
        }
    }
    public string PartOneDescription => "Total Distance";
    public string PartTwoDescription => "Similarity Score";
}