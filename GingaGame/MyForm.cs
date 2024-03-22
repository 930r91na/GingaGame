using System;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace GingaGame;

public partial class MyForm : Form
{
    private readonly Canvas _canvas;
    private readonly Mutex _canvasMutex = new();
    private readonly CollisionHandler _collisionHandler;
    private readonly Timer _fpsTimer = new();
    private readonly GameStateHandler _gameStateHandler;
    private readonly Canvas _nextPlanetCanvas;
    private readonly Mutex _nextPlanetCanvasMutex = new();
    private readonly PlanetFactory _planetFactory;
    private readonly Timer _planetSwitchTimer = new();
    private readonly Scene _scene;
    private readonly Score _score;
    private Planet _currentPlanet;
    private int _frameCounter;
    private Planet _nextPlanet;

    public MyForm()
    {
        InitializeComponent();

        // Canvas setup
        _canvas = new Canvas(PCT_CANVAS.Size);
        _canvas.InitializeContainer();
        PCT_CANVAS.Image = _canvas.Bitmap;

        _nextPlanetCanvas = new Canvas(nextPlanetPictureBox.Size);
        nextPlanetPictureBox.Image = _nextPlanetCanvas.Bitmap;

        var evolutionCanvas = new Canvas(EvolutionCyclePictureBox.Size);
        EvolutionCyclePictureBox.Image = evolutionCanvas.Bitmap;

        // Scene and game setup
        _scene = new Scene();
        _score = new Score();
        _currentPlanet =
            new Planet(0, 0, 0, _canvas)
            {
                IsPinned = true
            };
        _scene.AddElement(_currentPlanet);
        _planetFactory = new PlanetFactory();

        // Timer setup
        _planetSwitchTimer.Interval = 1000; // 1 second interval
        _planetSwitchTimer.Tick += PlanetSwitchTimer_Tick;

        // FPS timer setup
        _fpsTimer.Interval = 1000; // 1 second interval
        _fpsTimer.Tick += FpsTimer_Tick;
        _fpsTimer.Start();

        // Game timer setup
        TIMER.Interval = 1000 / 144; // 144 FPS
        TIMER.Tick += TIMER_Tick;
        TIMER.Start();

        // Initial next planet setup
        GenerateNextPlanet();

        // Initialize the collision handler
        _collisionHandler = new CollisionHandler(_scene, _canvas, _planetFactory, _score);

        // Initialize the game state handler
        _gameStateHandler = new GameStateHandler(_scene, _canvas, this);

        // Render the evolution cycle once
        evolutionCanvas.Graphics?.DrawImage(Resource1.EvolutionCycle, 0, 0, evolutionCanvas.Width,
            evolutionCanvas.Height);
    }

    private void GenerateNextPlanet()
    {
        _nextPlanet = _planetFactory.GenerateNextPlanet(_canvas);
    }

    private void TIMER_Tick(object sender, EventArgs e)
    {
        _frameCounter++;

        _canvasMutex.WaitOne(); // Acquire the lock
        try
        {
            _canvas.FastClear();
            _canvas.Container?.Render(_canvas.Graphics); // Container rendering

            // Update and Constraints Logic in one loop
            foreach (var planet in _scene.Planets)
            {
                planet.Update(); // Apply forces, Verlet integration
                planet.Constraints(); // Wall and container constraints
            }

            // Call collision detection after updates and before rendering
            _collisionHandler.CheckCollisions();

            // Check game state
            _gameStateHandler.CheckGameState();

            // Check if the score has changed
            if (_score.HasChanged)
            {
                scoreLabel.Text = $@"SCORE: {_score.CurrentScore}";
                _score.HasChanged = false;
            }

            _scene.Render(_canvas.Graphics); // Now render everything

            RenderNextPlanet();

            PCT_CANVAS.Invalidate();
        }
        finally
        {
            _canvasMutex.ReleaseMutex(); // Release the lock
        }
    }

    private void FpsTimer_Tick(object sender, EventArgs e)
    {
        fpsLabel.Text = $@"FPS: {_frameCounter}";
        _frameCounter = 0;
    }

    private void RenderNextPlanet()
    {
        _nextPlanetCanvasMutex.WaitOne(); // Acquire the lock
        try
        {
            // Draw the next planet texture below the label
            _nextPlanetCanvas.FastClear();

            var texture = PlanetTextures.GetCachedTexture(_nextPlanet.PlanetType);

            // Draw the next planet texture in the center of the canvas with the correct size
            var imageWidth = _nextPlanet.Radius * 2;
            var imageHeight = _nextPlanet.Radius * 2;
            var middleX = _nextPlanetCanvas.Width / 2 - imageWidth / 2;
            var middleY = (float)_nextPlanetCanvas.Height / 2 - imageHeight / 2;

            _nextPlanetCanvas.Graphics?.DrawImage(texture, middleX, middleY, imageWidth, imageHeight);

            nextPlanetPictureBox.Invalidate();
        }
        finally
        {
            _nextPlanetCanvasMutex.ReleaseMutex(); // Release the lock
        }
    }

    private void PlanetSwitchTimer_Tick(object sender, EventArgs e)
    {
        // Switch the current planet with the next planet
        _currentPlanet = _nextPlanet;
        _currentPlanet.IsPinned = true;

        // Generate a new next planet
        GenerateNextPlanet();

        _planetSwitchTimer.Stop();

        _scene.AddElement(_currentPlanet);

        // Re-enable input after the switch logic is complete
        PCT_CANVAS.Enabled = true;
    }

    private void MyForm_Load(object sender, EventArgs e)
    {
    }

    public void ResetGame()
    {
        // Reset the form and initialize the game again
        _scene.Clear();
        _score.ResetScore();
        _planetFactory.ResetUnlockedPlanets();
        _currentPlanet = new Planet(0, 0, 0, _canvas)
        {
            IsPinned = true
        };
        _scene.AddElement(_currentPlanet);
        GenerateNextPlanet();
    }

    private void PCT_CANVAS_Click(object sender, EventArgs e)
    {
        if (!_currentPlanet.IsPinned) return;

        UpdateCurrentPlanetPosition((MouseEventArgs)e);

        _currentPlanet.IsPinned = false;

        // Disable input immediately
        PCT_CANVAS.Enabled = false;

        _planetSwitchTimer.Start(); // Start the timer
    }

    private void PCT_CANVAS_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_currentPlanet.IsPinned) return;

        UpdateCurrentPlanetPosition(e);
    }

    private void UpdateCurrentPlanetPosition(MouseEventArgs e)
    {
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