using System.Text.RegularExpressions;

namespace TwentyFifteen;

internal class Day14 : IDay
{
    public uint Index => 14;

    public string PartOne => WinningDistance.ToString();
    public string PartTwo => WinningScore.ToString();
    public string PartOneDescription => "Distance has the winning reindeer traveled";
    public string PartTwoDescription => "Points the winning reindeer has";

    private uint _secondsRemainingToProcess;

    public uint WinningDistance { get; private set; }
    public uint WinningScore { get; private set; }

    private readonly Dictionary<string, (uint FlySpeed, uint FlyTime, uint RestTime)> _reindeer = new();

    private readonly Dictionary<string, (ReindeerStates State, uint TimeRemainingInCurrentState, uint DistanceTravelled)> _states = new();
    private readonly Dictionary<string, uint> _scores = new();

    private enum ReindeerStates
    {
        Flying,
        Resting
    }

    public void Process(string inputFile)
    {
        List<string> lines = File.ReadLines(inputFile).ToList();
        _secondsRemainingToProcess = uint.Parse(lines[0]);

        Regex decoder =
            new(
                @"(?<reindeer>[A-Za-z]+) can fly (?<speed>\d+) km/s for (?<flytime>\d+) seconds, but then must rest for (?<resttime>\d+) seconds.");

        foreach (string s in lines.Skip(1).ToList())
        {
            Match match = decoder.Match(s);
            _reindeer.Add(match.Groups["reindeer"].ToString(), (
                uint.Parse(match.Groups["speed"].ToString()),
                uint.Parse(match.Groups["flytime"].ToString()),
                uint.Parse(match.Groups["resttime"].ToString())));

            _states.Add(match.Groups["reindeer"].ToString(), (ReindeerStates.Flying, uint.Parse(match.Groups["flytime"].ToString()), 0));
            _scores.Add(match.Groups["reindeer"].ToString(),0);
        }

        while (_secondsRemainingToProcess > 0)
        {

            foreach (KeyValuePair<string, (ReindeerStates State, uint TimeRemainingInCurrentState, uint DistanceTravelled)> r in _states)
            {
                ReindeerStates state = r.Value.State;
                int timeRemaining = Convert.ToInt32(r.Value.TimeRemainingInCurrentState);
                uint distanceTraveled = r.Value.DistanceTravelled;
                timeRemaining--;
                if (timeRemaining < 0)
                {
                    state = state == ReindeerStates.Flying ? ReindeerStates.Resting : ReindeerStates.Flying;
                    timeRemaining = state == ReindeerStates.Flying
                        ? Convert.ToInt32(_reindeer[r.Key].FlyTime)-1
                        : Convert.ToInt32(_reindeer[r.Key].RestTime)-1;
                }
                if (state == ReindeerStates.Flying) { distanceTraveled += _reindeer[r.Key].FlySpeed; }


                _states[r.Key] = (state, Convert.ToUInt32(timeRemaining), distanceTraveled);
            }

            uint currentWinningDistance = _states.Max(f => f.Value.DistanceTravelled);
            var currentLeadingReindeer = _states.First(f => f.Value.DistanceTravelled == currentWinningDistance).Key;
            _scores[currentLeadingReindeer] += 1;

            _secondsRemainingToProcess--;
        }

        WinningDistance = _states.Max(f => f.Value.DistanceTravelled);
        WinningScore = _scores.Max(f => f.Value);

    }
}
