using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GingaGame;

public class GameStateHandler (Scene scene, Canvas canvas, PlanetFactory planetFactory, Score score, MyForm myForm)
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
        foreach (var unused in _planets.Where(planet => planet.Position.Y < EndLineHeight + EndLineThreshold + planet.Radius && planet.HasCollided && !planet.IsPinned))
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
        MessageBox.Show(_gameWonTriggered ? @"Keep trying!" : @"You died! Try again!");
        ResetGame();
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
        for (var i = _planets.Count - 1; i >= 0; i--)
        {
            if (_planets[i].IsPinned) continue;
            scene.RemoveElement(_planets[i]);
        }
        // Reset the unlocked planets in the PlanetFactory
        planetFactory.ResetUnlockedPlanets();
        
        // Call the ResetGame method in MyForm
        //myForm.ResetGame();
    }
}