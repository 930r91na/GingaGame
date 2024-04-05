using System.Collections.Generic;
using System.Globalization;
using System.IO;

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
        var newScore = new ScoreEntry(playerName, score);
        _scores.Add(newScore);
        _scores.Sort((x, y) => y.Score.CompareTo(x.Score)); // Sort descending

        // Append the new score to the file
        using var writer = File.AppendText(ScoreFile);
        writer.WriteLine($"{newScore.PlayerName}:{newScore.Score}");
    }

    public IEnumerable<ScoreEntry> GetTopScores()
    {
        _scores.Clear();
        LoadScores();
        return _scores;
    }

    private void LoadScores()
    {
        OrderScores();
        if (!File.Exists(ScoreFile)) return;
        var lines = File.ReadAllLines(ScoreFile);
        
        // Parse the lines and add the first 6 scores
        var count = 0;
        foreach (var line in lines)
        {
            if (count >= 6) break; // Only load the top 6 scores
            
            var parts = line.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out var score)) continue;
            var playerName = parts[0];
            if (playerName.Length > 8) // Check if the name is too long
                playerName = playerName.Substring(0, 8) + ".."; // Trim and append '...'
            // Convert to title case
            playerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(playerName.ToLowerInvariant());
            _scores.Add(new ScoreEntry(playerName, score));
            
            count++; // Increment the count
        }
        _scores.Sort((x, y) => y.Score.CompareTo(x.Score)); // Sort descending
    }

    private static void OrderScores()
    {
        if (!File.Exists(ScoreFile)) return;
        var lines = File.ReadAllLines(ScoreFile);

        // Parse the lines into a list of ScoreEntry objects
        var scoreEntries = new List<ScoreEntry>();
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out var score)) continue;
            var playerName = parts[0];
            scoreEntries.Add(new ScoreEntry(playerName, score));
        }

        // Sort the list in descending order by score
        scoreEntries.Sort((x, y) => y.Score.CompareTo(x.Score));

        // Write the sorted scores back to the file
        using var writer = new StreamWriter(ScoreFile);
        foreach (var entry in scoreEntries)
        {
            writer.WriteLine($"{entry.PlayerName}:{entry.Score}");
        }
    }
}