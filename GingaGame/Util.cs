using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace GingaGame
{
    public class Util
    {
        static Util instance;
        public Random rand;

        public static Util Instance
        {
            get
            {
                if (instance == null)
                    instance = new Util();
                return Util.instance;
            }
        }

        public Util()
        {
            rand = new Random();
        }

        public static float Distance(VPoint pt0, VPoint pt1)
        {
            float dx = (pt0.Position.X - pt1.Position.X);
            float dy = (pt0.Position.Y - pt1.Position.Y);

            return (float)Math.Sqrt((dx * dx) + (dy * dy));
        }

        public static float Distance(PointF pt0, PointF pt1)
        {
            float dx = (pt0.X - pt1.X);
            float dy = (pt0.Y - pt1.Y);

            return (float)Math.Sqrt((dx * dx) + (dy * dy));
        }

        public static void DrawImage(Graphics g, Image img, VPoint[] pts)
        {
            float w = Distance(pts[0], pts[1]);
            float h = Distance(pts[0], pts[3]);

            float dx = pts[1].Position.X - pts[0].Position.X;
            float dy = pts[1].Position.Y - pts[0].Position.Y;

            float angle = (float)(Math.Atan2(dy, dx) * (57.2958));

            RotateImage(g, img, angle, pts[0].Position.X, pts[0].Position.Y);
        }

        private static void RotateImage(Graphics g, Image image, float angle, float x, float y)
        {
            Matrix originalTransform = g.Transform;
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

    }
