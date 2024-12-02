namespace TwentyTwentyFour;

public class Day02 : IDay
{
    public uint Index => 2;

    private readonly List<int[]> _reports = [];
    public void Process(string inputFile)
    {
        string[]? input = File.ReadAllLines(inputFile);
        System.Diagnostics.Debug.Assert(input != null);

        foreach (string s in input)
        {
            string[] tokens = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            _reports.Add(tokens.Select(int.Parse).ToArray());
        }
    }

    public string PartOne
    {
        get
        {
            uint safeFloors = 0;
            
            foreach (int[] t in _reports)
            {
                int maxDifference = t.Zip(t.Skip(1), (a, b) => Math.Abs(b - a)).Max();
                int minDifference = t.Zip(t.Skip(1), (a, b) => Math.Abs(b - a)).Min();
                if (maxDifference > 3 || minDifference < 1) { continue; }
                
                // the deltas between the numbers are valid, now to ensure they're all moving in the same direction.
                IEnumerable<int> deltas = t.Zip(t.Skip(1), (a, b) => b - a).ToList();
                bool isIncreasing = deltas.All(delta => delta >= 0);
                bool isDecreasing = deltas.All(delta => delta <= 0);
                if (!isIncreasing && !isDecreasing) { continue; }
                
                // if we're here, the list is valid, so increment the safe floor count
                safeFloors++;
            }
            
            return safeFloors.ToString();
        }
    }

    public string PartTwo
    {
        get
        {
            uint safeFloors = 0;
            
            foreach (int[] t in _reports)
            {
                
                // we now want to create a loop across this list of ints. Each time running the check, and removing
                // _one_ number from it. Then we check to see if it's safe, if it is, the whole thing is safe
                // otherwise we continue. 
                bool isThisFloorAlreadySafe = false;
                for (int i = 0; i < t.Length; i++)
                {
                    if (isThisFloorAlreadySafe) { continue; }
                    var arrayWellActOn = t.Take(i).ToList();
                    arrayWellActOn.AddRange(t.Skip(i + 1).Take(100));
                    
                    int maxDifference = arrayWellActOn.Zip(arrayWellActOn.Skip(1), (a, b) => Math.Abs(b - a)).Max();
                    int minDifference = arrayWellActOn.Zip(arrayWellActOn.Skip(1), (a, b) => Math.Abs(b - a)).Min();
                    if (maxDifference > 3 || minDifference < 1) { continue; }
                    
                    // the deltas between the numbers are valid, now to ensure they're all moving in the same direction.
                    IEnumerable<int> deltas = arrayWellActOn.Zip(arrayWellActOn.Skip(1), (a, b) => b - a).ToList();
                    bool isIncreasing = deltas.All(delta => delta >= 0);
                    bool isDecreasing = deltas.All(delta => delta <= 0);
                    if (!isIncreasing && !isDecreasing) { continue; }
                    
                    // if we're here, the list is valid, so increment the safe floor count
                    safeFloors++;
                    isThisFloorAlreadySafe = true;
                }
            }
            
            return safeFloors.ToString();
        }
    }
    public string PartOneDescription => "Safe Reports";
    public string PartTwoDescription => "Safe floor-dampened reports";
}