using System;
using System.Windows.Forms;

namespace GingaGame.UI;

public partial class GingaGameForm : Form
{
    private readonly MainMenuControl _mainMenuControl;
    private GameMode1Control _gameMode1Control;
    private GameMode2Control _gameMode2Control;

    public GingaGameForm()
    {
        InitializeComponent();

        // Create a new instance of MainMenuControl and add it to the content panel
        _mainMenuControl = new MainMenuControl();
        contentPanel.Controls.Add(_mainMenuControl);

        // Subscribe to the Click events of the game mode buttons
        _mainMenuControl.GameMode1Button.Click += GameMode1ButtonOnClick;
        _mainMenuControl.GameMode2Button.Click += GameMode2ButtonOnClick;
    }

    private void GameMode1ButtonOnClick(object sender, EventArgs e)
    {
        // Create a new instance of GameMode1Control and add it to the content panel
        _gameMode1Control = new GameMode1Control();
        contentPanel.Controls.Add(_gameMode1Control);

        // Maximize the window
        WindowState = FormWindowState.Maximized;

        _mainMenuControl.Hide();
    }

    private void GameMode2ButtonOnClick(object sender, EventArgs e)
    {
        // Create a new instance of GameMode2Control and add it to the content panel
        _gameMode2Control = new GameMode2Control();
        contentPanel.Controls.Add(_gameMode2Control);

        // Maximize the window
        WindowState = FormWindowState.Maximized;

        _mainMenuControl.Hide();
    }
}