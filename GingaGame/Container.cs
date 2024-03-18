using System.Collections.Generic;
using System.Drawing;

namespace GingaGame;

internal class Container
{
    public Container(VPoint topLeft, VPoint topRight, VPoint bottomLeft, VPoint bottomRight)
    {
        Boundaries = [];

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

    private List<VPole> Boundaries { get; }

    public void Render(Graphics g)
    {
        foreach (var boundary in Boundaries) boundary.Render(g);
    }
}