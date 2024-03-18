using System.Drawing;

namespace GingaGame;

public class VPole : VElement
{
    public VPole(VPoint pointA, VPoint pointB)
    {
        PointA = pointA;
        PointB = pointB;
        Length = Vector2.Distance(PointA.Position, PointB.Position);
    }

    private VPoint PointA { get; }
    private VPoint PointB { get; }
    private float Length { get; }

    public override void Update()
    {
        var delta = PointB.Position - PointA.Position;
        var currentLength = delta.Magnitude();
        var difference = Length - currentLength;
        var direction = delta / currentLength;

        switch (PointA.IsPinned)
        {
            case false when !PointB.IsPinned:
                PointA.Position -= direction * (difference / 2f);
                PointB.Position += direction * (difference / 2f);
                break;
            case true:
                PointB.Position += direction * difference;
                break;
            default:
            {
                if (PointB.IsPinned)
                {
                    PointA.Position -= direction * difference;
                }
                break;
            }
        }
    }

    public override void Render(Graphics g)
    {
        g?.DrawLine(Pens.White, PointA.Position.X, PointA.Position.Y, PointB.Position.X, PointB.Position.Y);
    }

    public override void Constraints()
    {
        // No constraints
    }
}