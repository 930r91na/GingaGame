using System;
using System.Collections.Generic;

namespace GingaGame;

public class PlanetFactory
{
    private readonly Random _randomGenerator = new();
    private readonly List<int> _unlockedPlanets = [0]; // Start with Pluto

    public Planet GenerateNextPlanet(Canvas canvas)
    {
        int nextIndex;
        do
        {
            nextIndex = _randomGenerator.Next(0, 5);
        } while (!_unlockedPlanets.Contains(nextIndex));

        var middleX = canvas.Width / 2;

        return new Planet(nextIndex, middleX, 0, canvas)
        {
            IsPinned = false
        };
    }

    // Method to unlock a new planet (when merging happens)
    public void UnlockPlanet(int planetIndex)
    {
        if (!_unlockedPlanets.Contains(planetIndex)) _unlockedPlanets.Add(planetIndex);
    }
}