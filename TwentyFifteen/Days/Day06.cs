namespace TwentyFifteen;

internal class Day06 : IDay
{
    public uint Index => 6;

    public string PartOne => LightsOn.ToString();

    public string PartTwo => LightBrightness.ToString();

    public string PartOneDescription => "Lights on";

    public string PartTwoDescription => "Lights brightness";

    public ulong LightsOn {get;set;}
    public ulong LightBrightness {get;set;}

    public void Process(string inputFile)
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

            List<string> input = File.ReadLines(inputFile).ToList<string>();

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

            LightsOn = totallightsOn;
            LightBrightness = totalBrightness;
    }
}