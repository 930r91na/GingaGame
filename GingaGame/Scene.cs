using System.Collections.Generic;
using System.Drawing;

namespace GingaGame;

public class Scene
{
    public List<Planet> Planets { get; } = [];

    public void AddElement(Planet planet)
    {
        Planets.Add(planet);
    }

    public void Render(Graphics g)
    {
        foreach (var planet in Planets) planet.Render(g);
    }
}