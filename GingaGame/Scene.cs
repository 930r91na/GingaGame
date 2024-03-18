using System.Collections.Generic;
using System.Drawing;

namespace GingaGame
{
    public class Scene
    {
        public List<VElement> Elements { get; set; } = [];
        
        public void AddElement(VElement element)
        {
            Elements.Add(element);
        }

        public void Render(Graphics g, Size size)
        {
            for (int s = 0; s < Elements.Count; s++)
            {
                Elements[s].Render(g, size);
            }
        }
    }
}
