using System;
using System.Linq;
using System.Windows.Forms;

namespace GingaGame;

public partial class MyForm : Form
{
    private readonly Canvas _canvas;
    private readonly Scene _scene;
    
    public MyForm()
    {
        InitializeComponent();
        _canvas = new Canvas(PCT_CANVAS.Size);
        PCT_CANVAS.Image = _canvas.Bitmap;
        var middle = _canvas.Width / 2;
        _scene = new Scene();
        var earth = new VPoint(middle, 200, _canvas, Resource1.Tierra, 1f, 60);
        var moon = new VPoint(middle, 0, _canvas, Resource1.Luna, 0.5f, 30);
        var mercury = new VPoint(middle + 20, 0, _canvas, Resource1.Mercurio, 0.5f, 40);
        var neptune = new VPoint(middle, 250, _canvas, Resource1.Neptuno, 1f, 70);
        _canvas.InitializeContainer();
        _scene.AddElement(earth);
        _scene.AddElement(moon);
        _scene.AddElement(mercury);
        _scene.AddElement(neptune);
    }
    
    private void TIMER_Tick(object sender, EventArgs e)
    {
        _canvas.FastClear();
        _canvas.Container?.Render(_canvas.Graphics); // Container rendering

        // Update and Collision Logic in one loop
        foreach (var planet in _scene.Elements)
        {
            planet.Update(); // Apply forces, Verlet integration
            planet.Constraints(); 
        }

        // Collision Detection and Handling
        foreach (var planet in _scene.Elements) 
        {
            foreach (var otherPlanet in _scene.Elements.Where(otherPlanet => planet != otherPlanet && planet.CollidesWith(otherPlanet)))
            {
                planet.HandleCollision(otherPlanet);
            }
        }

        _scene.Render(_canvas.Graphics); // Now render everything
        PCT_CANVAS.Invalidate();
    }
    
    private void MyForm_SizeChanged(object sender, EventArgs e)
    {
        
    }

    private void MyForm_Load(object sender, EventArgs e)
    {
        
    }

    private void PCT_CANVAS_Click(object sender, EventArgs e)
    {
        
    }
}