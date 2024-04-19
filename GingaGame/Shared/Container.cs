using System.Drawing;

namespace GingaGame.Shared;

public class Container
{
    private const int VerticalTopMargin = 70;
    public PointF TopLeft { get; private set; }
    public PointF TopRight { get; private set; }
    public PointF BottomLeft { get; private set; }
    public PointF BottomRight { get; private set; }

    public void InitializeGameMode1(float canvasWidth, float height)
    {
        const float verticalMargin = VerticalTopMargin;
        const float horizontalLength = 310;
        var horizontalMargin = (canvasWidth - horizontalLength) / 2;

        TopLeft = new PointF(horizontalMargin, verticalMargin);
        TopRight = new PointF(canvasWidth - horizontalMargin, verticalMargin);
        BottomLeft = new PointF(horizontalMargin, height - verticalMargin);
        BottomRight = new PointF(canvasWidth - horizontalMargin, height - verticalMargin);
    }

    public void InitializeGameMode2(float canvasWidth, float height, int verticalMargin = VerticalTopMargin,
        float horizontalMargin = 0)
    {
        if (horizontalMargin <= 0) horizontalMargin = (canvasWidth - canvasWidth / 3) / 2; // 1/3 of the width

        TopLeft = new PointF(horizontalMargin, verticalMargin);
        TopRight = new PointF(canvasWidth - horizontalMargin, verticalMargin);
        BottomLeft = new PointF(horizontalMargin, height);
        BottomRight = new PointF(canvasWidth - horizontalMargin, height);
    }

    public void Render(Graphics g, float yOffset = 0)
    {
        // Adjust the Y position with the offset
        var adjustedTopLeft = TopLeft with { Y = TopLeft.Y - yOffset };
        var adjustedBottomLeft = BottomLeft with { Y = BottomLeft.Y - yOffset };
        var adjustedTopRight = TopRight with { Y = TopRight.Y - yOffset };
        var adjustedBottomRight = BottomRight with { Y = BottomRight.Y - yOffset };

        // Draw the lines with the adjusted Y positions
        g.DrawLine(Pens.White, adjustedTopLeft, adjustedBottomLeft);
        g.DrawLine(Pens.White, adjustedBottomLeft, adjustedBottomRight);
        g.DrawLine(Pens.White, adjustedBottomRight, adjustedTopRight);
    }
}