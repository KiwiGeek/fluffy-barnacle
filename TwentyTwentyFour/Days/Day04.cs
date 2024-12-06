using System.Diagnostics;

namespace TwentyTwentyFour;

public class Day04 : IDay
{
    public uint Index => 4;
    private char[,]? _matrix;
    public void Process(string inputFile)
    {
        string[] input = File.ReadAllLines(inputFile);
        
        // get the dimensions of the word search.
        int horizontal = input.First().Length;
        int vertical = input.Count();

        _matrix = new char[vertical, horizontal];
        int y = 0;
        foreach (string line in input)
        {
            int x = 0;
            foreach (char c in line)
            {
                _matrix[y, x] = c;
                x++;
            }

            y++;
        }
        
    }

    public string PartOne
    {
        get
        {
            int countOfMatches = 0;
            
            // search for Right matches.
            Debug.Assert(_matrix != null, nameof(_matrix) + " != null");
            for (int row = 0; row < _matrix.GetLength(0); row++)
            {
                for (int col = 0; col < _matrix.GetLength(1)-3; col++)
                {
                    if ((_matrix[row, col] == 'X') &
                        (_matrix[row, col + 1] == 'M') &
                        (_matrix[row, col + 2] == 'A') &
                        (_matrix[row, col + 3] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            // search for down-right matches
            for (int row = 0; row < _matrix.GetLength(0)-3; row++)
            {
                for (int col = 0; col < _matrix.GetLength(1)-3; col++)
                {
                    if ((_matrix[row, col] == 'X') &
                        (_matrix[row+1, col + 1] == 'M') &
                        (_matrix[row+2, col + 2] == 'A') &
                        (_matrix[row+3, col + 3] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            // search for down matches
            for (int row = 0; row < _matrix.GetLength(0)-3; row++)
            {
                for (int col = 0; col < _matrix.GetLength(1); col++)
                {
                    if ((_matrix[row, col] == 'X') &
                        (_matrix[row+1, col] == 'M') &
                        (_matrix[row+2, col] == 'A') &
                        (_matrix[row+3, col] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            // search for down-left matches
            for (int row = 0; row < _matrix.GetLength(0)-3; row++)
            {
                for (int col = 3; col < _matrix.GetLength(1); col++)
                {
                    if ((_matrix[row, col] == 'X') &
                        (_matrix[row+1, col-1] == 'M') &
                        (_matrix[row+2, col-2] == 'A') &
                        (_matrix[row+3, col-3] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            // search for left matches
            for (int row = 0; row < _matrix.GetLength(0); row++)
            {
                for (int col = 3; col < _matrix.GetLength(1); col++)
                {
                    if ((_matrix[row, col] == 'X') &
                        (_matrix[row, col-1] == 'M') &
                        (_matrix[row, col-2] == 'A') &
                        (_matrix[row, col-3] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            // search for up-left matches
            for (int row = 3; row < _matrix.GetLength(0); row++)
            {
                for (int col = 3; col < _matrix.GetLength(1); col++)
                {
                    if ((_matrix[row, col] == 'X') &
                        (_matrix[row-1, col-1] == 'M') &
                        (_matrix[row-2, col-2] == 'A') &
                        (_matrix[row-3, col-3] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            // search for up matches
            for (int row = 3; row < _matrix.GetLength(0); row++)
            {
                for (int col = 0; col < _matrix.GetLength(1); col++)
                {
                    if ((_matrix[row, col] == 'X') &
                        (_matrix[row-1, col] == 'M') &
                        (_matrix[row-2, col] == 'A') &
                        (_matrix[row-3, col] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            // search for up-right matches
            for (int row = 3; row < _matrix.GetLength(0); row++)
            {
                for (int col = 0; col < _matrix.GetLength(1)-3; col++)
                {
                    if ((_matrix[row, col] == 'X') &
                        (_matrix[row-1, col+1] == 'M') &
                        (_matrix[row-2, col+2] == 'A') &
                        (_matrix[row-3, col+3] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }

            return countOfMatches.ToString();
        }
    }

    public string PartTwo
    {
        get
        {
            int countOfMatches = 0;
            Debug.Assert(_matrix != null, nameof(_matrix) + " != null");
            for (int row = 0; row < _matrix.GetLength(0)-2; row++)
            {
                for (int col = 0; col < _matrix.GetLength(1)-2; col++)
                {
                    if ((_matrix[row, col] == 'M') &
                        (_matrix[row+1, col + 1] == 'A') &
                        (_matrix[row+2, col + 2] == 'S') &
                        (_matrix[row, col + 2] == 'M') &
                        (_matrix[row+2, col] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            for (int row = 0; row < _matrix.GetLength(0)-2; row++)
            {
                for (int col = 0; col < _matrix.GetLength(1)-2; col++)
                {
                    if ((_matrix[row, col] == 'S') &
                        (_matrix[row+1, col + 1] == 'A') &
                        (_matrix[row+2, col + 2] == 'M') &
                        (_matrix[row, col + 2] == 'M') &
                        (_matrix[row+2, col] == 'S'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            for (int row = 0; row < _matrix.GetLength(0)-2; row++)
            {
                for (int col = 0; col < _matrix.GetLength(1)-2; col++)
                {
                    if ((_matrix[row, col] == 'M') &
                        (_matrix[row+1, col + 1] == 'A') &
                        (_matrix[row+2, col + 2] == 'S') &
                        (_matrix[row, col + 2] == 'S') &
                        (_matrix[row+2, col] == 'M'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            for (int row = 0; row < _matrix.GetLength(0)-2; row++)
            {
                for (int col = 0; col < _matrix.GetLength(1)-2; col++)
                {
                    if ((_matrix[row, col] == 'S') &
                        (_matrix[row+1, col + 1] == 'A') &
                        (_matrix[row+2, col + 2] == 'M') &
                        (_matrix[row, col + 2] == 'S') &
                        (_matrix[row+2, col] == 'M'))
                    {
                        countOfMatches++;
                    }
                }
            }
            
            return countOfMatches.ToString();
        }
    }
    public string PartOneDescription => "Number of XMAS's";
    public string PartTwoDescription => "Number of X-MAS's";
    
}