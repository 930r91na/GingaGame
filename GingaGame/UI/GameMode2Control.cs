using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using GingaGame.Shared;
using Timer = System.Windows.Forms.Timer;

namespace GingaGame.UI;

public partial class GameMode2Control : UserControl
{
    private const GameMode GameMode = Shared.GameMode.Mode2;
    private const float ParallaxBackgroundFactor = 0.1f;
    private readonly Mutex _canvasMutex = new();
    private readonly Timer _fpsTimer = new();
    private readonly Mutex _nextPlanetCanvasMutex = new();
    private readonly Timer _planetSwitchTimer = new();
    private Image _backgroundImage;
    private int _backgroundYOffset;
    private Canvas _canvas;
    private CollisionHandler _collisionHandler;
    private Container _container;
    private int _currentFloorIndex;
    private Planet _currentPlanet;
    private Planet _currentPlanetToDrop;
    private bool _followPlanet;
    private int _frameCounter;
    private bool _gameOverTriggered;
    private bool _gameWonTriggered;
    private int _horizontalMargin;
    private Canvas _nextPlanetCanvas;
    private Planet _nextPlanetToDrop;
    private int _numberOfFloors = 4;
    private PlanetFactory _planetFactory;
    private List<int> _planetsPerFloor = [];
    private Scene _scene;
    private Score _score;
    private int _scrollOffset;
    private int _verticalMargin = 70;

    public GameMode2Control()
    {
        InitializeComponent();

        InitializeGameMode2();
    }

    private int FloorHeight => canvasPictureBox.Height - 2 * _verticalMargin;

    private void InitializeGameMode2()
    {
        // Canvas setup
        _canvas = new Canvas(canvasPictureBox.Size);
        canvasPictureBox.Image = _canvas.Bitmap;

        _nextPlanetCanvas = new Canvas(nextPlanetPictureBox.Size);
        nextPlanetPictureBox.Image = _nextPlanetCanvas.Bitmap;

        var evolutionCanvas = new Canvas(evolutionCyclePictureBox.Size);
        evolutionCyclePictureBox.Image = evolutionCanvas.Bitmap;

        // Score setup
        _score = new Score();

        // Scene and game setup
        _scene = new Scene();

        _currentPlanetToDrop =
            new Planet(10, new Vector2(0, 0))
            {
                IsPinned = true
            };

        _scene.AddPlanet(_currentPlanetToDrop);

        // Load game mode 2 map
        LoadGameMode2Map();

        _scene.InitializeFloors(FloorHeight, _verticalMargin, _numberOfFloors, _planetsPerFloor);

        // Calculate container size
        var canvasWidth = canvasPictureBox.Width;
        var containerHeight = _scene.Floors.Count * FloorHeight + _verticalMargin;

        // Initialize the container
        _container = new Container();
        _container.InitializeGameMode2(canvasWidth, containerHeight, _verticalMargin, _horizontalMargin);

        _scene.AddContainer(_container);

        // Planet factory setup
        _planetFactory = new PlanetFactory(GameMode);

        // Background setup
        _backgroundImage = Resource1.ScrollerBackground1;

        // Timers setup
        // Planet switch timer setup
        _planetSwitchTimer.Interval = 500; // 0.5 second interval
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
        _collisionHandler = new CollisionHandler(_scene, _planetFactory, _score, _container, GameMode, this);

        // Render the evolution cycle once
        evolutionCanvas.Graphics?.DrawImage(Resource1.EvolutionCycle, 0, 0, evolutionCanvas.Width,
            evolutionCanvas.Height);
    }

    private void LoadGameMode2Map()
    {
        var projectDirectory =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));

        var mapContent = File.ReadAllText(Path.Combine(projectDirectory, "GingaGame/Resources/GameMode2Map.txt"));
        var lines = mapContent.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var keyValue = line.Split('=');
            var key = keyValue[0];
            var value = keyValue[1];

            switch (key)
            {
                case "VerticalMargin":
                    _verticalMargin = int.Parse(value);
                    break;
                case "HorizontalMargin":
                    _horizontalMargin = int.Parse(value);
                    break;
                case "NumberOfFloors":
                    var floorData = value.Split(["->"], StringSplitOptions.None);
                    _numberOfFloors = int.Parse(floorData[0]);
                    _planetsPerFloor = floorData[1].Split(',').Select(int.Parse).ToList();
                    break;
            }
        }

        // Validation
        if (_numberOfFloors is < 2 or > 10)
            throw new Exception("Invalid number of floors. It should be between 2 and 10.");

        if (_planetsPerFloor.Count != _numberOfFloors)
            throw new Exception("The number of floors should match the number of planets per floor.");

        if (_planetsPerFloor.Sum() != 11)
            throw new Exception("The sum of the number of planets for each floor should be 11.");

        if (_planetsPerFloor.Last() != 1) throw new Exception("The last number should always be 1.");

        if (_planetsPerFloor.Any(p => p is 0))
            throw new Exception("The number of planets per floor should be greater than 0.");
    }

    private void canvasPictureBox_Resize(object sender, EventArgs e)
    {
        // Recreate the canvas with the new size
        _canvas = new Canvas(canvasPictureBox.Size);

        // Assign the new Bitmap to the PictureBox
        canvasPictureBox.Image = _canvas.Bitmap;

        // Redraw the scene
        _canvas.Graphics?.Clear(Color.Transparent);
        _scene?.Render(_canvas.Graphics, canvasPictureBox.Height);
        _container?.Render(_canvas.Graphics);
        canvasPictureBox.Invalidate();
    }

    private void GenerateNextPlanet()
    {
        _nextPlanetToDrop = _planetFactory.GenerateNextPlanet(_canvas);
    }

    private void ResetGame()
    {
        // Reset the form and initialize the game again
        _scene.Clear();
        _score.ResetScore();
        _planetFactory.ResetUnlockedPlanets();
        _currentPlanetToDrop = new Planet(10, new Vector2(0, 0))
        {
            IsPinned = true
        };
        _scene.AddPlanet(_currentPlanetToDrop);
        GenerateNextPlanet();
    }

    private void RenderNextPlanet()
    {
        _nextPlanetCanvasMutex.WaitOne(); // Acquire the lock
        try
        {
            // Draw the next planet texture below the label
            _nextPlanetCanvas.Graphics?.Clear(Color.Transparent); // Clear the canvas

            var texture = PlanetTextures.GetCachedTexture(_nextPlanetToDrop.PlanetType);

            // Draw the next planet texture in the center of the canvas with the correct size
            var imageWidth = _nextPlanetToDrop.Radius * 2;
            var imageHeight = _nextPlanetToDrop.Radius * 2;
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
            if (_collisionHandler.IsGameOver() && !_gameOverTriggered)
            {
                _gameOverTriggered = true;
                MessageBox.Show(@"Game Over! You lost!", @"Game Over", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetGame();
            }

            // Scrolling Logic
            CalculateScrollOffset();

            // Render Logic

            // Parallax Background and Foreground Rendering
            _backgroundYOffset = (int)(_scrollOffset * ParallaxBackgroundFactor);

            // Calculate how many background image repetitions are needed to cover the viewable area
            var backgroundRepetitions =
                (int)Math.Ceiling(
                    (canvasPictureBox.Height + 2 * _verticalMargin + _backgroundYOffset) /
                    (float)_backgroundImage.Height);

            // Draw the background image multiple times, offsetting it vertically for each repetition
            for (var i = 0; i < backgroundRepetitions; i++)
            {
                var yPosition = -_backgroundYOffset + i * _backgroundImage.Height;
                _canvas.Graphics?.DrawImage(_backgroundImage, 0, yPosition);
            }

            // Render the scene
            _scene.Render(_canvas.Graphics, canvasPictureBox.Height, _scrollOffset);

            RenderNextPlanet();

            canvasPictureBox.Invalidate();
        }
        finally
        {
            _canvasMutex.ReleaseMutex(); // Release the lock
        }
    }

    private void CalculateScrollOffset()
    {
        if (_currentPlanet == null) return;

        // Find the current floor
        var currentFloor = _scene.Floors.FirstOrDefault(f =>
            f.StartPositionY <= _currentPlanet.Position.Y && _currentPlanet.Position.Y <= f.EndPositionY);

        if (currentFloor == null) return; // Planet is outside the floor range

        // Update the current floor index
        _currentFloorIndex = currentFloor.Index;

        // Calculate the scroll offset based on the current floor
        _scrollOffset = FloorHeight * _currentFloorIndex;

        // Gradually adjust the offset for a smooth transition when following the planet
        if (_followPlanet) _scrollOffset += (int)(_currentPlanet.Position.Y - currentFloor.StartPositionY);
    }

    public Planet GetCurrentPlanet()
    {
        return _currentPlanet;
    }

    public void SetCurrentPlanet(Planet planet)
    {
        _currentPlanet = planet;
    }

    private void FpsTimer_Tick(object sender, EventArgs e)
    {
        fpsLabel.Text = $@"FPS: {_frameCounter}";
        _frameCounter = 0;
    }

    private void PlanetSwitchTimer_Tick(object sender, EventArgs e)
    {
        // Switch the current planet with the next planet
        _currentPlanetToDrop = _nextPlanetToDrop;
        _currentPlanetToDrop.IsPinned = true;

        // Generate a new next planet
        GenerateNextPlanet();

        _planetSwitchTimer.Stop();

        _scene.AddPlanet(_currentPlanetToDrop);

        // Re-enable input after the switch logic is complete
        canvasPictureBox.Enabled = true;
    }

    private void canvasPictureBox_Click(object sender, EventArgs e)
    {
        if (!_currentPlanetToDrop.IsPinned) return;

        // First go back to the top if not already there
        if (_scrollOffset > 0)
        {
            _scrollOffset = 0;
            DeselectCurrentPlanet();
        }
        else
        {
            // Drop the current planet
            _currentPlanetToDrop.IsPinned = false;
            _currentPlanet = _currentPlanetToDrop;

            // Disable input immediately
            canvasPictureBox.Enabled = false;

            _planetSwitchTimer.Start(); // Start the timer
        }
    }

    private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_currentPlanetToDrop.IsPinned) return;

        UpdateCurrentPlanetPosition(e);
    }

    private void UpdateCurrentPlanetPosition(MouseEventArgs e)
    {
        if (e.X < _container!.TopLeft.X + _currentPlanetToDrop.Radius)
        {
            _currentPlanetToDrop.Position.X = _container.TopLeft.X + _currentPlanetToDrop.Radius;
            _currentPlanetToDrop.OldPosition.X = _container.TopLeft.X + _currentPlanetToDrop.Radius;
        }
        else if (e.X > _container.BottomRight.X - _currentPlanetToDrop.Radius)
        {
            _currentPlanetToDrop.Position.X = _container.BottomRight.X - _currentPlanetToDrop.Radius;
            _currentPlanetToDrop.OldPosition.X = _container.BottomRight.X - _currentPlanetToDrop.Radius;
        }
        else
        {
            _currentPlanetToDrop.Position.X = e.X;
            _currentPlanetToDrop.OldPosition.X = e.X;
        }
    }

    public void GameWon()
    {
        if (_gameWonTriggered) return;

        // Prevent multiple triggers
        _gameWonTriggered = true;
        MessageBox.Show(@"Congratulations! You won!", @"Game won", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void followPlanetCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        _followPlanet = followPlanetCheckBox.Checked;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        const int scrollSpeed = 35;
        switch (keyData)
        {
            case Keys.Left:
                _currentPlanetToDrop.Position.X -= 15;
                break;
            case Keys.Right:
                _currentPlanetToDrop.Position.X += 15;
                break;
            case Keys.Up:
                if (_scrollOffset >= scrollSpeed)
                {
                    _scrollOffset -= scrollSpeed;
                    DeselectCurrentPlanet();
                }

                break;
            case Keys.Down:
                if (_scrollOffset < (_scene.Floors.Count - 1) * FloorHeight - _verticalMargin +
                    scrollSpeed)
                {
                    _scrollOffset += scrollSpeed;
                    DeselectCurrentPlanet();
                }

                break;
            case Keys.Back:
                // Reset scroll offset
                _scrollOffset = 0;
                DeselectCurrentPlanet();
                break;
            case Keys.Enter:
                if (!_currentPlanetToDrop.IsPinned) return false;
                // First go back to the top if not already there
                if (_scrollOffset > 0)
                {
                    _scrollOffset = 0;
                    DeselectCurrentPlanet();
                }
                else
                {
                    // Drop the current planet
                    _currentPlanetToDrop.IsPinned = false;
                    _currentPlanet = _currentPlanetToDrop;

                    // Disable input immediately
                    canvasPictureBox.Enabled = false;

                    _planetSwitchTimer.Start(); // Start the timer
                }

                break;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void canvasPictureBox_MouseWheel(object sender, MouseEventArgs e)
    {
        const int scrollSpeed = 35;
        if (e.Delta > 0)
        {
            // Scroll up
            if (_scrollOffset < scrollSpeed) return;
            _scrollOffset -= scrollSpeed;
            DeselectCurrentPlanet();
        }
        else
        {
            // Scroll down
            if (_scrollOffset > (_scene.Floors.Count - 1) * FloorHeight - _verticalMargin +
                scrollSpeed) return;
            _scrollOffset += scrollSpeed;
            DeselectCurrentPlanet();
        }
    }

    private void DeselectCurrentPlanet()
    {
        _currentPlanet = null;
    }
}