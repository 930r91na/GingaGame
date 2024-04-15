using System.Drawing;

namespace GingaGame.Shared;

public class Container
{
    public const int VerticalTopMargin = 70;
    public PointF TopLeft { get; private set; }
    public PointF TopRight { get; private set; }
    public PointF BottomLeft { get; private set; }
    public PointF BottomRight { get; private set; }

    public void InitializeGameMode1(float width, float height)
    {
        const float verticalMargin = VerticalTopMargin;
        const float horizontalLength = 310;
        var horizontalMargin = (width - horizontalLength) / 2;

        TopLeft = new PointF(horizontalMargin, verticalMargin);
        TopRight = new PointF(width - horizontalMargin, verticalMargin);
        BottomLeft = new PointF(horizontalMargin, height - verticalMargin);
        BottomRight = new PointF(width - horizontalMargin, height - verticalMargin);
    }

    public void InitializeGameMode2(float width, float height)
    {
        var horizontalMargin = (width - width / 3) / 2; // 1/3 of the width

        TopLeft = new PointF(horizontalMargin, VerticalTopMargin);
        TopRight = new PointF(width - horizontalMargin, VerticalTopMargin);
        BottomLeft = new PointF(horizontalMargin, height);
        BottomRight = new PointF(width - horizontalMargin, height);
    }

    public void Render(Graphics g)
    {
        g.DrawLine(Pens.White, TopRight, BottomRight);
        g.DrawLine(Pens.White, BottomRight, BottomLeft);
        g.DrawLine(Pens.White, BottomLeft, TopLeft);
    }
}