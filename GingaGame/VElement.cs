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

    }
}

