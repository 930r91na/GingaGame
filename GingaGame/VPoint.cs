using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GingaGame
{
    public class VPoint
    {
        public Vector2 Position { get; set; }
        private Vector2 _oldPosition;
        public Vector2 Velocity;
        public bool IsVisible { get; set; } = true;
        private const float Friction = 0.95f;
        private const float GroundFriction = 0.7f;
        private readonly Vector2 _gravity = new Vector2(0, 1);

        private const int Radius = 5;
        public bool IsPinned { get; set; }
        public float Mass { get; set; } = 1f;
        public Color Color = Color.IndianRed; // Assuming you have a way to set and use this color

        public VPoint(float x, float y)
        {
            Position = _oldPosition = new Vector2(x, y);
        }

        public void Update(float deltaTime)
        {
            if (IsPinned) return;

            Velocity = Position - _oldPosition;
            Velocity *= Friction;

            _oldPosition = Position;
            Position += Velocity + _gravity * Mass * deltaTime * deltaTime; // Adjusted gravity application for deltaTime squared
        }

        public void ApplyForce(Vector2 force)
        {
            // Adjusting for deltaTime in ApplyForce might not be directly needed, it's more about how force affects acceleration
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
            float deltaX = 1.0f;
            float deltaY = 2.0f;
            Position = new Vector2(Position.X + deltaX, Position.Y + deltaY);

        }

        public bool DetectCollision(List<VPoint> points)
        {
            // Simplified collision detection; consider complex scenarios and response as in CODE 1
            return points.Any(other => !ReferenceEquals(this, other) && !other.IsPinned &&
                                       Vector2.Distance(this.Position, other.Position) < Radius * 2);
        }

        public void Render(Graphics g, float canvasWidth, float canvasHeight)
        {
            Update(0.016f); // Assuming a fixed update step for simplicity
            Constraints(canvasWidth, canvasHeight);

            // Rendering adjusted to demonstrate dynamic color usage
            g.FillEllipse(new SolidBrush(Color), Position.X - Radius, Position.Y - Radius, Radius * 2, Radius * 2);
        }

        private Vector2 Acceleration = new Vector2(0, 0); // Ensuring acceleration is part of the class structure
    }
}
