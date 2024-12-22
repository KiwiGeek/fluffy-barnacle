namespace TwentyTwentyFour;

public class Day07 : IDay
{

    private readonly List<(long target, List<long> numbers)> _equations = [];
    
    public uint Index => 7;
    public void Process(string inputFile)
    {
        _equations.AddRange(File.ReadAllLines(inputFile)
            .Select(input => 
            new ValueTuple<long, List<long>>(
                long.Parse(input.Split(':', StringSplitOptions.RemoveEmptyEntries).First()),
                input.Split(':', StringSplitOptions.TrimEntries).Last().Split(' ').Select(long.Parse).Reverse().ToList()
            )
        ));
    }
    
    List<long> GetPossibleSums(List<long> numbers, bool allowConcatenation)
        {
            List<long> result = [];
            if (numbers.Count == 1)
            {
                // should never get here, but just in case we were supplied a list with just one
                // input.
                return result.ToList();
            }
            if (numbers.Count == 2)
            {
                // we're down to two items.
                result.Add(numbers[0] * numbers[1]);
                result.Add(numbers[0] + numbers[1]);
                if (allowConcatenation) { result.Add(long.Parse($"{numbers[1]}{numbers[0]}")); }
                return result;
            }
            
            // if we're here, we have more than 2 numbers left. So tear off everything but the left
            // number, recurse, and then return the results + or *.
            List<long> temp = GetPossibleSums(numbers.Skip(1).ToList(), allowConcatenation);
            foreach (long number in temp)
            {
                result.Add(numbers[0] * number);
                result.Add(numbers[0] + number);
                if (allowConcatenation) { result.Add(long.Parse($"{number}{numbers[0]}")); }
            }
            
            return result;
        }
    
    public string PartOne => _equations
                .Where(equation => GetPossibleSums(equation.numbers, false).Contains(equation.target))
                .Sum(equation => equation.target)
                .ToString();
    
    public string PartTwo=> _equations
        .Where(equation => GetPossibleSums(equation.numbers, true).Contains(equation.target))
        .Sum(equation => equation.target)
        .ToString();
    
    public string PartOneDescription => "Total Calibration result (part one)";
    public string PartTwoDescription => "Total Calibration result (part two)";
}