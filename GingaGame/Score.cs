using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GingaGame;

public class Score
{
    public int CurrentScore { get; private set; }
    public bool HasChanged { get; set; }

    public void IncreaseScore(int amount)
    {
        CurrentScore += amount;
    }

    public void ResetScore()
    {
        HasChanged = true;
        CurrentScore = 0;
    }
}

public class ScoreEntry(string playerName, int score)
{
    public string PlayerName { get; } = playerName;
    public int Score { get; } = score;
}

public class Scoreboard
{
    private const string ScoreFile = "scores.txt";
    private readonly List<ScoreEntry> _scores = [];

    public Scoreboard()
    {
        LoadScores();
    }

    public void AddScore(string playerName, int score)
    {
        _scores.Add(new ScoreEntry(playerName, score));
        _scores.Sort((x, y) => y.Score.CompareTo(x.Score)); // Sort descending

        // Clear the list if it has more than 3 entries
        if (_scores.Count > 3) _scores.RemoveRange(3, _scores.Count - 3); // Keep top 3
        SaveScores();
    }

    public IEnumerable<ScoreEntry> GetTopScores()
    {
        return _scores;
    }

    private void LoadScores()
    {
        if (!File.Exists(ScoreFile)) return;
        var lines = File.ReadAllLines(ScoreFile);
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[1], out var score))
                _scores.Add(new ScoreEntry(parts[0], score));
        }
    }

    private void SaveScores()
    {
        var lines = _scores.Select(entry => $"{entry.PlayerName}:{entry.Score}").ToList();
        File.WriteAllLines(ScoreFile, lines);
    }
}