using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GingaGame;

public class GameStateHandler(Scene scene, Canvas canvas, Score score, Scoreboard scoreboard, MyForm myForm)
{
    private const float EndLineHeight = 70;
    private const int EndLineThreshold = 70;
    private const int Tolerance = 5;
    private readonly List<Planet> _planets = scene.Planets;
    private bool _gameOverTriggered;
    private bool _gameWonTriggered;
    private bool _renderEndLine;

    public void CheckGameState()
    {
        canvas.RenderEndLine(_renderEndLine);

        // Check if a planet is near the endLine from the bottom
        IsNearEndLine();
        if (!_gameOverTriggered) CheckLoseCondition();

        if (!_gameWonTriggered) CheckWinCondition();
    }

    private void IsNearEndLine()
    {
        _renderEndLine = false;
        foreach (var unused in _planets.Where(planet =>
                     planet.Position.Y < EndLineHeight + EndLineThreshold + planet.Radius && planet.HasCollided &&
                     !planet.IsPinned))
        {
            _renderEndLine = true;
            break;
        }
    }

    private void CheckLoseCondition()
    {
        if (!_planets.Any(planet =>
                planet.Position.Y < EndLineHeight + planet.Radius - Tolerance && planet.HasCollided)) return;
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

    private void CheckWinCondition()
    {
        if (_planets.All(planet => planet.PlanetType != 10)) return;
        _gameWonTriggered = true;
        MessageBox.Show(@"Congratulations! You won!", @"Game won", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ResetGame()
    {
        myForm.ResetGame();
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