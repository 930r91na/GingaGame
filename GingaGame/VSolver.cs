using System;
using System.Collections.Generic;

namespace GingaGame
{
    public class VSolver
    {
        public List<VElement> Elements { get; private set; }

        public VSolver() => Elements = new List<VElement>();

        public void AddElement(VElement element) => Elements.Add(element);

        public void Update(float deltaTime)
        {
            foreach (var element in Elements) element.Update(deltaTime);
        }
        public void CheckForCollisions()
        {
            for (int i = 0; i < Elements.Count - 1; i++)
            {
                for (int j = i + 1; j < Elements.Count; j++)
                {
                    var elementA = Elements[i];
                    var elementB = Elements[j];
                    if (elementA is Planet planetA && elementB is Planet planetB)
                    {
                        float dx = planetA.Position.X - planetB.Position.X;
                        float dy = planetA.Position.Y - planetB.Position.Y;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                        // Check if the distance between planets is less than the sum of their radii
                        if (distance < (planetA.Radius + planetB.Radius))
                        {
                            // Collision detected, handle it
                            HandleCollision(planetA, planetB);
                        }
                    }
                }
            }
        }
        public void HandleCollision(Planet planetA, Planet planetB)
        {
            // A simple bounce effect by swapping velocities
            var tempVelocity = planetA.Velocity;
            planetA.Velocity = planetB.Velocity;
            planetB.Velocity = tempVelocity;

            // Optionally, adjust the positions to ensure the planets are not stuck together
            ResolveOverlap(planetA, planetB);
        }

        public void ResolveOverlap(Planet planetA, Planet planetB)
        {
            float dx = planetA.Position.X - planetB.Position.X;
            float dy = planetA.Position.Y - planetB.Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            if (distance == 0) return; // Avoid division by zero

            float overlap = 0.5f * ((planetA.Radius + planetB.Radius) - distance);
            Vector2 direction = new Vector2(dx / distance, dy / distance);

            planetA.Position += direction * overlap;
            planetB.Position -= direction * overlap;
        }



        public void CalculateGravity(List<Planet> planets)
        {
            float G = 6.67430e-11f; // Gravitational constant

            for (int i = 0; i < planets.Count; i++)
            {
                for (int j = i + 1; j < planets.Count; j++)
                {
                    Vector2 distanceVector = planets[j].Position - planets[i].Position;
                    float distance = distanceVector.Magnitude();

                    if (distance > 0) // Avoid divide by zero
                    {
                        float forceMagnitude = G * planets[i].Mass * planets[j].Mass / (distance * distance);
                        Vector2 forceDirection = distanceVector / distance;

                        planets[i].ApplyForce(forceDirection * forceMagnitude);
                        planets[j].ApplyForce(-forceDirection * forceMagnitude);
                    }
                }
            }
        }
    }
}

