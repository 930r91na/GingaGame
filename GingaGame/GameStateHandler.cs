using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GingaGame;

public class GameStateHandler (Scene scene, Canvas canvas, Score score)
{
    private readonly List<Planet> _planets = scene.Planets;
    private const float EndLineHeight = 70;
    private const int EndLineThreshold = 70;
    private const int Tolerance = 3;
    private bool _renderEndLine;
    private bool _gameOverTriggered;
    private bool _gameWonTriggered;
    
    public void CheckGameState()
    {
        canvas.RenderEndLine(_renderEndLine);
        
        // Check if a planet is near the endLine from the bottom
        IsNearEndLine();
        if (!_gameOverTriggered)
        {
            CheckLoseCondition();
        }
        
        if (!_gameWonTriggered)
        {
            CheckWinCondition();
        }
    }
    
    private void IsNearEndLine()
    {
        _renderEndLine = false;
        foreach (var planet in _planets)
        {
            if ((planet.Position.Y < EndLineHeight + EndLineThreshold + planet.Radius) && planet.HasCollided)
            {
                if (planet.IsPinned) continue;
                _renderEndLine = true;
                break;
            }
        }
    }
    
    private void CheckLoseCondition()
    {
        foreach (var planet in _planets)
        {
            if (planet.Position.Y < EndLineHeight + planet.Radius - Tolerance && planet.HasCollided)
            {
                _gameOverTriggered = true;
                // Trigger Game Over condition

                MessageBox.Show(_gameWonTriggered ? @"Keep trying!" : @"You died! Try again!");
                ResetGame();
                break; 
            }
        }
    }

    private void CheckWinCondition()
    {
        if (_planets.All(planet => planet.PlanetType != 10)) return;
        _gameWonTriggered = true;
        MessageBox.Show(@"Congratulations! You won!");
    }

    private void ResetGame()
    {
        score.ResetScore();
        List<Planet> planetsToRemove = [.._planets];
        foreach (var planet in planetsToRemove)
        {
            if (planet.IsPinned) continue;
            scene.RemoveElement(planet);
        }
        _planets.Clear();
        _gameOverTriggered = false;
    }
}