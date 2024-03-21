using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

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
        CurrentScore = 0;
    }
}


public class PlayerScore
{
    public int Score { get; set; }
}
public class ScoreManager
{
    private readonly string _filePath = "BestScores.json";
    private List<PlayerScore> BestScores { get; set; } = new List<PlayerScore>();

    public ScoreManager()
    {
        LoadScores();
    }

    private void LoadScores()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            BestScores = JsonConvert.DeserializeObject<List<PlayerScore>>(json) ?? new List<PlayerScore>();
        }
    }

    public void SaveScore(int score, string playerName)
    {
        BestScores.Add(new PlayerScore { Score = score });
        BestScores = BestScores.OrderByDescending(s => s.Score).Take(3).ToList();  // Keep only top 3 scores

        string json = JsonConvert.SerializeObject(BestScores, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}