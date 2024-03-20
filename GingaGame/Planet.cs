using System.Drawing;

namespace GingaGame
{
    public class Planet(
        int planetType,
        float x,
        float y,
        Canvas canvas,
        PlanetPropertiesMap propertiesMap,
        PlanetPoints pointsMap)
        : VPoint(x, y, canvas, propertiesMap.PropertiesPerPlanet[planetType].Texture,
            propertiesMap.PropertiesPerPlanet[planetType].Mass, propertiesMap.PropertiesPerPlanet[planetType].Size)
    {
        public int PlanetType { get; private set; } = planetType;
        public Image Texture { get; private set; } = propertiesMap.PropertiesPerPlanet[planetType].Texture;
        public float Mass { get; private set; } = propertiesMap.PropertiesPerPlanet[planetType].Mass;
        public float Radius { get; private set; } = propertiesMap.PropertiesPerPlanet[planetType].Size;
        public int Points { get; private set; } = pointsMap.PointsPerPlanet[planetType];
    }
}