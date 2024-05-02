using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GingaGame.GameMode1;
using GingaGame.UI;

namespace GingaGame.Shared;

public class CollisionHandler(
    Scene scene,
    PlanetFactory planetFactory,
    Score score,
    Container container,
    GameMode gameMode,
    GameMode2Control gameMode2Control = null,
    GameStateHandler gameStateHandler = null)
{
    private readonly List<Planet> _planets = scene.Planets;
    private readonly List<(Planet, Planet)> _potentialCollisionPairs = [];

    public void CheckCollisions()
    {
        _potentialCollisionPairs.Clear();

        // Step 1: Broad Phase
        BroadPhaseCheck();

        // Step 2: Narrow Phase
        NarrowPhaseCheck();
    }

    private void BroadPhaseCheck()
    {
        for (var i = 0; i < _planets.Count; i++)
        for (var j = i + 1; j < _planets.Count; j++)
        {
            var planet1 = _planets[i];
            var planet2 = _planets[j];

            // If the planets are pinned, they cannot collide
            if (planet1.IsPinned || planet2.IsPinned) continue;

            // Calculate bounding boxes
            if (!DoBoundingBoxesIntersect(planet1, planet2)) continue; // No potential collision

            // Potential collision - pass to Narrow Phase
            _potentialCollisionPairs.Add((planet1, planet2));
        }
    }

    private static bool DoBoundingBoxesIntersect(Planet planet1, Planet planet2)
    {
        var box1 = new RectangleF(planet1.Position.X - planet1.Radius, planet1.Position.Y - planet1.Radius,
            planet1.Radius * 2, planet1.Radius * 2);
        var box2 = new RectangleF(planet2.Position.X - planet2.Radius, planet2.Position.Y - planet2.Radius,
            planet2.Radius * 2, planet2.Radius * 2);

        return box1.IntersectsWith(box2);
    }

    private void NarrowPhaseCheck()
    {
        // If there are no potential collision pairs, no need to check further
        if (_potentialCollisionPairs.Count == 0) return;

        // Create a copy of the list
        var potentialCollisionPairsCopy = new List<(Planet, Planet)>(_potentialCollisionPairs);

        // Iterate through pairs of planets that passed the broad phase
        foreach (var (planet1, planet2) in potentialCollisionPairsCopy)
        {
            // Recalculate distance (more accurate, as in broad-phase it might be an overestimate)
            var distanceX = planet1.Position.X - planet2.Position.X;
            var distanceY = planet1.Position.Y - planet2.Position.Y;
            var distanceSquared = distanceX * distanceX + distanceY * distanceY;
            var sumOfRadiiSquared = (planet1.Radius + planet2.Radius) * (planet1.Radius + planet2.Radius);

            if (distanceSquared <= sumOfRadiiSquared) // Collision detected
                HandleCollision(planet1, planet2);
        }
    }

    private void HandleCollision(Planet planet1, Planet planet2)
    {
        // Merging planets 
        if (planet1.PlanetType == planet2.PlanetType)
        {
            // Handle same planet collision
            var mergedPlanet = MergePlanets(planet1, planet2);

            if (mergedPlanet == null) return; // No new planet to process

            gameStateHandler.CheckWinCondition(mergedPlanet);

            // Handle collisions again, as the new planet might collide with others
            CheckCollisions();
        }
        else
        {
            // Handle different planet collision
            HandleDifferentPlanetCollision(planet1, planet2);

            // Check if the game is over
            gameStateHandler.CheckGameEndConditions(planet1);
            gameStateHandler.CheckGameEndConditions(planet2);
        }
    }

    private Planet MergePlanets(Planet planet1, Planet planet2)
    {
        scene.RemovePlanet(planet1);
        scene.RemovePlanet(planet2);

        // Unlock new planet (if needed)
        if (!UnlockNextPlanetType(planet1, planet2)) return null;

        // Create a new planet
        var mergedPlanet = CreateMergedPlanet(planet1, planet2);

        // Update the current planet in GameMode2 if needed
        if (gameMode == GameMode.Mode2)
        {
            var currentPlanet = gameMode2Control.GetCurrentPlanet();
            if (currentPlanet == planet1 || currentPlanet == planet2)
                gameMode2Control.SetCurrentPlanet(mergedPlanet);
        }

        // Add the new planet to the scene
        scene.AddPlanet(mergedPlanet);

        // Update scores for game mode 1
        if (gameMode == GameMode.Mode1)
            UpdateScoreWithPlanetPoints(mergedPlanet.Points);

        return mergedPlanet;
    }

    private bool UnlockNextPlanetType(Planet planet1, Planet planet2)
    {
        switch (gameMode)
        {
            case GameMode.Mode1:
                if (planet1.PlanetType + 1 >= 11) // if two largest planets are merged
                {
                    const int largestPlanetScore = 100;
                    UpdateScoreWithPlanetPoints(largestPlanetScore);
                    return false; // No new planet to unlock
                }

                // Unlock the next planet
                planetFactory.UnlockPlanet(planet1.PlanetType + 1);
                break;

            case GameMode.Mode2:
                if (planet2.PlanetType - 1 <= 0) // if two smallest planets are merged
                    return false; // No new planet to unlock

                // Unlock the previous planet
                planetFactory.UnlockPlanet(planet2.PlanetType - 1);
                break;
            default:
                throw new ArgumentException("Invalid game mode");
        }

        return true;
    }

    private Planet CreateMergedPlanet(Planet planet1, Planet planet2)
    {
        // The position of the new planet will be the middle point between the two planets
        var middlePoint = (planet1.Position + planet2.Position) / 2;

        var newPlanet = gameMode switch
        {
            GameMode.Mode1 => new Planet(planet1.PlanetType + 1, middlePoint),
            GameMode.Mode2 => new Planet(planet2.PlanetType - 1, middlePoint),
            _ => throw new ArgumentException("Invalid game mode")
        };
        return newPlanet;
    }

    private void UpdateScoreWithPlanetPoints(int largestPlanetScore)
    {
        score.IncreaseScore(largestPlanetScore);
        score.HasChanged = true;
    }

    private static void HandleDifferentPlanetCollision(Planet planet1, Planet planet2)
    {
        // 1. Overlap Correction
        CorrectOverlap(planet1, planet2);

        // 2. Simulate Bounce if velocity is high enough
        SimulateBounce(planet1, planet2);
    }

    private static void CorrectOverlap(Planet planet1, Planet planet2)
    {
        const float overlapCorrectionFactor = 0.5f;

        var overlap = planet1.Radius + planet2.Radius - Vector2.Distance(planet1.Position, planet2.Position);
        var normal = (planet1.Position - planet2.Position).Normalized(); // Collision direction
        var positionAdjustment = normal * overlap * overlapCorrectionFactor;

        var totalMass = planet1.Mass + planet2.Mass;
        var massRatio1 = planet2.Mass / totalMass;
        var massRatio2 = planet1.Mass / totalMass;

        planet1.Position += positionAdjustment * massRatio1;
        planet2.Position -= positionAdjustment * massRatio2;
    }

    private static void SimulateBounce(Planet planet1, Planet planet2)
    {
        // Check if the velocity is high enough for a bounce
        const float velocityThreshold = 0.8f;
        var relativeVelocity = planet1.Velocity - planet2.Velocity;
        var normal = (planet1.Position - planet2.Position).Normalized();
        var velocityAlongNormal = relativeVelocity.Dot(normal);

        // If the velocity is not high enough, no bounce
        if (Math.Abs(velocityAlongNormal) < velocityThreshold) return;

        // Bounce the planets
        const float bounceFactor = 0.1f;
        var separationVelocity = normal * bounceFactor;
        planet1.Position += separationVelocity;
        planet2.Position -= separationVelocity;

        planet1.HasCollided = true;
        planet2.HasCollided = true;
    }

    public void CheckConstraints(Planet planet)
    {
        WallConstraints(planet);
        ContainerConstraints(planet);
        if (gameMode == GameMode.Mode2) FloorConstraints(planet);
    }

    private static void WallConstraints(Planet planet)
    {
        // Check if the point is outside the top boundary of the wall
        if (planet.Position.Y < planet.Radius) planet.Position.Y = planet.Radius;
    }

    private void ContainerConstraints(Planet planet)
    {
        // Check if the point is outside the left boundary of the container
        if (container != null && planet.Position.X < container.TopLeft.X + planet.Radius)
            planet.Position.X = container.TopLeft.X + planet.Radius;

        // Check if the point is outside the right boundary of the container
        if (container != null && planet.Position.X > container.TopRight.X - planet.Radius)
            planet.Position.X = container.TopRight.X - planet.Radius;

        // Check if the point is outside the bottom boundary of the container
        if (container != null && planet.Position.Y > container.BottomLeft.Y - planet.Radius)
            planet.Position.Y = container.BottomLeft.Y - planet.Radius;
    }

    private void FloorConstraints(Planet planet)
    {
        if (gameMode != GameMode.Mode2) return; // Apply only in GameMode2

        // Find the current floor
        var floor = scene.Floors.FirstOrDefault(f =>
            f.StartPositionY <= planet.Position.Y && planet.Position.Y <= f.EndPositionY);

        if (floor == null) return; // Planet is outside the floor range

        // Check if the planet can pass through the floor
        if (planet.PlanetType <= floor.NextPlanetIndex)
            // Can pass - no collision
            return;

        const int floorEndPositionHeight = 30;

        // Handle Collision (similar to container boundaries)
        if (!(planet.Position.Y > floor.EndPositionY - floorEndPositionHeight - planet.Radius)) return;

        if (floor.NextPlanetIndex == -1) // Last floor
            // Game Won
            gameMode2Control.GameWon();
        planet.Position.Y = floor.EndPositionY - floorEndPositionHeight - planet.Radius;
    }

    public bool IsGameOver()
    {
        // Check if a planet has passed the end line
        return _planets.Any(planet =>
            planet.Position.Y < container.TopLeft.Y + planet.Radius && planet.HasCollided);
    }
}