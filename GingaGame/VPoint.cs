using System.Drawing;

namespace GingaGame;

public class VPoint : VElement
{
    private const float Friction = 0.95f;
    private const float GroundFriction = 0.7f;

    public readonly float Radius;
    private readonly Vector2 _gravity = new(0, 1);
    private Vector2 _oldPosition;
    private readonly Canvas _canvas;
    public Vector2 Position;
    private Vector2 _velocity;
    private readonly Image _texture;

    public VPoint(float x, float y, Canvas canvas, Image texture, float mass, float radius)
    {
        Position = new Vector2(x, y);
        _canvas = canvas;
        _texture = texture;
        Mass = mass;
        Radius = radius;
        Position = _oldPosition = new Vector2(x, y);
    }

    public bool IsPinned { get; set; }
    private float Mass { get; }

    public override void Update()
    {
        if (IsPinned) return;

        _velocity = Position - _oldPosition;
        _velocity *= Friction;

        if (Position.Y >= _canvas.Height - Radius &&
            _velocity.Y > 0.000001) _velocity.Y *= -GroundFriction; // Apply the bounce effect

        _oldPosition = Position;
        Position += _velocity + _gravity * Mass;
    }

    public override void Constraints()
    {
        WallConstraints();
    }

    private void WallConstraints()
    {
        if (Position.X < Radius) Position.X = Radius;
        if (Position.X > _canvas.Width - Radius) Position.X = _canvas.Width - Radius;
        if (Position.Y < Radius) Position.Y = Radius;
        if (Position.Y > _canvas.Height - Radius) Position.Y = _canvas.Height - Radius;
    }

    public override void Render(Graphics g)
    {
        Update();
        Constraints();
        var imageWidth = Radius * 2;
        var imageHeight = Radius * 2;
        g?.DrawImage(_texture, Position.X - imageWidth / 2, Position.Y - imageHeight / 2, imageWidth, imageHeight);
        // Draw the middle of the point
        //g?.DrawLine(Pens.Yellow, (float)(Position.X - 0.5), (float)(Position.Y - 0.5), (float)(Position.X + 0.5), (float)(Position.Y + 0.5));
    }
}