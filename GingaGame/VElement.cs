using GingaGame.Properties;
using System.Collections.Generic;
using System.Drawing;

namespace GingaGame
{
    public class VElement
    {
        int p, l;
        Image img;
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
        public void Update(float deltaTime)
        {
            foreach (var point in points)
            {
                point.Update(deltaTime);
            }
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
            }

            for (p = 0; p < points.Count; p++)
            {
                if (points[p].IsVisible)
                    points[p].Render(g, space.Width, space.Height);
            }
            for (p = 0; p < poles.Count; p++)
                poles[p].Render(g); 
            Util.DrawImage(g, img, points.ToArray());
        }
    }
    public class Planet : VElement
    {
        
    }

}

