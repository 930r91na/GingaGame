using System;
using System.Collections.Generic;

namespace GingaGame;

public class PlanetFactory(PlanetPropertiesMap propertiesMap, PlanetPoints pointsMap)
{
    private readonly List<int> _unlockedPlanets = [0]; // Start with Pluto
    private readonly Random _randomGenerator = new();

    public Planet GenerateNextPlanet(Canvas canvas)
    {
        int nextIndex;
        do
        {
            nextIndex = _randomGenerator.Next(0, 5);
        } while (!_unlockedPlanets.Contains(nextIndex));

        return new Planet(nextIndex, 0, 0, canvas, propertiesMap, pointsMap)
        {
            IsPinned = false
        };
    }

    // Method to unlock a new planet (when merging happens)
    public void UnlockPlanet(int planetIndex)
    {
        if (!_unlockedPlanets.Contains(planetIndex))
        {
            _unlockedPlanets.Add(planetIndex);
        }
    }
}
