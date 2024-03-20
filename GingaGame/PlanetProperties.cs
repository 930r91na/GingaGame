using System.Collections.Generic;
using System.Drawing;

namespace GingaGame;

public class PlanetProperties(int planetIndex, float size, float mass, Image texture)
{
    public int PlanetIndex { get; set; } = planetIndex;
    public float Size { get; set; } = size;
    public float Mass { get; set; } = mass;
    public Image Texture { get; set; } = texture;
}

public class PlanetPropertiesMap
{
    public Dictionary<int, PlanetProperties> PropertiesPerPlanet { get; private set; } = new()
    {
        { 0, new PlanetProperties(0, 10f, 1, Resource1.Pluto) },
        { 1, new PlanetProperties(1, 15f, 1.5f, Resource1.Moon) },
        { 2, new PlanetProperties(2, 20f, 2, Resource1.Mercury) },
        { 3, new PlanetProperties(3, 25f, 2.5f, Resource1.Mars) },
        { 4, new PlanetProperties(4, 30f, 3, Resource1.Venus) },
        { 5, new PlanetProperties(5, 35f, 3.5f, Resource1.Earth) },
        { 6, new PlanetProperties(6, 40f, 4, Resource1.Neptune) },
        { 7, new PlanetProperties(7, 45f, 4.5f, Resource1.Uranus) },
        { 8, new PlanetProperties(8, 50f, 5, Resource1.Saturn) },
        { 9, new PlanetProperties(9, 55f, 5.5f, Resource1.Jupiter) },
        { 10, new PlanetProperties(10, 60f, 6, Resource1.Sun) }
    };
}

public class PlanetPoints
{
    public Dictionary<int, int> PointsPerPlanet { get; private set; } = new()
    {
        { 0, 10 },
        { 1, 12 },
        { 2, 14 },
        { 3, 16 },
        { 4, 18 },
        { 5, 20 },
        { 6, 22 },
        { 7, 24 },
        { 8, 26 },
        { 9, 28 },
        { 10, 30 }
    };
}