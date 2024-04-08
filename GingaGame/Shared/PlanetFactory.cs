using System;
using System.Collections.Generic;

namespace GingaGame.Shared;

public class PlanetFactory
{
    private readonly Random _randomGenerator = new();
    private readonly List<int> _unlockedPlanets = [0]; // Start with Pluto's index

    public Planet GenerateNextPlanet(Canvas canvas, CollisionHandler collisionHandler)
    {
        int nextIndex;
        do
        {
            nextIndex = _randomGenerator.Next(0, 5);
        } while (!_unlockedPlanets.Contains(nextIndex));

        var middleX = canvas.Width / 2;

        return new Planet(nextIndex, middleX, 0, collisionHandler)
        {
            IsPinned = true
        };
    }

    // Method to unlock a new planet (when merging happens)
    public void UnlockPlanet(int planetIndex)
    {
        if (!_unlockedPlanets.Contains(planetIndex)) _unlockedPlanets.Add(planetIndex);
    }

    public void ResetUnlockedPlanets()
    {
        _unlockedPlanets.Clear();

        // Start with Pluto
        _unlockedPlanets.Add(0);
    }
}