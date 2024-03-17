using System.Collections.Generic;
using System.Drawing;

namespace GingaGame
{
    internal class Container
    {
        public List<VPole> Boundaries { get; private set; }

        public Container(VPoint topLeft, VPoint topRight, VPoint bottomLeft, VPoint bottomRight)
        {
            Boundaries = new List<VPole>();

            var leftWall = new VPole(topLeft, bottomLeft);
            var rightWall = new VPole(topRight, bottomRight);
            var baseWall = new VPole(bottomLeft, bottomRight);

            topLeft.IsPinned = true;
            topRight.IsPinned = true;
            bottomLeft.IsPinned = true;
            bottomRight.IsPinned = true;

            Boundaries.Add(leftWall);
            Boundaries.Add(rightWall);
            Boundaries.Add(baseWall);
        }

        public void Render(Graphics g)
        {
            foreach (var boundary in Boundaries)
            {
                boundary.Render(g);
            }
        }
    }
}
