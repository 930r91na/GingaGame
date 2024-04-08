using System.ComponentModel;
using System.Drawing;

namespace GingaGame.UI;

partial class MainMenuControl
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.playButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.logoPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.logoPictureBox.BackgroundImage = global::GingaGame.Resource1.Logo;
            this.logoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.logoPictureBox.Location = new System.Drawing.Point(624, 15);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(300, 300);
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // playButton
            // 
            this.playButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.playButton.BackColor = System.Drawing.Color.FromArgb(0, 130, 130); // Teal
            this.playButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.playButton.Font = new System.Drawing.Font("Franklin Gothic Heavy", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playButton.ForeColor = System.Drawing.Color.Transparent;
            this.playButton.Location = new System.Drawing.Point(656, 359);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(246, 75);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play Game";
            this.playButton.UseVisualStyleBackColor = false;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.exitButton.BackColor = Color.FromArgb(0xC6, 0x42, 0x3D); // Muted Red
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.exitButton.Font = new System.Drawing.Font("Franklin Gothic Heavy", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.ForeColor = System.Drawing.Color.Transparent;
            this.exitButton.Location = new System.Drawing.Point(656, 483);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(246, 75);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // MainMenuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GingaGame.Resource1.Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.logoPictureBox);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "MainMenuControl";
            this.Size = new System.Drawing.Size(1532, 800);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox logoPictureBox;
    public System.Windows.Forms.Button playButton;
    public System.Windows.Forms.Button exitButton;
}