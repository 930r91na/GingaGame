using System.Collections.Generic;
using System.Drawing;

namespace GingaGame;

public class Scene
{
    public List<VElement> Elements { get; } = [];

    public void AddElement(VElement element)
    {
        Elements.Add(element);
    }

    public void Render(Graphics g)
    {
        foreach (var element in Elements)
        {
            element.Render(g);
        }
    }
}