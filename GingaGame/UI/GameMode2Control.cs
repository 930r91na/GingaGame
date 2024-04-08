using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using GingaGame.Shared;
using Timer = System.Windows.Forms.Timer;

namespace GingaGame.UI;

public partial class GameMode2Control : UserControl
{
    private const GameMode GameMode = Shared.GameMode.Mode2;
    private readonly Mutex _canvasMutex = new();
    private readonly Timer _fpsTimer = new();
    private readonly Mutex _nextPlanetCanvasMutex = new();
    private readonly Timer _planetSwitchTimer = new();
    private Canvas _canvas;
    private CollisionHandler _collisionHandler;
    private Planet _currentPlanet;
    private int _frameCounter;
    private Planet _nextPlanet;
    private Canvas _nextPlanetCanvas;
    private PlanetFactory _planetFactory;
    private Scene _scene;
    private Score _score;
    private Scoreboard _scoreboard;

    public GameMode2Control()
    {
        InitializeComponent();

        InitializeGameMode2();
    }

    private void InitializeGameMode2()
    {
        // Canvas setup
        _canvas = new Canvas(canvasPictureBox.Size);
        canvasPictureBox.Image = _canvas.Bitmap;

        _nextPlanetCanvas = new Canvas(nextPlanetPictureBox.Size);
        nextPlanetPictureBox.Image = _nextPlanetCanvas.Bitmap;

        var evolutionCanvas = new Canvas(evolutionCyclePictureBox.Size);
        evolutionCyclePictureBox.Image = evolutionCanvas.Bitmap;

        // Score and Scoreboard setup
        _score = new Score();
        _scoreboard = new Scoreboard(GameMode);
        UpdateScoreboardLabel();

        // Scene and game setup
        _scene = new Scene();
        _currentPlanet =
            new Planet(10, 0, 0, _collisionHandler)
            {
                IsPinned = true
            };
        _scene.AddElement(_currentPlanet);
        _planetFactory = new PlanetFactory(GameMode);

        // Timer setup
        _planetSwitchTimer.Interval = 1000; // 1 second interval
        _planetSwitchTimer.Tick += PlanetSwitchTimer_Tick;

        // FPS timer setup
        _fpsTimer.Interval = 1000; // 1 second interval
        _fpsTimer.Tick += FpsTimer_Tick;
        _fpsTimer.Start();

        // Game timer setup
        gameLoopTimer.Interval = 1000 / 144; // 144 FPS
        gameLoopTimer.Tick += gameLoopTimer_Tick;
        gameLoopTimer.Start();

        // Initial next planet setup
        GenerateNextPlanet();

        // Initialize the collision handler
        //_collisionHandler = new CollisionHandler(_scene, _canvas, _collisionHandler, _planetFactory, _score, _container);

        // Initialize the game state handler
        // TODO: Implement the game state handler

        // Render the evolution cycle once
        evolutionCanvas.Graphics?.DrawImage(Resource1.EvolutionCycle, 0, 0, evolutionCanvas.Width,
            evolutionCanvas.Height);
    }

    private void UpdateScoreboardLabel()
    {
        var topScores = _scoreboard.GetTopScores();
        var scoreText = string.Join("\n", topScores.Select(entry => $"{entry.PlayerName}: {entry.Score}"));
        topScoresLabel.Text = scoreText;
    }

    private void GenerateNextPlanet()
    {
        _nextPlanet = _planetFactory.GenerateNextPlanet(_canvas, _collisionHandler);
    }

    private void RenderNextPlanet()
    {
        _nextPlanetCanvasMutex.WaitOne(); // Acquire the lock
        try
        {
            // Draw the next planet texture below the label
            _nextPlanetCanvas.Graphics?.Clear(Color.Transparent); // Clear the canvas

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

    private void gameLoopTimer_Tick(object sender, EventArgs e)
    {
        _frameCounter++;

        _canvasMutex.WaitOne(); // Acquire the lock
        try
        {
            _canvas.Graphics?.Clear(Color.Transparent); // Clear the canvas

            // TODO: Container rendering

            // Update and Constraints Logic in one loop
            foreach (var planet in _scene.Planets)
            {
                planet.Update(); // Apply forces, Verlet integration
                _collisionHandler.CheckConstraints(planet); // Container constraints
            }

            // Call collision detection after updates and before rendering
            _collisionHandler.CheckCollisions();

            // Check game state
            // TODO: Implement the game state handler

            // Check if the score has changed
            if (_score.HasChanged)
            {
                scoreLabel.Text = $@"Score: {_score.CurrentScore}";
                _score.HasChanged = false;
            }

            _scene.Render(_canvas.Graphics); // Now render everything

            RenderNextPlanet();

            canvasPictureBox.Invalidate();
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
        canvasPictureBox.Enabled = true;
    }
}