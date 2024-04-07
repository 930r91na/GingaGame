using System;
using System.Drawing;
using System.Linq;
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
    private readonly Scoreboard _scoreboard = new();
    private Planet _currentPlanet;
    private int _frameCounter;
    private Planet _nextPlanet;
    private Camera _camera;
    private Map _map;
    private Layer _background;
    public MyForm()
    {
        InitializeComponent();

        // Camaera, Map, Layer, Scene setup
        Map.K = 5;
        Map.Unit = 32;
        int div = 4;
        _map = new Map();
        _camera = new Camera();
        _background = new Layer(_map.BMP, new Size(PCT_CANVAS.Width / div, PCT_CANVAS.Height / div), new Size(PCT_CANVAS.Width, PCT_CANVAS.Height));
        _scene = new Scene(_camera, _background, _map);

        // Canvas setup
        _canvas = new Canvas(_scene, PCT_CANVAS);
        _canvas.InitializeContainer();
        PCT_CANVAS.Image = _canvas.Bitmap;

        _nextPlanetCanvas = new Canvas(null,nextPlanetPictureBox);
        nextPlanetPictureBox.Image = _nextPlanetCanvas.Bitmap;

        var evolutionCanvas = new Canvas(null, EvolutionCyclePictureBox);
        EvolutionCyclePictureBox.Image = evolutionCanvas.Bitmap;

        // Scoreboard setup
        UpdateScoreboardLabel();

        // Scene and game setup
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
        _gameStateHandler = new GameStateHandler(_scene, _canvas, _score, _scoreboard, this);

        // Render the evolution cycle once
        evolutionCanvas.g?.DrawImage(Resource1.EvolutionCycle, 0, 0, evolutionCanvas.Width,
            evolutionCanvas.Height);
    }

    public void ResetGame()
    {
        // Reset the form and initialize the game again
        _scene.Clear();
        _score.ResetScore();
        UpdateScoreboardLabel();
        _planetFactory.ResetUnlockedPlanets();
        _currentPlanet = new Planet(0, 0, 0, _canvas)
        {
            IsPinned = true
        };
        _scene.AddElement(_currentPlanet);
        GenerateNextPlanet();
    }

    private void UpdateScoreboardLabel()
    {
        var topScores = _scoreboard.GetTopScores();
        var scoreText = string.Join("\n", topScores.Select(entry => $"{entry.PlayerName}: {entry.Score}"));
        topScoresLabel.Text = scoreText;
    }

    private void GenerateNextPlanet()
    {
        _nextPlanet = _planetFactory.GenerateNextPlanet(_canvas);
    }

    private void RenderNextPlanet()
    {
        _nextPlanetCanvasMutex.WaitOne(); // Acquire the lock
        try
        {
            // Draw the next planet texture below the label
            _nextPlanetCanvas.g?.Clear(Color.Transparent); // Clear the canvas

            var texture = PlanetTextures.GetCachedTexture(_nextPlanet.PlanetType);

            // Draw the next planet texture in the center of the canvas with the correct size
            var imageWidth = _nextPlanet.Radius * 2;
            var imageHeight = _nextPlanet.Radius * 2;
            var middleX = _nextPlanetCanvas.Width / 2 - imageWidth / 2;
            var middleY = (float)_nextPlanetCanvas.Height / 2 - imageHeight / 2;

            _nextPlanetCanvas.g?.DrawImage(texture, middleX, middleY, imageWidth, imageHeight);

            nextPlanetPictureBox.Invalidate();
        }
        finally
        {
            _nextPlanetCanvasMutex.ReleaseMutex(); // Release the lock
        }
    }

    private void TIMER_Tick(object sender, EventArgs e)
    {
        _frameCounter++;

        _canvasMutex.WaitOne(); // Acquire the lock
        try
        {
            _canvas.g?.Clear(Color.Transparent); // Clear the canvas
            _canvas.Container?.Render(_canvas.g); // Container rendering

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
                scoreLabel.Text = $@"Score: {_score.CurrentScore}";
                _score.HasChanged = false;
            }

            _scene.Render(_canvas.g); // Now render everything

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