using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GingaGame
{
    public class VPoint: VElement
    {
        Canvas canvas;
        public Vector2 Position;
        private Vector2 _oldPosition;
        public Vector2 Velocity;
        public Vector2 Acceleration = new Vector2(0, 0);
        public bool IsVisible { get; set; } = true;
        private const float Friction = 0.95f;
        private const float GroundFriction = 0.7f;
        private readonly Vector2 _gravity = new Vector2(0, 1);

        private const int Radius = 5;
        private const int Diameter = Radius * 2;
        public bool IsPinned { get; set; } = false;
        public float Mass { get; set; } = 1f;
        public Color Color = Color.IndianRed;

        public VPoint(float x, float y)
        {
            Position = _oldPosition = new Vector2(x, y);
        }

        public void Update(float deltaTime)
        {
            if (IsPinned) return;

            Velocity = Position - _oldPosition;
            Velocity *= Friction;

            if (Position.Y >= canvas.Height - Radius && Velocity.Y > 0.000001)
            {
                Velocity.Y *= -GroundFriction; // Apply the bounce effect
            }

            _oldPosition = Position;
            Position += Velocity + _gravity * Mass;

            Acceleration = new Vector2(0, 0); // Reset acceleration after applying gravity
        }

        public void ApplyForce(Vector2 force)
        {
            Acceleration += force / Mass;
        }

        public void Pin()
        {
            IsPinned = true;
        }

        public void Unpin()
        {
            IsPinned = false;
        }

        public void Constraints(float width, float height)
        {
            WallConstraints(width, height);
            // Include PoleConstraints here if applicable, adapted from the second code's logic
        }

        private void WallConstraints(float width, float height)
        {
            if (Position.X < Radius) Position.X = Radius;
            if (Position.X > width - Radius) Position.X = width - Radius;
            if (Position.Y < Radius) Position.Y = Radius;
            if (Position.Y > height - Radius) Position.Y = height - Radius;
        }

        // Adapt PoleConstraints method here as needed

        public void DetectAndResolveCollisions(List<VPoint> points)
        {
            foreach (var other in points.Where(p => p != this && !p.IsPinned))
            {
                CheckCollision(other);
            }
        }

        private void CheckCollision(VPoint other)
        {
            var dx = other.Position.X - this.Position.X;
            var dy = other.Position.Y - this.Position.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance < Diameter)
            {
                ResolveCollision(other, dx, dy, distance);
            }
        }

        private void ResolveCollision(VPoint other, double dx, double dy, double distance)
        {
            // Calculate angle, sine, and cosine
            var angle = Math.Atan2(dy, dx);
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);

            // Rotate velocities
            var vel0 = new Vector2((float)(Velocity.X * cos + Velocity.Y * sin), (float)(Velocity.Y * cos - Velocity.X * sin));
            var vel1 = new Vector2((float)(other.Velocity.X * cos + other.Velocity.Y * sin), (float)(other.Velocity.Y * cos - other.Velocity.X * sin));

            // Collision reaction
            var finalVel0 = new Vector2(((Radius - Radius) * vel0.X + 2 * Radius * vel1.X) / (Radius + Radius), vel0.Y);
            var finalVel1 = new Vector2((2 * Radius * vel0.X + (Radius - Radius) * vel1.X) / (Radius + Radius), vel1.Y);

            // Apply dampening
            finalVel0 *= Friction;
            finalVel1 *= Friction;

            // Update velocities
            Velocity.X = (float)(finalVel0.X * cos - finalVel0.Y * sin);
            Velocity.Y = (float)(finalVel0.Y * cos + finalVel0.X * sin);
            other.Velocity.X = (float)(finalVel1.X * cos - finalVel1.Y * sin);
            other.Velocity.Y = (float)(finalVel1.Y * cos + finalVel1.X * sin);

            // Calculate the overlap distance
            var overlap = 0.5 * (distance - Diameter - Diameter);

            // If the balls are on the same x-axis, add a random value to the x-axis
            if (dx == 0)
            {
                dx = 0.0001;
            }

            // Adjust positions
            Position.X -= (float)(overlap * dx / distance);
            Position.Y -= (float)(overlap * dy / distance);
            other.Position.X += (float)(overlap * dx / distance);
            other.Position.Y += (float)(overlap * dy / distance);
        }

        public void Render(Graphics g, float canvasWidth, float canvasHeight)
        {
            Update(0.016f); // You might need to adjust this to your game's timing system
            Constraints(canvasWidth, canvasHeight);

            g.FillEllipse(new SolidBrush(Color), Position.X - Radius, Position.Y - Radius, Diameter, Diameter);
        }
    }
}
