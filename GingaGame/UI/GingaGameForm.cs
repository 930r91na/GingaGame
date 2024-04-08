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
        
        _mainMenuControl.GameMode1Button.Click += GameMode1ButtonOnClick;
        _mainMenuControl.GameMode2Button.Click += GameMode2ButtonOnClick;
    }
    
    private void GameMode1ButtonOnClick(object sender, EventArgs e)
    {
        _gameMode1Control = new GameMode1Control();
        contentPanel.Controls.Add(_gameMode1Control);
        
        Location = new Point(Location.X - 100, Location.Y - 100);
        Size = new Size(Size.Width + 400, Size.Height + 250);
        
        _mainMenuControl.Hide();
    }
    
    private void GameMode2ButtonOnClick(object sender, EventArgs e)
    {
        _gameMode2Control = new GameMode2Control();
        contentPanel.Controls.Add(_gameMode2Control);
        
        _mainMenuControl.Hide();
    }
}