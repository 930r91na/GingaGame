using System;
using System.Collections.Generic;
using GingaGame.GameMode1;

namespace GingaGame.Shared;

public class CollisionHandler(
    Scene scene,
    Canvas canvas,
    CollisionHandler collisionHandler,
    PlanetFactory planetFactory,
    Score score,
    Container container)
{
    private readonly List<Planet> _planets = scene.Planets;
    private readonly List<(Planet, Planet)> _potentialCollisionPairs = [];

    public void CheckCollisions()
    {
        // Clear the list of potential collision pairs from the previous frame
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

            // Calculate distance between centers
            var distanceX = planet1.Position.X - planet2.Position.X;
            var distanceY = planet1.Position.Y - planet2.Position.Y;
            var distanceSquared = distanceX * distanceX + distanceY * distanceY;

            // Check against sum of radii
            var sumOfRadii = planet1.Radius + planet2.Radius;
            var sumOfRadiiSquared = sumOfRadii * sumOfRadii;

            if (distanceSquared > sumOfRadiiSquared)
            {
                // No collision possible
            }

            // Potential collision - pass to Narrow Phase
            _potentialCollisionPairs.Add((planet1, planet2));
        }
    }

    private void NarrowPhaseCheck()
    {
        // Iterate through pairs of planets that passed the broad phase
        if (_potentialCollisionPairs.Count == 0) return;

        // Create a copy of the list
        var potentialCollisionPairsCopy = new List<(Planet, Planet)>(_potentialCollisionPairs);

        foreach (var (planet1, planet2) in potentialCollisionPairsCopy)
        {
            // Recalculate distance (more accurate, as in broad-phase it might be an overestimate)
            var distanceX = planet1.Position.X - planet2.Position.X;
            var distanceY = planet1.Position.Y - planet2.Position.Y;
            var distanceSquared = distanceX * distanceX + distanceY * distanceY;
            var sumOfRadiiSquared = (planet1.Radius + planet2.Radius) * (planet1.Radius + planet2.Radius);

            if (distanceSquared <= sumOfRadiiSquared)
                // Collision detected!
                HandleCollision(planet1, planet2);
        }
    }

    private void HandleCollision(Planet planet1, Planet planet2)
    {
        // Merging planets 
        if (planet1.PlanetType == planet2.PlanetType)
        {
            // Unlock a larger planet in the PlanetFactory
            planetFactory.UnlockPlanet(planet1.PlanetType + 1);

            // Remove both planets from the scene or mark them inactive
            scene.RemoveElement(planet1);
            scene.RemoveElement(planet2);

            // Create a new planet with the next type
            // The position will be the middle point between the two planets
            var middlePoint = (planet1.Position + planet2.Position) / 2;

            // Check if two planets are the largest type (10)
            if (planet1.PlanetType + 1 >= 11)
            {
                // Update scores
                score.IncreaseScore(100);
                score.HasChanged = true;
                return;
            }

            var newPlanet = new Planet(planet1.PlanetType + 1, middlePoint.X, middlePoint.Y, collisionHandler);

            // Add the new planet to the scene
            scene.AddElement(newPlanet);

            // Update scores
            score.IncreaseScore(newPlanet.Points);
            score.HasChanged = true;

            // Handle collisions again, as the new planet might collide with others
            CheckCollisions();
        }
        else
        {
            const int iterations = 8;

            for (var i = 0; i < iterations; i++)
            {
                // 1. Overlap Correction
                var overlap = planet1.Radius + planet2.Radius - Vector2.Distance(planet1.Position, planet2.Position);
                var normal = (planet1.Position - planet2.Position).Normalized(); // Collision direction
                var positionAdjustment = normal * overlap / 2;

                // 2. Velocity Threshold Check
                const float velocityThreshold = 0.8f;
                var relativeVelocity = planet1.Velocity - planet2.Velocity;
                var velocityAlongNormal = relativeVelocity.Dot(normal);

                var totalMass = planet1.Mass + planet2.Mass;
                var massRatio1 = planet2.Mass / totalMass;
                var massRatio2 = planet1.Mass / totalMass;

                planet1.Position += positionAdjustment * massRatio1;
                planet2.Position -= positionAdjustment * massRatio2;

                if (Math.Abs(velocityAlongNormal) < velocityThreshold) continue;

                // Simulating bounce and merging logic if velocity is high enough
                const float bounceFactor = 0.1f;
                var separationVelocity = normal * bounceFactor;
                planet1.Position += separationVelocity;
                planet2.Position -= separationVelocity;

                // Update has collided
                planet1.HasCollided = true;
                planet2.HasCollided = true;
            }
        }
    }

    public void CheckConstraints(VPoint point)
    {
        WallConstraints(point);
        ContainerConstraints(point);
    }

    private void WallConstraints(VPoint point)
    {
        if (point.Position.X < point.Radius) point.Position.X = point.Radius;
        if (point.Position.X > canvas.Width - point.Radius) point.Position.X = canvas.Width - point.Radius;
        if (point.Position.Y < point.Radius) point.Position.Y = point.Radius;
        if (point.Position.Y > canvas.Height - point.Radius) point.Position.Y = canvas.Height - point.Radius;
    }

    private void ContainerConstraints(VPoint point)
    {
        // Check if the point is outside the left boundary of the container
        if (container != null && point.Position.X < container.TopLeft.X + point.Radius)
            point.Position.X = container.TopLeft.X + point.Radius;

        // Check if the point is outside the right boundary of the container
        if (container != null && point.Position.X > container.TopRight.X - point.Radius)
            point.Position.X = container.TopRight.X - point.Radius;

        // Check if the point is outside the bottom boundary of the container
        if (container != null && point.Position.Y > container.BottomLeft.Y - point.Radius)
            point.Position.Y = container.BottomLeft.Y - point.Radius;
    }
}