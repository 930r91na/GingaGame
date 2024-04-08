using System.Drawing;

namespace GingaGame.GameMode1;

public class Container
{
    public PointF TopLeft { get; private set; }
    public PointF TopRight { get; private set; }
    public PointF BottomLeft { get; private set; }
    public PointF BottomRight { get; private set; }

    public void Initialize(float width, float height)
    {
        const float verticalTopMargin = 70;
        const float verticalBottomMargin = 20;
        var horizontalMargin = (width - width / 3) / 2;
        TopLeft = new PointF(horizontalMargin, verticalTopMargin);
        TopRight = new PointF(width - horizontalMargin, verticalTopMargin);
        BottomLeft = new PointF(horizontalMargin, height - verticalBottomMargin);
        BottomRight = new PointF(width - horizontalMargin, height - verticalBottomMargin);
    }

    public void Render(Graphics g)
    {
        g.DrawLine(Pens.White, TopRight, BottomRight);
        g.DrawLine(Pens.White, BottomRight, BottomLeft);
        g.DrawLine(Pens.White, BottomLeft, TopLeft);
    }
}