using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GingaGame
{
    public class VPoint
    {
        // Properties initialization simplified
        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; set; }
        public Vector2 Acceleration { get; set; } = new Vector2(0, 0);
        public bool IsPinned { get; set; }
        public float Mass { get; set; } = 1f;
        public float CollisionRadius { get; set; } = 5f;
        public bool IsVisible { get; set; } = true;

        public VPoint(float x, float y)
        {
            Position = PreviousPosition = new Vector2(x, y);
        }

        public void Update(float deltaTime)
        {
            if (!IsPinned)
            {
                Vector2 velocity = Position - PreviousPosition + Acceleration * (deltaTime * deltaTime);
                PreviousPosition = Position;
                Position += velocity;
            }
            Acceleration = new Vector2(0, 0);
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
            Position = new Vector2(Math.Max(0, Math.Min(width, Position.X)),
                                   Math.Max(0, Math.Min(height, Position.Y)));

        }

        public bool DetectCollision(List<VPoint> points)
        {
            return points.Any(point => !ReferenceEquals(this, point) &&
                                       Vector2.Distance(this.Position, point.Position) < (this.CollisionRadius + point.CollisionRadius));
        }

        public void Render(Graphics g)
        {
            if (IsVisible)
            {
                g.FillEllipse(Brushes.Black, Position.X - 2.5f, Position.Y - 2.5f, 5, 5);
            }
        }

        }
}
