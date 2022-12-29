using System.Runtime.CompilerServices;

namespace TwentyFifteen;

internal class Day18 : IDay
{
    public uint Index => 18;
    public string PartOne => Phase1Lights.ToString();
    public string PartTwo => Phase2Lights.ToString();
    public string PartOneDescription => $"Lights are on after {_iterations} steps";
    public string PartTwoDescription => $"Lights are on after {_iterations} steps with stuck corners";
    public uint Phase1Lights { get; private set; }
    public uint Phase2Lights { get; private set; }

    private uint _iterations;
    private uint _width;
    private uint _height;

    private bool[,]? _buffer1;
    private bool[,]? _buffer2;

    private uint _bufferIndex;
    private bool[,] FrontBuffer => (_bufferIndex == 0 ? _buffer1 : _buffer2) ?? throw new InvalidOperationException();
    private bool[,] BackBuffer => (_bufferIndex == 0 ? _buffer2 : _buffer1) ?? throw new InvalidOperationException();

    private void SwapBuffers()
    {
        _bufferIndex = ++_bufferIndex % 2;
    }


    public uint LightsOn
    {
        get
        {
            uint result = default;
            for (uint y = 0; y < _height; y++)
            {
                for (uint x = 0; x < _width; x++)
                {
                    result += FrontBuffer[y, x] ? 1U : 0U;
                }
            }

            return result;
        }
    }

    private uint GetValueOfCell(uint y, uint x) => FrontBuffer[y, x] ? 1U : 0U;

    private uint GetValuesAroundCell(uint y, uint x) => (y > 0 && x > 0 ? GetValueOfCell(y - 1, x - 1) : 0U) +
                                                        (y > 0 ? GetValueOfCell(y - 1, x) : 0U) +
                                                        (y > 0 && x < _width - 1 ? GetValueOfCell(y - 1, x + 1) : 0U) +
                                                        (x > 0 ? GetValueOfCell(y, x - 1) : 0U) +
                                                        (x < _width - 1 ? GetValueOfCell(y, x + 1) : 0U) +
                                                        (y < _height - 1 && x > 0 ? GetValueOfCell(y + 1, x - 1) : 0U) +
                                                        (y < _height - 1 ? GetValueOfCell(y + 1, x) : 0U) +
                                                        (y < _height - 1 && x < _width - 1 ? GetValueOfCell(y + 1, x + 1) : 0U);

    private void PrintFrontBuffer()
    {
        for (uint y = 0; y < _height; y++)
        {
            for (uint x = 0; x < _width; x++)
            {
                Console.Write(FrontBuffer[y, x] ? "#" : ".");
            }

            Console.WriteLine();
        }
    }

    private void Iterate()
    {
        for (uint y = 0; y < _height; y++)
        {
            for (uint x = 0; x < _width; x++)
            {
                if (FrontBuffer[y, x])
                {
                    // A light which is on stays on when 2 or 3 neighbors are on, and turns off otherwise.
                    BackBuffer[y,x] = GetValuesAroundCell( y, x) == 2 || GetValuesAroundCell( y, x) == 3;
                }
                else
                {
                    // A light which is off turns on if exactly 3 neighbors are on, and stays off otherwise.
                    BackBuffer[y, x] = GetValuesAroundCell(y, x) == 3;
                }
            }
        }
    }

    public void Process(string inputFile)
    {
        _iterations = uint.Parse(File.ReadLines(inputFile).First());
        _width = Convert.ToUInt32(File.ReadLines(inputFile).Skip(1).First().Length);
        _height = Convert.ToUInt32(File.ReadLines(inputFile).Skip(1).Count());

        // initialize the buffers;
        _buffer1 = new bool[_height, _width];
        _buffer2 = new bool[_height, _width];
        {
            uint y = 0;
            uint x = 0;
            foreach (string s in File.ReadLines(inputFile).Skip(1).ToList())
            {
                foreach (char c in s)
                {
                    FrontBuffer[y, x] = c == '#';
                    x++;
                }
                y++;
                x = 0;
            }
        }

        // iterate
        for (int i = 0; i < _iterations; i++)
        {
            Iterate();
            SwapBuffers();
        }

        Phase1Lights = LightsOn;

        // reset for the second run
        _buffer1 = new bool[_height, _width];
        _buffer2 = new bool[_height, _width];
        {
            uint y = 0;
            uint x = 0;
            foreach (string s in File.ReadLines(inputFile).Skip(1).ToList())
            {
                foreach (char c in s)
                {
                    FrontBuffer[y, x] = c == '#';
                    x++;
                }
                y++;
                x = 0;
            }
        }
        FrontBuffer[0, 0] = true;
        FrontBuffer[_height-1, 0] = true;
        FrontBuffer[0, _width-1] = true;
        FrontBuffer[_height - 1, _width - 1] = true;

        // iterate
        for (int i = 0; i < _iterations; i++)
        {
            Iterate();
            BackBuffer[0, 0] = true;
            BackBuffer[_height - 1, 0] = true;
            BackBuffer[0, _width - 1] = true;
            BackBuffer[_height - 1, _width - 1] = true;
            SwapBuffers();
        }

        Phase2Lights = LightsOn;
    }
}