using System;
using System.Collections.Generic;

namespace GingaGame.Shared;

public class PlanetFactory(GameMode gameMode)
{
    private readonly Random _randomGenerator = new();

    private readonly List<int> _unlockedPlanets = gameMode switch
    {
        GameMode.Mode1 => [0], // Start with Pluto
        GameMode.Mode2 => [10], // Start with Sun
        _ => throw new ArgumentException("Invalid game mode")
    };

    public Planet GenerateNextPlanet(Canvas canvas, CollisionHandler collisionHandler)
    {
        int nextIndex;
        do
        {
            nextIndex = gameMode switch
            {
                GameMode.Mode1 => _randomGenerator.Next(0, 5),
                GameMode.Mode2 => _randomGenerator.Next(6, 11),
                _ => throw new ArgumentException("Invalid game mode")
            };
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

        _unlockedPlanets.AddRange(gameMode switch
        {
            GameMode.Mode1 => [0], // Start with Pluto
            GameMode.Mode2 => [10], // Start with Sun
            _ => throw new ArgumentException("Invalid game mode")
        });
    }
}