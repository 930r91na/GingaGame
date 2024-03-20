using System;
using System.Linq;
using System.Windows.Forms;

namespace GingaGame;

public partial class MyForm : Form
{
    private readonly Canvas _canvas;
    private readonly Canvas _nextPlanetCanvas;
    private readonly Scene _scene;
    private Planet _currentPlanet;
    private Planet _nextPlanet;
    private Score _score;
    private readonly Timer _planetSwitchTimer = new();
    private readonly PlanetFactory _planetFactory;

    public MyForm()
    {
        InitializeComponent();
        _canvas = new Canvas(PCT_CANVAS.Size);
        _canvas.InitializeContainer();
        PCT_CANVAS.Image = _canvas.Bitmap;
        
        _nextPlanetCanvas = new Canvas(nextPlanetPictureBox.Size);
        nextPlanetPictureBox.Image = _nextPlanetCanvas.Bitmap;
        
        _scene = new Scene();
        _score = new Score();
        _currentPlanet =
            new Planet(0, 0, 0, _canvas, new PlanetPropertiesMap(), new PlanetPoints())
            {
                IsPinned = true
            };
        _scene.AddElement(_currentPlanet);
        _planetFactory = new PlanetFactory(new PlanetPropertiesMap(), new PlanetPoints());

        // Timer setup
        _planetSwitchTimer.Interval = 1000; // 1 second interval
        _planetSwitchTimer.Tick += PlanetSwitchTimer_Tick;

        // Initial next planet setup
        GenerateNextPlanet();
    }

    private void GenerateNextPlanet()
    {
        _nextPlanet = _planetFactory.GenerateNextPlanet(_canvas);
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

        RenderNextPlanet();

        PCT_CANVAS.Invalidate();
    }

    private void RenderNextPlanet()
    {
        // Draw the next planet texture below the label
        _nextPlanetCanvas.FastClear();
        _nextPlanetCanvas.Graphics?.DrawImage(_nextPlanet.Texture, 0, 0, _nextPlanetCanvas.Width,
            _nextPlanetCanvas.Height);
        
        nextPlanetPictureBox.Invalidate();
    }

    private void PlanetSwitchTimer_Tick(object sender, EventArgs e)
    {
        _currentPlanet = _nextPlanet;
        _currentPlanet.IsPinned = true; 
        // Reset _currentPlanet position to the top of the container

        GenerateNextPlanet();

        _planetSwitchTimer.Stop(); 
        
        _scene.AddElement(_currentPlanet);

        // Re-enable input after the switch logic is complete
        PCT_CANVAS.Enabled = true;
    }

    private void MyForm_SizeChanged(object sender, EventArgs e)
    {
    }

    private void MyForm_Load(object sender, EventArgs e)
    {
    }

    private void PCT_CANVAS_Click(object sender, EventArgs e)
    {
        if (!_currentPlanet.IsPinned) return;

        _currentPlanet.IsPinned = false;

        // Disable input immediately
        PCT_CANVAS.Enabled = false;

        _planetSwitchTimer.Start(); // Start the timer
    }

    private void PCT_CANVAS_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_currentPlanet.IsPinned) return;
        // Adjust x position of the planet based on the mouse position and the container width
        if (e.X < _canvas.Container!.TopLeft.X + _currentPlanet.Radius)
        {
         _currentPlanet.Position.X = _canvas.Container.TopLeft.X + _currentPlanet.Radius;
         _currentPlanet.OldPosition.X = _canvas.Container.TopLeft.X + _currentPlanet.Radius;
        }
        else if (e.X > _canvas.Container.BottomRight.X - _currentPlanet.Radius)
        {
         _currentPlanet.Position.X = _canvas.Container.BottomRight.X - _currentPlanet.Radius;
         _currentPlanet.OldPosition.X = _canvas.Container.BottomRight.X - _currentPlanet.Radius;
        }
        else
        {
         _currentPlanet.Position.X = e.X;
         _currentPlanet.OldPosition.X = e.X;
        }
    }
}