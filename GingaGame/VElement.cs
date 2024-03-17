using GingaGame.Properties;
using System.Collections.Generic;
using System.Drawing;

namespace GingaGame
{
    public class VElement
    {
        int p, l;
        Image img, tierraImage;
        List<VPoint> points;
        List<VPole> poles;

        public List<VPole> Poles
        {
            get { return poles; }
        }
        public List<VPoint> Points
        {
            get { return points; }
        }

        public VElement()
        {
            points = new List<VPoint>();
            poles = new List<VPole>();

            // Assuming you have a way to access your resources here
            string imagePath = @"C:\Users\ansko\OneDrive\Documentos\GingaGame\GingaGame\Resources\Tierra.png";
            tierraImage = Image.FromFile(imagePath);
            img = new Bitmap(img, 50, 50);
        }

        public void AddPoint(VPoint pt)
        {
            points.Add(pt);
        }

        public void AddPole(VPole pl)
        {
            poles.Add(pl);
        }

        // This is the Update method you need to add or modify
        public void Update(float deltaTime)
        {
            // Your logic to update the element based on deltaTime
            // This could involve updating the positions of points, checking for collisions, etc.

            // Example update logic for each point
            foreach (var point in points)
            {
                // Example method call, assuming VPoint has an Update method that accepts deltaTime
                point.Update(deltaTime);
            }

            // You might also update poles or any other properties of VElement here
        }

        public void Render(Graphics g, Size space)
        {
            for (p = 0; p < points.Count; p++)
                points[p].Update(space.Height);

            for (l = 0; l < 5; l++)
            {
                for (p = 0; p < poles.Count; p++)
                    poles[p].Update();

                for (p = 0; p < points.Count; p++)
                    points[p].DetectCollision(points);

                for (p = 0; p < points.Count; p++)
                    points[p].Constraints(space.Width, space.Height);
            }

            for (p = 0; p < points.Count; p++)
            {
                if (points[p].IsVisible)
                    points[p].Render(g);
            }
            for (p = 0; p < poles.Count; p++)
                poles[p].Render(g);

            Util.DrawImage(g, img, points.ToArray());
        }

        public class ImageObject : VElement
        {
            public Image Image { get; private set; }
            public VPoint Position { get; set; } // Assuming you want to control the image object's position with this

            public ImageObject(string imagePath, VPoint position) : base()
            {
                // Instead of loading the image here, use the already loaded image from VElement if applicable
                // Or ensure you're loading it correctly from your resources
                Image = Image.FromFile(imagePath); // Consider accessing Resources directly if possible
                Position = position;
            }

            public void Render(Graphics g, Size space)
            {
                if (Image != null && Position != null)
                {
                    // Access X and Y through Position's Vector2 structure
                    float x = Position.Position.X;
                    float y = Position.Position.Y;

                    // Draw the image at the object's current position
                    // Adjust the drawing position if necessary to handle the image's size
                    g.DrawImage(Image, x - (Image.Width / 2), y - (Image.Height / 2), Image.Width, Image.Height);
                }
            }

        }
    }
}

