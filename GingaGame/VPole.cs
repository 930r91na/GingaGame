using System.Drawing;

namespace GingaGame;

public class VPole
{
    public VPole(VPoint pointA, VPoint pointB)
    {
        PointA = pointA;
        PointB = pointB;
        Length = Vector2.Distance(PointA.Position, PointB.Position);
    }

    public VPoint PointA { get; set; }
    public VPoint PointB { get; set; }
    public float Length { get; set; }

    public void Update()
    {
        var delta = PointB.Position - PointA.Position;
        var currentLength = delta.Magnitude();
        var difference = Length - currentLength;
        var direction = delta / currentLength;

        if (!PointA.IsPinned && !PointB.IsPinned)
        {
            PointA.Position -= direction * (difference / 2f);
            PointB.Position += direction * (difference / 2f);
        }
        else if (PointA.IsPinned)
        {
            PointB.Position += direction * difference;
        }
        else if (PointB.IsPinned)
        {
            PointA.Position -= direction * difference;
        }
    }

    public void Render(Graphics g)
    {
        g.DrawLine(Pens.Black, PointA.Position.X, PointA.Position.Y, PointB.Position.X, PointB.Position.Y);
    }
}