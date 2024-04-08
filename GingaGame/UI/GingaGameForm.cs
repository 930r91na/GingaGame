using System;
using System.Drawing;
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
        
        _mainMenuControl = new MainMenuControl();
        contentPanel.Controls.Add(_mainMenuControl);
        
        _mainMenuControl.playButton.Click += PlayButtonOnClick;
        _mainMenuControl.exitButton.Click += ExitButtonOnClick;
    }
    
    private void PlayButtonOnClick(object sender, EventArgs e)
    {
        // Hide both the play and exit buttons
        _mainMenuControl.playButton.Hide();
        _mainMenuControl.exitButton.Hide();
        
        // Create two new buttons
        var gameMode1Button = new Button();
        gameMode1Button.Text = @"Game Mode 1";
        
        var gameMode2Button = new Button();
        gameMode2Button.Text = @"Game Mode 2";
        
        // Copy the styles of the play and exit buttons to the new buttons
        CopyMouseStyles(_mainMenuControl.playButton, gameMode1Button);
        CopyMouseStyles(_mainMenuControl.exitButton, gameMode2Button);
        
        // Add the two new buttons to the form
        _mainMenuControl.Controls.Add(gameMode1Button);
        _mainMenuControl.Controls.Add(gameMode2Button);
        
        // Add event handlers to the new buttons
        gameMode1Button.Click += GameMode1ButtonOnClick;
        gameMode2Button.Click += GameMode2ButtonOnClick;
    }
    
    private static void CopyMouseStyles(ButtonBase source, ButtonBase target)
    {
        target.Anchor = source.Anchor;
        target.BackColor = source.BackColor;
        target.FlatAppearance.MouseOverBackColor = source.FlatAppearance.MouseOverBackColor;
        target.FlatAppearance.MouseDownBackColor = source.FlatAppearance.MouseDownBackColor;
        target.FlatStyle = source.FlatStyle;
        target.Font = source.Font;
        target.ForeColor = source.ForeColor;
        target.Location = source.Location;
        target.Size = source.Size;
        target.UseVisualStyleBackColor = source.UseVisualStyleBackColor;
    }
    
    private void GameMode1ButtonOnClick(object sender, EventArgs e)
    {
        _gameMode1Control = new GameMode1Control();
        contentPanel.Controls.Add(_gameMode1Control);
        
        Location = new Point(Location.X - 100, Location.Y - 150);
        Size = new Size(Size.Width + 400, Size.Height + 250);
        
        _mainMenuControl.Hide();
    }
    
    private void GameMode2ButtonOnClick(object sender, EventArgs e)
    {
        _gameMode2Control = new GameMode2Control();
        contentPanel.Controls.Add(_gameMode2Control);
        
        _mainMenuControl.Hide();
    }
    
    private static void ExitButtonOnClick(object sender, EventArgs e)
    {
        Application.Exit();
    }
}