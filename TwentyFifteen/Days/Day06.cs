namespace TwentyFifteen;

internal class Day06 : IDay
{
    public uint Index => 6;

    public string PartOne => LightsOn.ToString();

    public string PartTwo => LightBrightness.ToString();

    public string PartOneDescription => "Lights on";

    public string PartTwoDescription => "Lights brightness";

    private ulong LightsOn {get;set;}
    private ulong LightBrightness {get;set;}

    public void Process(string inputFile)
    {
        
            bool[,] lights = new bool[1000, 1000];
            uint[,] lightBrightness = new uint[1000, 1000];

            List<string> input = File.ReadLines(inputFile).ToList();

            foreach (string s in input)
            {
                System.Text.RegularExpressions.Regex instructionDecoder = new("^(?<instr>(?:turn on)|(?:turn off)|(?:toggle)) (?<x1>\\d*),(?<y1>\\d*) through (?<x2>\\d*),(?<y2>\\d*)$");
                System.Text.RegularExpressions.Match match = instructionDecoder.Match(s);

                string instr = match.Groups["instr"].Value;
                uint x1 = uint.Parse(match.Groups["x1"].Value);
                uint x2 = uint.Parse(match.Groups["x2"].Value);
                uint y1 = uint.Parse(match.Groups["y1"].Value);
                uint y2 = uint.Parse(match.Groups["y2"].Value);

                SetBlock(x1, y1, x2, y2, instr);
            }

            // count lights on
            ulong totalLightsOn = 0;
            ulong totalBrightness = 0;

            for (int y = 0; y <= 999; y++)
            {
                for (int x = 0; x <= 999; x++)
                {
                    if (lights[x, y]) totalLightsOn++;
                    totalBrightness += lightBrightness[x, y];
                }
            }

            LightsOn = totalLightsOn;
            LightBrightness = totalBrightness;
            return;

            void SetBlock(uint x1, uint y1, uint x2, uint y2, string instruction)
            {
                for (uint i = y1; i <= y2; i++)
                {
                    SetRow(x1, x2, i, instruction);
                }

                return;

                void SetRow(uint setRowX1, uint setRowX2, uint setRowY, string setRowInstruction)
                {
                    for (uint i = setRowX1; i <= setRowX2; i++)
                    {
                        SetLight(i, setRowY, setRowInstruction);
                    }

                    return;

                    void SetLight(uint setLightX, uint setLightY, string setLightInstruction)
                    {
                        if (setLightInstruction == "turn on")
                        {
                            lights[setLightX, setLightY] = true;
                            lightBrightness[setLightX, setLightY]++;
                        }
                        if (setLightInstruction == "turn off")
                        {
                            lights[setLightX, setLightY] = false;
                            if (lightBrightness[setLightX, setLightY] > 0) { lightBrightness[setLightX, setLightY]--; }
                        }
                        if (setLightInstruction == "toggle")
                        {
                            lights[setLightX, setLightY] = !lights[setLightX, setLightY];
                            lightBrightness[setLightX, setLightY] += 2;
                        }
                    }
                }
            }
    }
}