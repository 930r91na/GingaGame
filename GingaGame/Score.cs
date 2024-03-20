namespace GingaGame;

public class Score
{
    public int CurrentScore { get; private set; }

    public void IncreaseScore(int amount)
    {
        CurrentScore += amount;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}