using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Windows.Forms;

namespace GingaGame;

public class VPoint : VElement
{
    private const float Friction = 0.95f;
    private const float GroundFriction = 0.7f;

    private float _radius;
    private readonly Vector2 _gravity = new(0, 1);
    private Vector2 _oldPosition;
    public Vector2 Acceleration = new(0, 0);
    public Color Color = Color.IndianRed;
    private readonly Canvas _canvas;
    public Vector2 Position;
    public Vector2 Velocity;
    public Image Texture;

    public VPoint(float x, float y, Canvas canvas, Image texture, float mass, float radius)
    {
        Position = new Vector2(x, y);
        _canvas = canvas;
        Texture = texture;
        Mass = mass;
        _radius = radius;
        Position = _oldPosition = new Vector2(x, y);
    }

    public bool IsVisible { get; set; } = true;
    public bool IsPinned { get; set; }
    public float Mass { get; set; } = 1f;

    public override void Update()
    {
        if (IsPinned) return;

        Velocity = Position - _oldPosition;
        Velocity *= Friction;

        if (Position.Y >= _canvas.Height - _radius &&
            Velocity.Y > 0.000001) Velocity.Y *= -GroundFriction; // Apply the bounce effect

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

    public override void Constraints()
    {
        WallConstraints();
    }

    private void WallConstraints()
    {
        if (Position.X < _radius) Position.X = _radius;
        if (Position.X > _canvas.Width - _radius) Position.X = _canvas.Width - _radius;
        if (Position.Y < _radius) Position.Y = _radius;
        if (Position.Y > _canvas.Height - _radius) Position.Y = _canvas.Height - _radius;
    }
    
    public void DetectAndResolveCollisions(List<VPoint> points)
    {
        foreach (var other in points.Where(p => p != this && !p.IsPinned)) CheckCollision(other);
    }

    private void CheckCollision(VPoint other)
    {
        var dx = other.Position.X - Position.X;
        var dy = other.Position.Y - Position.Y;
        var distance = Math.Sqrt(dx * dx + dy * dy);

        if (distance < _radius * _radius) ResolveCollision(other, dx, dy, distance);
    }

    private void ResolveCollision(VPoint other, double dx, double dy, double distance)
    {
        // Calculate angle, sine, and cosine
        var angle = Math.Atan2(dy, dx);
        var sin = Math.Sin(angle);
        var cos = Math.Cos(angle);

        // Rotate velocities
        var vel0 = new Vector2((float)(Velocity.X * cos + Velocity.Y * sin),
            (float)(Velocity.Y * cos - Velocity.X * sin));
        var vel1 = new Vector2((float)(other.Velocity.X * cos + other.Velocity.Y * sin),
            (float)(other.Velocity.Y * cos - other.Velocity.X * sin));

        // Collision reaction
        var finalVel0 = new Vector2(((_radius - _radius) * vel0.X + 2 * _radius * vel1.X) / (_radius + _radius), vel0.Y);
        var finalVel1 = new Vector2((2 * _radius * vel0.X + (_radius - _radius) * vel1.X) / (_radius + _radius), vel1.Y);

        // Apply dampening
        finalVel0 *= Friction;
        finalVel1 *= Friction;

        // Update velocities
        Velocity.X = (float)(finalVel0.X * cos - finalVel0.Y * sin);
        Velocity.Y = (float)(finalVel0.Y * cos + finalVel0.X * sin);
        other.Velocity.X = (float)(finalVel1.X * cos - finalVel1.Y * sin);
        other.Velocity.Y = (float)(finalVel1.Y * cos + finalVel1.X * sin);

        // Calculate the overlap distance
        var overlap = 0.5 * (distance - _radius * _radius - _radius * _radius);

        // If the balls are on the same x-axis, add a random value to the x-axis
        if (dx == 0) dx = 0.0001;

        // Adjust positions
        Position.X -= (float)(overlap * dx / distance);
        Position.Y -= (float)(overlap * dy / distance);
        other.Position.X += (float)(overlap * dx / distance);
        other.Position.Y += (float)(overlap * dy / distance);
    }

    public override void Render(Graphics g)
    {
        Update();
        Constraints();
        g?.DrawImage(Texture, Position.X - _radius, Position.Y - _radius, _radius * _radius, _radius * _radius);
    }
    
}