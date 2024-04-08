using System;
using System.Drawing;
using System.Windows.Forms;

namespace GingaGame.UI;

public partial class MainMenuControl : UserControl
{
    public readonly Button GameMode1Button = new();
    public readonly Button GameMode2Button = new();
    public MainMenuControl()
    {
        InitializeComponent();
    }
    
    private void playButton_Click(object sender, EventArgs e)
    {
        // Hide both the play and exit buttons
        playButton.Hide();
        exitButton.Hide();
        
        InitializeGameModeButtons();

        // Add the two game mode buttons to the form
        Controls.Add(GameMode1Button);
        Controls.Add(GameMode2Button);
    }

    private void InitializeGameModeButtons()
    {
        // Set the text of the game mode buttons
        GameMode1Button.Text = @"Game Mode 1";
        GameMode2Button.Text = @"Game Mode 2";
        
        // Copy the styles of the play and exit buttons to the game mode buttons
        CopyButton(playButton, GameMode1Button);
        CopyButton(exitButton, GameMode2Button);
        
        // Change the colors of the game mode buttons
        GameMode1Button.BackColor = Color.FromArgb(20, 70, 110); // Gray
        GameMode2Button.BackColor = Color.FromArgb(50, 50, 150); // Gray
    }

    private static void CopyButton(ButtonBase source, ButtonBase target)
    {
        target.Anchor = source.Anchor;
        target.BackColor = source.BackColor;
        target.FlatStyle = source.FlatStyle;
        target.Font = source.Font;
        target.ForeColor = source.ForeColor;
        target.Location = source.Location;
        target.Size = source.Size;
        target.UseVisualStyleBackColor = source.UseVisualStyleBackColor;
    }
    
    private void exitButton_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
}
