#nullable enable
using System.Drawing;

namespace GingaGame;

public abstract class VElement
{
    public abstract void Update();
    public abstract void Render(Graphics? g);
    public abstract void Constraints();
    
}