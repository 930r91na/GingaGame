using System.Drawing;

namespace GingaGame;

public class Container(PointF topLeft, PointF topRight, PointF bottomLeft, PointF bottomRight)
{
    public PointF TopLeft { get; } = topLeft;
    public PointF TopRight { get; } = topRight;
    public PointF BottomLeft { get; } = bottomLeft;
    private PointF BottomRight { get; } = bottomRight;

    public void Render(Graphics g)
    {
        g.DrawLine(Pens.White, TopRight, BottomRight);
        g.DrawLine(Pens.White, BottomRight, BottomLeft);
        g.DrawLine(Pens.White, BottomLeft, TopLeft);
    }
}