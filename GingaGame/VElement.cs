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

        public virtual void Render(Graphics g, Size space)
        {
            for (p = 0; p < points.Count; p++)
                points[p].Update(space.Height); 

            for (l = 0; l < 5; l++)
            {
                for (p = 0; p < poles.Count; p++)
                    poles[p].Update(); 
                for (p = 0; p < points.Count; p++)
                    points[p].DetectAndResolveCollisions(points); 
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
        public Vector2 Position { get; set; }
        public Image Texture { get; set; }
        public float Mass { get; set; }
        public float Radius { get; set; }
        public Vector2 Velocity { get; set; }

        public void ApplyForce(Vector2 force)
        {
         
            Vector2 acceleration = force / Mass;

            Velocity += acceleration; 
        }
        
        public Planet(Vector2 position, Image texture, float mass, float radius)
        {
            this.Position = position;
            this.Texture = texture;
            this.Mass = mass;
            this.Radius = radius;

        }
        public override void Render(Graphics g, Size space)
        {
            g.DrawImage(this.Texture, Position.X - Radius, Position.Y - Radius, Radius * 2, Radius * 2);
        }


    }

}

