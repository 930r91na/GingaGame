using System.Drawing;

namespace GingaGame;

public class Planet(
    int planetIndex,
    float x,
    float y,
    Canvas canvas,
    PlanetPropertiesMap propertiesMap,
    PlanetPoints pointsMap)
    : VPoint(x, y, canvas, propertiesMap.PropertiesPerPlanet[planetIndex].Texture,
        propertiesMap.PropertiesPerPlanet[planetIndex].Mass, propertiesMap.PropertiesPerPlanet[planetIndex].Size,
        propertiesMap.PropertiesPerPlanet[planetIndex].PlanetIndex)
{
    public Image Texture { get; private set; } = propertiesMap.PropertiesPerPlanet[planetIndex].Texture;
    public float Mass { get; private set; } = propertiesMap.PropertiesPerPlanet[planetIndex].Mass;
    public float Radius { get; private set; } = propertiesMap.PropertiesPerPlanet[planetIndex].Size;
    public int Points { get; private set; } = pointsMap.PointsPerPlanet[planetIndex];
}