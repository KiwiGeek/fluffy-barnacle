using System.Diagnostics;

namespace TwentyTwentyFour;

public class Day05 : IDay
{
    public uint Index => 5;

    private readonly Dictionary<int, List<int>?> _pageOrderingRules = [];
    private readonly List<string> _updatePageNumbers = [];

    public void Process(string inputFile)
    {
        string[] input = File.ReadAllLines(inputFile);


        int state = 1;
        foreach (var line in input)
        {
            // stage 1: reading input lines
            if (state == 1)
            {
                if (line == string.Empty)
                {
                    state = 2;
                    continue;
                }

                var page = int.Parse(line.Split("|")[0]);

                if (_pageOrderingRules.TryGetValue(page, out List<int>? pageOrder))
                {
                    Debug.Assert(pageOrder != null, nameof(pageOrder) + " != null");
                    pageOrder.Add(int.Parse(line.Split("|")[1]));
                }
                else
                {
                    _pageOrderingRules.Add(page, [int.Parse(line.Split("|")[1])]);
                }
            }
            else
            {
                // stage 2: reading the page numbers
                _updatePageNumbers.Add(line);
            }
        }
    }

    public string PartOne
    {
        get
        {
            int sumOfMiddlePages = 0;

            foreach (string updatePage in _updatePageNumbers)
            {
                if (UpdatePagesAreValid(updatePage))
                {
                    int middlePageIndex = (updatePage.Split(",").Length / 2);
                    sumOfMiddlePages += Convert.ToInt32(updatePage.Split(',')[middlePageIndex]);
                }
            }

            return sumOfMiddlePages.ToString();
        }
    }

    private bool UpdatePagesAreValid(string updatePage)
    {
        var splitPages = updatePage.Split(",");
        for (int i = 1; i < splitPages.Length; i++)
        {
            // we want to grab the page at index i, and then check all the pages before it, to make sure
            // they're not in the dictionary needing to be after.
            int pageWereLookingAt = int.Parse(splitPages[i]);

            var pagesToExamine = splitPages[..(i)];

            foreach (string page in pagesToExamine)
            {
                if (_pageOrderingRules.TryGetValue(pageWereLookingAt, out List<int>? rulesForThisPage))
                {
                    if (rulesForThisPage != null && rulesForThisPage.Contains(Convert.ToInt32(page)))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public string PartTwo
    {
        get
        {
            
            int sumOfMiddlePages = 0;

            foreach (List<int> pages in from updatePage in _updatePageNumbers where !UpdatePagesAreValid(updatePage) select updatePage.Split(",").Select(int.Parse).ToList())
            {
                pages.Sort((x, y) =>
                {
                    if (_pageOrderingRules.TryGetValue(x, out List<int>? rulesForThisPage))
                    {
                        if (rulesForThisPage != null && rulesForThisPage.Contains(y))
                        {
                            return -1;
                        }
                    }

                    if (_pageOrderingRules.TryGetValue(y, out List<int>? rulesForThisPage2))
                    {
                        if (rulesForThisPage2 != null && rulesForThisPage2.Contains(x))
                        {
                            return 1;
                        }
                    }

                    return 0;
                });
                    
                int middlePageIndex = pages.Count / 2;
                sumOfMiddlePages += pages[middlePageIndex];
            }
            
            return sumOfMiddlePages.ToString();
            
        }
    }

    public string PartOneDescription => "Sum of middle pages";
    public string PartTwoDescription => "Sum of middle pages of corrected order pages";
}