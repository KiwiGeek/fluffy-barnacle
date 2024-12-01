namespace TwentyFifteen;

internal class Day17 : IDay
{
    public uint Index => 17;
    public string PartOne => CombinationsOfContainers.ToString();
    public string PartTwo => CombinationsOfMinimumContainers.ToString();
    public string PartOneDescription => "Combinations of containers";
    public string PartTwoDescription => "Number of combinations of minimum containers";
    private uint CombinationsOfContainers { get; set; }
    private uint _minimumContainersRequired = uint.MaxValue;
    private uint CombinationsOfMinimumContainers { get; set; } = uint.MaxValue;

    private uint FindCombinations(List<uint> sortedContainers, uint target, string pathSoFar)
    {

        // we can start by removing any items from the list that are 
        sortedContainers.RemoveAll(f => f > target);

        // we're now in one of three scenarios:
        // 1) The sum of the remaining entries is exactly the target. So return 1.
        // 2) the sum of the remaining entries is less than the target. So return 0, we can't get there from here.
        // 3) The sum of the remaining entries is more than the target, so return the sum of:
        //      a) this method called again, with the first item removed, and the same target, and
        //      b) this method called again, with the first item removed, and a target = current-target minus the removed first item.

        if (sortedContainers.Sum(f => f) == target)
        {
            string thisPath = $"{string.Join(",", sortedContainers.Select(x => x.ToString()).ToArray())}{pathSoFar}";
            if (thisPath.StartsWith(',')) thisPath = thisPath[1..];
            uint numberOfContainers = Convert.ToUInt32(thisPath.Split(',').Length)+ 1;
            if (numberOfContainers < _minimumContainersRequired)
            {
                _minimumContainersRequired = numberOfContainers;
                CombinationsOfMinimumContainers = 1;
            } 
            else if (numberOfContainers == _minimumContainersRequired)
            {
                CombinationsOfMinimumContainers += 1;
            }

            return 1;
        }
        if (sortedContainers.Sum(f => f) < target) return 0;

        uint firstContainer = sortedContainers.First();
        return FindCombinations(sortedContainers.Skip(1).Take(sortedContainers.Count - 1).ToList(), target, pathSoFar) +
               FindCombinations(sortedContainers.Skip(1).Take(sortedContainers.Count - 1).ToList(),
                   target - firstContainer, $"{pathSoFar},{firstContainer}");
    }


    public void Process(string inputFile)
    {
        List<string> lines = File.ReadLines(inputFile).ToList();
        uint target = uint.Parse(lines[0]);
        List<uint> sortedContainers = lines.Skip(1).OrderByDescending(uint.Parse).Select(uint.Parse).ToList();

        CombinationsOfContainers = FindCombinations(sortedContainers, target, string.Empty);
    }

}