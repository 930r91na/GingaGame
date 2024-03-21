namespace GingaGame;

public class VPoint
{
    private const float Friction = 0.85f;
    private readonly Canvas _canvas;
    private readonly Vector2 _gravity = new(0, 1);

    private readonly float _radius;
    public bool IsPinned;
    public Vector2 OldPosition;
    public Vector2 Position;
    public Vector2 Velocity;

    protected VPoint(float x, float y, Canvas canvas, float radius)
    {
        Position = new Vector2(x, y);
        _canvas = canvas;
        _radius = radius;
        Mass = radius / 10;
        Position = OldPosition = new Vector2(x, y);
    }

    public float Mass { get; }

    public void Update()
    {
        if (IsPinned) return;

        Velocity = Position - OldPosition;
        Velocity *= Friction;

        // Save current position
        OldPosition = Position;

        // Perform Verlet integration
        Position += Velocity + _gravity; // * Mass;
    }

    public void Constraints()
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
        if (container != null && Position.X < container.TopLeft.X + _radius) Position.X = container.TopLeft.X + _radius;

        // Check if the point is outside the right boundary of the container
        if (container != null && Position.X > container.TopRight.X - _radius)
            Position.X = container.TopRight.X - _radius;

        // Check if the point is outside the bottom boundary of the container
        if (container != null && Position.Y > container.BottomLeft.Y - _radius)
            Position.Y = container.BottomLeft.Y - _radius;
    }
}