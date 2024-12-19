namespace TwentyTwentyFour;

public class Day06 : IDay
{
    public uint Index => 6;
    private char[,]? _matrix;
    private char[,]? _startingMatrix;
    private int? _startingRow;
    private int? _startingColumn;
    private uint _direction = 1; // 0=left, 1=up, 2=right, 3=down.
    private int? _currentRow;
    private int? _currentColumn;
    List<(int row, int col)> visitedLocations = [];
    
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
                if (c == '^')
                {
                    _currentColumn = x;
                    _currentRow = y;
                }
                _matrix[y, x] = c;
                x++;
            }
            y++;
        }

        _startingMatrix = (char[,]?)_matrix.Clone();
        _startingRow = _currentRow;
        _startingColumn = _currentColumn;

        while (true)
        {
            // set the current location to visited
            _matrix[(int)_currentRow!, (int)_currentColumn!] = '%';

            int nextRow = (int)_currentRow!;
            int nextColumn = (int)_currentColumn!;
            
            // get the desired position for the next move (this occurs before any turn)
            if (_direction == 0) { --nextColumn; }
            else if (_direction == 1) { --nextRow; }
            else if (_direction == 2) { ++nextColumn; }
            else { ++nextRow; }
            
            // determine if this will leave the play field.
            if (nextRow == -1
                || nextColumn == -1
                || nextColumn == _matrix.GetLength(1)
                || nextRow == _matrix.GetLength(0))
            {
                break; 
            }
            
            // determine if the next move will collide with something
            if (_matrix[nextRow, nextColumn] == '#')
            {
                _direction = (_direction + 1) % 4;
                nextRow = (int)_currentRow!;
                nextColumn = (int)_currentColumn!;
            
                // get the new next position
                if (_direction == 0) { --nextColumn; }
                else if (_direction == 1) { --nextRow; }
                else if (_direction == 2) { ++nextColumn; }
                else { ++nextRow; }
            
                // determine if this will leave the play field.
                if (nextRow == -1 
                    || nextColumn == -1 
                    || nextColumn == _matrix.GetLength(1) 
                    || nextRow == _matrix.GetLength(0)) 
                {   
                    break; 
                }
            }
            
            _currentColumn = nextColumn;
            _currentRow = nextRow;
        }
        
        
        // print out the results for debugging.
        foreach (int printY in Enumerable.Range(0, _matrix.GetLength(0)))
        {
            foreach (int printX in Enumerable.Range(0, _matrix.GetLength(1)))
            {
                Console.Write(_matrix[printY, printX]);
            }
            Console.WriteLine();
        }

    }

    public string PartOne
    {
        get
        {
            int positionsVisited = 0;
            
            foreach (int printY in Enumerable.Range(0, _matrix!.GetLength(0)))
            {
                foreach (int printX in Enumerable.Range(0, _matrix!.GetLength(1)))
                {
                    if (_matrix[printY, printX] == '%')
                    {
                        positionsVisited++;
                        if (printY != _startingRow || printX != _startingColumn)
                        {
                            visitedLocations.Add((printY, printX));
                        }
                    }
                }
                
            } 
            
            return positionsVisited.ToString();
        }
    }
    public string PartTwo {
        get
        {

            int possibleBlocks = 0;
            
            foreach (var r in visitedLocations)
            {
                if (WouldLoopIfBarrierAdded(r.row, r.col))
                {
                    possibleBlocks++;
                }
            }

            return possibleBlocks.ToString();

        }
    }

    private bool WouldLoopIfBarrierAdded(int rRow, int rCol)
    {
        
        List<(int row, int col, int direction)> alreadyTurnedAt = [];
        
        // clone the starting grid.
        char[,] workingGrid = _startingMatrix!.Clone() as char[,];
        workingGrid[rRow,rCol] = '#';
        int currentRow = (int)_startingRow!;
        int currentColumn = (int)_startingColumn!;
        int currentDirection = 1;
        
        
        
        while (true)
        {
            int nextRow = currentRow!;
            int nextColumn = currentColumn!;
            
            // get the desired position for the next move (this occurs before any turn)
            if (currentDirection == 0) { --nextColumn; }
            else if (currentDirection == 1) { --nextRow; }
            else if (currentDirection == 2) { ++nextColumn; }
            else { ++nextRow; }
            
            // determine if this will leave the play field.
            if (nextRow == -1
                || nextColumn == -1
                || nextColumn == workingGrid.GetLength(1)
                || nextRow == workingGrid.GetLength(0))
            {
                return false;
            }
            
            // determine if the next move will collide with something
            if (workingGrid[nextRow,nextColumn] == '#')
            {
                
                // see if we've already turned here.
                if (alreadyTurnedAt.Contains((currentRow, currentColumn, currentDirection)))
                {
                    return true;
                }
                
                alreadyTurnedAt.Add((currentRow, currentColumn, currentDirection));
                
                currentDirection = (currentDirection + 1) % 4;

                continue;
            }
            
            currentColumn = nextColumn;
            currentRow = nextRow;
        }

        return false;
        
    }

    public string PartOneDescription => "Distinct positions visited";
    public string PartTwoDescription => "Different obstacle positions";
}
