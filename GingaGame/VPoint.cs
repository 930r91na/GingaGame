using System;
using System.Drawing;

namespace GingaGame;

public class VPoint : VElement
{
    private const float Friction = 0.95f;

    private float _radius;
    private readonly Vector2 _gravity = new(0, 1);
    private Vector2 _oldPosition;
    private readonly Canvas _canvas;
    public Vector2 Position;
    private Vector2 _velocity;
    private Image _texture;
    private static bool IsPinned => false;

    private float Mass { get; set; }

    public VPoint(float x, float y, Canvas canvas, Image texture, float mass, float radius)
    {
        Position = new Vector2(x, y);
        _canvas = canvas;
        _texture = texture;
        Mass = mass;
        _radius = radius;
        Position = _oldPosition = new Vector2(x, y);
    }

    public override void Update()
    {
        if (IsPinned) return;
        
        _velocity = Position - _oldPosition;
        _velocity *= Friction;
        _oldPosition = Position;
        Position += _velocity + _gravity * Mass;
    }
    public override void Constraints()
    {
        WallConstraints();
        ContainerConstraints();
    }

    private void WallConstraints()
    {
        if (Position.X < _radius) Position.X = _radius;
        if (Position.X > _canvas.Width - _radius) Position.X = _canvas.Width - _radius;
        if (Position.Y < _radius) Position.Y = _radius;
        if (Position.Y > _canvas.Height - _radius) Position.Y = _canvas.Height - _radius;
    }
    
    private void ContainerConstraints()
    {
        var container = _canvas.Container;

        // Check if the point is outside the left boundary of the container
        if (container != null && Position.X < container.TopLeft.X + _radius)
        {
            Position.X = container.TopLeft.X + _radius;
        }

        // Check if the point is outside the right boundary of the container
        if (container != null && Position.X > container.TopRight.X - _radius)
        {
            Position.X = container.TopRight.X - _radius;
        }

        // Check if the point is outside the bottom boundary of the container
        if (container != null && Position.Y > container.BottomLeft.Y - _radius)
        {
            Position.Y = container.BottomLeft.Y - _radius;
        }
    }
    
    public override bool CollidesWith(VElement other)
    {
        if (other is not VPoint otherPoint) return false;
        float distance = Vector2.Distance(Position, otherPoint.Position);
        return distance <= _radius + otherPoint._radius;
    }

    public override void HandleCollision(VElement other)
    {
        if (other is not VPoint otherPoint) return;

        // 1. Overlap Correction
        var overlap = _radius + otherPoint._radius - Vector2.Distance(Position, otherPoint.Position);
        var normal = (Position - otherPoint.Position).Normalized(); // Collision direction
        var positionAdjustment = normal * overlap / 2;

        // 2. Velocity Threshold Check
        const float velocityThreshold = 0.8f; // Adjust this value as needed
        var relativeVelocity = _velocity - otherPoint._velocity;
        var velocityAlongNormal = relativeVelocity.Dot(normal); 

        if (Math.Abs(velocityAlongNormal) < velocityThreshold)
        {
            Position += positionAdjustment;
            otherPoint.Position -= positionAdjustment;
        }
        else
        {
            // 1. Overlap Correction
            Position += positionAdjustment;
            otherPoint.Position -= positionAdjustment;
            
            // 2. Simulating bounce and merging logic if velocity is high enough
            const float bounceFactor = 0.8f;
            var separationVelocity = normal * bounceFactor;
            Position += separationVelocity;
            otherPoint.Position -= separationVelocity;

            // Merging Logic
            if (_texture != otherPoint._texture) return;
            // Replace '_texture' and adjust mass/radius based on a table or logic
            _texture = GetNextPlanetTexture(_texture);
            _radius *= 1.5f;
            Mass *= 2f;
        }
    }
    
    private Image GetNextPlanetTexture(Image currentTexture)
    {
        // Placeholder! Replace with logic to get the next planet texture
        return currentTexture;
    }


    public override void Render(Graphics g)
    {
        Constraints();
        var imageWidth = _radius * 2;
        var imageHeight = _radius * 2;
        g?.DrawImage(_texture, Position.X - imageWidth / 2, Position.Y - imageHeight / 2, imageWidth, imageHeight);
        // Draw the collision circle
        g?.DrawEllipse(new Pen(Color.Red, 1), Position.X - _radius, Position.Y - _radius, _radius * 2, _radius * 2);
    }
}