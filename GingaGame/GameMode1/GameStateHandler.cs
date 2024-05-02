using System.Windows.Forms;
using GingaGame.Shared;
using GingaGame.UI;

namespace GingaGame.GameMode1;

public class GameStateHandler(Container container, Score score, Scoreboard scoreboard, GameMode1Control gameModeControl)
{
    private const int EndLineThreshold = 70;
    private const int Tolerance = 5;
    private bool _gameOverTriggered;
    private bool _gameWonTriggered;
    private bool _renderEndLine;
    private float EndLineHeight => container.TopLeft.Y;

    public void Draw()
    {
        if (!_renderEndLine)
            container.HideEndLine();
        else
            container.ShowEndLine();

        _renderEndLine = false;
    }

    public void CheckGameEndConditions(Planet planet)
    {
        if (!_gameOverTriggered) CheckLoseCondition(planet);

        if (IsNearEndLine(planet) && _renderEndLine == false) _renderEndLine = true;
    }

    private bool IsNearEndLine(Planet planet)
    {
        return planet.Position.Y < EndLineHeight + EndLineThreshold + planet.Radius;
    }

    private void CheckLoseCondition(Planet planet)
    {
        if (!(planet.Position.Y < EndLineHeight + planet.Radius - Tolerance)) return;
        _gameOverTriggered = true;
        MessageBox.Show(@"Game Over! You lost!", @"Game Over", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // Check if the player won the game before losing
        if (_gameWonTriggered)
        {
            var result = ShowInputDialog("Congratulations! You won! Enter your name:", "Game won");
            if (result.DialogResult != DialogResult.OK) return;
            if (result.Controls.Count > 0 && result.Controls[0] is TextBox textBox)
            {
                var playerName = textBox.Text;
                scoreboard.AddScore(playerName, score.CurrentScore);
            }
        }

        ResetGame();
    }

    public void CheckWinCondition(Planet planet)
    {
        if (planet.PlanetType != 10 || _gameWonTriggered) return;
        _gameWonTriggered = true;
        MessageBox.Show(@"Congratulations! You won!", @"Game won", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ResetGame()
    {
        gameModeControl.ResetGame();
        _gameOverTriggered = false;
        _gameWonTriggered = false;
    }

    private static Form ShowInputDialog(string text, string caption)
    {
        var prompt = new Form
        {
            Width = 500,
            Height = 150,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = caption,
            StartPosition = FormStartPosition.CenterScreen
        };
        var textLabel = new Label { Left = 50, Top = 20, Text = text };
        var textBox = new TextBox { Left = 50, Top = 50, Width = 400 };
        var confirmation = new Button
            { Text = @"Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
        confirmation.Click += (_, _) => { prompt.Close(); };
        prompt.Controls.Add(textBox);
        prompt.Controls.Add(confirmation);
        prompt.Controls.Add(textLabel);
        prompt.AcceptButton = confirmation;

        prompt.ShowDialog();
        return prompt;
    }
}