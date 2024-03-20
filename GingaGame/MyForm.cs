using System;
using System.Linq;
using System.Windows.Forms;

namespace GingaGame;

public partial class MyForm : Form
{
    private readonly Canvas _canvas;
    private readonly Scene _scene;
    private Score _score;
    private Planet _currentPlanet;
    private Planet _nextPlanet;
    
    public MyForm()
    {
        InitializeComponent();
        _canvas = new Canvas(PCT_CANVAS.Size);
        _canvas.InitializeContainer();
        PCT_CANVAS.Image = _canvas.Bitmap;
        _scene = new Scene();
        _score = new Score();
        _currentPlanet = _nextPlanet =
            new Planet(0, 0, 0, _canvas, new PlanetPropertiesMap(), new PlanetPoints())
            {
                IsPinned = true
            };
        _scene.AddElement(_currentPlanet);
    }

    private void TIMER_Tick(object sender, EventArgs e)
    {
        _canvas.FastClear();
        _canvas.Container?.Render(_canvas.Graphics); // Container rendering

        // Update and Constraints Logic in one loop
        foreach (var planet in _scene.Elements)
        {
            planet.Update(); // Apply forces, Verlet integration
            planet.Constraints();
        }

        // Collision Detection and Handling
        foreach (var planet in _scene.Elements)
        foreach (var otherPlanet in _scene.Elements.Where(otherPlanet =>
                     planet != otherPlanet && planet.CollidesWith(otherPlanet)))
            planet.HandleCollision(otherPlanet);

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
    
    private void PCT_CANVAS_MouseMove(object sender, MouseEventArgs e)
    {
    }
}