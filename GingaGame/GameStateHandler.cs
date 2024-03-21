using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;   

namespace GingaGame;

public class GameStateHandler (Scene scene, Canvas canvas, PlanetFactory planetFactory, Score score)
{
    private readonly List<Planet> _planets = scene.Planets;
    private readonly float _endLineHeight = 70;
    private readonly int _endLinetreshold = 70;
    private bool _renderEndLine = false;
    private bool _gameOverTriggered = false;
    
    public void CheckGameState()
    {
        canvas.RenderEndLine(_renderEndLine);
        
        // Check if a planet is near the endLine from the bottom
        IsNearEndLine();
        if (!_gameOverTriggered)
        {
            CheckLoseCondition();
        }
    }
    
    private void IsNearEndLine()
    {
        _renderEndLine = false;
        foreach (var planet in _planets)
        {
            if ((planet.Position.Y < _endLineHeight + _endLinetreshold + planet.Radius) && planet.HasCollided)
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
            if (planet.Position.Y < _endLineHeight + planet.Radius && planet.HasCollided)
            {
                _gameOverTriggered = true;
                // Trigger Game Over condition
                MessageBox.Show("You died! Try again!");
                    
                ResetGame();
                break; 
            }
        }
    }

    private void ResetGame()
    {
        score.ResetScore();
        List<Planet> planetsToRemove = new List<Planet>(_planets);
        foreach (var planet in planetsToRemove)
        {
            if (planet.IsPinned) continue;
            scene.RemoveElement(planet);
        }
        _planets.Clear();
        _gameOverTriggered = false;
    }
}