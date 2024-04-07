using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingaGame
{
    public class Layer
    {
        RectangleF zoom, display;
        Bitmap imgDisplay;
        public PointF Pos;

        public PointF Camera
        {
            get { return Pos; }
            set
            {
                Pos = value;
                zoom.X = (int)Pos.X;
                zoom.Y = (int)Pos.Y;
            }
        }

        public Layer(Bitmap img, Size zoom, Size display)
        {
            Pos = new PointF(0, 0);
            this.zoom = new RectangleF(0, 0, zoom.Width, zoom.Height);
            this.display = new RectangleF(0, 0, display.Width, display.Height);
            this.imgDisplay = img;
        }

        public void Display(Graphics g)
        {
            //          image     , destiny, zoom,     units
            g.DrawImage(imgDisplay, display, zoom, GraphicsUnit.Pixel);
        }
    }
}
