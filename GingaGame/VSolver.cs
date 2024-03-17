using System.Collections.Generic;

namespace GingaGame
{
    public class VSolver
    {
        public List<VElement> Elements { get; private set; }

        public VSolver() => Elements = new List<VElement>();

        public void AddElement(VElement element) => Elements.Add(element);

        public void Update(float deltaTime)
        {
            foreach (var element in Elements) element.Update(deltaTime);
        }
    }
}

