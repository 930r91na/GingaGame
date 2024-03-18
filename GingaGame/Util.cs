using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GingaGame;

public class Util
{
    private static Util instance;
    public Random rand;

    public Util()
    {
        rand = new Random();
    }

    public static Util Instance
    {
        get
        {
            if (instance == null)
                instance = new Util();
            return instance;
        }
    }

    public static float Distance(VPoint pt0, VPoint pt1)
    {
        var dx = pt0.Position.X - pt1.Position.X;
        var dy = pt0.Position.Y - pt1.Position.Y;

        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    public static float Distance(PointF pt0, PointF pt1)
    {
        var dx = pt0.X - pt1.X;
        var dy = pt0.Y - pt1.Y;

        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    public static void DrawImage(Graphics g, Image img, VPoint[] pts)
    {
        var w = Distance(pts[0], pts[1]);
        var h = Distance(pts[0], pts[3]);

        var dx = pts[1].Position.X - pts[0].Position.X;
        var dy = pts[1].Position.Y - pts[0].Position.Y;

        var angle = (float)(Math.Atan2(dy, dx) * 57.2958);

        RotateImage(g, img, angle, pts[0].Position.X, pts[0].Position.Y);
    }

    private static void RotateImage(Graphics g, Image image, float angle, float x, float y)
    {
        var originalTransform = g.Transform;
        Matrix traMatrix, rotMatrix;
        rotMatrix = new Matrix();
        traMatrix = new Matrix();
        traMatrix.Translate(x, y);
        rotMatrix.Rotate(angle);
        rotMatrix.Multiply(traMatrix, MatrixOrder.Append);
        g.Transform = rotMatrix;
        g.DrawImage(image, new PointF(0, 0));
        g.Transform = originalTransform;
    }
}