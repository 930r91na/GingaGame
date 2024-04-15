using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using GingaGame.GameMode1;
using GingaGame.Shared;
using Timer = System.Windows.Forms.Timer;

namespace GingaGame.UI;

public partial class GameMode1Control : UserControl
{
    private const GameMode GameMode = Shared.GameMode.Mode1;
    private readonly Mutex _canvasMutex = new();
    private readonly Timer _fpsTimer = new();
    private readonly Mutex _nextPlanetCanvasMutex = new();
    private readonly Timer _planetSwitchTimer = new();
    private Canvas _canvas;
    private CollisionHandler _collisionHandler;
    private Container _container;
    private Planet _currentPlanet;
    private int _frameCounter;
    private GameStateHandler _gameStateHandler;
    private Planet _nextPlanet;
    private Canvas _nextPlanetCanvas;
    private PlanetFactory _planetFactory;
    private Scene _scene;
    private Score _score;
    private Scoreboard _scoreboard;
    private float _testTextBoxNumber;

    public GameMode1Control()
    {
        InitializeComponent();

        InitializeOriginalGameMode();
    }

    private void InitializeOriginalGameMode()
    {
        // Canvas setup
        _canvas = new Canvas(canvasPictureBox.Size);
        canvasPictureBox.Image = _canvas.Bitmap;

        _nextPlanetCanvas = new Canvas(nextPlanetPictureBox.Size);
        nextPlanetPictureBox.Image = _nextPlanetCanvas.Bitmap;

        var evolutionCanvas = new Canvas(evolutionCyclePictureBox.Size);
        evolutionCyclePictureBox.Image = evolutionCanvas.Bitmap;

        // Container setup
        _container = new Container();
        _container.InitializeGameMode1(_canvas.Width, _canvas.Height);

        // Score and Scoreboard setup
        _score = new Score();
        _scoreboard = new Scoreboard(GameMode);
        UpdateScoreboardLabel();

        // Scene and game setup
        _scene = new Scene();

        _currentPlanet =
            new Planet(0, new Vector2(0, 0))
            {
                IsPinned = true
            };

        _scene.AddPlanet(_currentPlanet);
        _scene.AddContainer(_container);

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
        _collisionHandler =
            new CollisionHandler(_scene, _planetFactory, _score, _container, GameMode);

        // Initialize the game state handler
        _gameStateHandler = new GameStateHandler(_scene, _canvas, _score, _scoreboard, this);

        // Render the evolution cycle once
        evolutionCanvas.Graphics?.DrawImage(Resource1.EvolutionCycle, 0, 0, evolutionCanvas.Width,
            evolutionCanvas.Height);
    }

    private void canvasPictureBox_Resize(object sender, EventArgs e)
    {
        // Recreate the canvas with the new size
        _canvas = new Canvas(canvasPictureBox.Size);

        // Assign the new Bitmap to the PictureBox
        canvasPictureBox.Image = _canvas.Bitmap;

        // Redraw the scene
        _canvas.Graphics?.Clear(Color.Transparent);
        _scene?.Render(_canvas.Graphics);
        _container?.Render(_canvas.Graphics);
        canvasPictureBox.Invalidate();
    }

    public void ResetGame()
    {
        // Reset the form and initialize the game again
        _scene.Clear();
        _score.ResetScore();
        UpdateScoreboardLabel();
        _planetFactory.ResetUnlockedPlanets();
        _currentPlanet = new Planet(0, new Vector2(0, 0))
        {
            IsPinned = true
        };
        _scene.AddPlanet(_currentPlanet);
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

            // Update Logic
            foreach (var planet in _scene.Planets) planet.Update(); // Apply forces, Verlet integration

            const int iterations = 5; // Number of iterations for constraint and collision logic

            // Too many iterations -> more stable, slower and less responsive
            // Too few iterations -> less stable, faster and more responsive
            // 5 iterations is a good balance for this game

            // Constraint and Collision Logic (with iterations)
            for (var i = 0; i < iterations; i++)
            {
                foreach (var planet in
                         _scene.Planets) _collisionHandler.CheckConstraints(planet); // Container constraints
                _collisionHandler.CheckCollisions();
            }

            // Check game state
            _gameStateHandler.CheckGameState();

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

        _scene.AddPlanet(_currentPlanet);

        // Re-enable input after the switch logic is complete
        canvasPictureBox.Enabled = true;
    }

    private void canvasPictureBox_Click(object sender, EventArgs e)
    {
        if (!_currentPlanet.IsPinned) return;

        UpdateCurrentPlanetPosition((MouseEventArgs)e);

        _currentPlanet.IsPinned = false;

        // Disable input immediately
        canvasPictureBox.Enabled = false;

        _planetSwitchTimer.Start(); // Start the timer
    }

    private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_currentPlanet.IsPinned) return;

        UpdateCurrentPlanetPosition(e);
    }

    private void UpdateCurrentPlanetPosition(MouseEventArgs e)
    {
        if (e.X < _container!.TopLeft.X + _currentPlanet.Radius)
        {
            _currentPlanet.Position.X = _container.TopLeft.X + _currentPlanet.Radius;
            _currentPlanet.OldPosition.X = _container.TopLeft.X + _currentPlanet.Radius;
        }
        else if (e.X > _container.BottomRight.X - _currentPlanet.Radius)
        {
            _currentPlanet.Position.X = _container.BottomRight.X - _currentPlanet.Radius;
            _currentPlanet.OldPosition.X = _container.BottomRight.X - _currentPlanet.Radius;
        }
        else
        {
            _currentPlanet.Position.X = e.X;
            _currentPlanet.OldPosition.X = e.X;
        }
    }

    private void testTextBox_TextChanged(object sender, EventArgs e)
    {
        if (float.TryParse(testTextBox.Text, out var number)) _testTextBoxNumber = number;
    }
}