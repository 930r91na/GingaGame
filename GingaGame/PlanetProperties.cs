using System.Collections.Generic;
using System.Drawing;

namespace GingaGame;

public class PlanetProperties(string planetType, float size, float mass, Image texture)
{
    public string PlanetType { get; set; } = planetType;
    public float Size { get; set; } = size;
    public float Mass { get; set; } = mass;
    public Image Texture { get; set; } = texture;
}

public class PlanetPropertiesMap
{
    public Dictionary<string, PlanetProperties> PropertiesPerPlanet { get; private set; } = new()
    {
        {"Moon", new PlanetProperties("Moon", 10, 1f, Resource1.Luna)},
        {"Mercury", new PlanetProperties("Mercury", 15, 1.5f, Resource1.Mercurio)},
        {"Earth", new PlanetProperties("Earth", 20, 2f, Resource1.Tierra)},
        {"Neptune", new PlanetProperties("Neptune", 25, 2.5f, Resource1.Neptuno)}
    };
}

public class PlanetPoints
{
    public Dictionary<string, int> PointsPerPlanet { get; private set; } = new()
    {
        {"Moon", 10},
        {"Mercury", 12},
        {"Earth", 14},
        {"Neptune", 12}
    };
}