using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GingaGame;

public class GameStateHandler(Scene scene, Canvas canvas, MyForm myForm)
{
    private const float EndLineHeight = 70;
    private const int EndLineThreshold = 70;
    private const int Tolerance = 3;
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
    }
}