using System.ComponentModel;

namespace GingaGame.UI;

partial class GameMode2Control
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
        this.components = new System.ComponentModel.Container();
        this.evolutionCycleLabel = new System.Windows.Forms.Label();
        this.fpsLabel = new System.Windows.Forms.Label();
        this.nextPlanetLabel = new System.Windows.Forms.Label();
        this.scoreLabel = new System.Windows.Forms.Label();
        this.scoreboardLabel = new System.Windows.Forms.Label();
        this.topScoresLabel = new System.Windows.Forms.Label();
        this.canvasPictureBox = new System.Windows.Forms.PictureBox();
        this.evolutionCyclePictureBox = new System.Windows.Forms.PictureBox();
        this.nextPlanetPictureBox = new System.Windows.Forms.PictureBox();
        this.gameLoopTimer = new System.Windows.Forms.Timer(this.components);
        ((System.ComponentModel.ISupportInitialize)(this.canvasPictureBox)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.evolutionCyclePictureBox)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nextPlanetPictureBox)).BeginInit();
        this.SuspendLayout();
        // 
        // evolutionCycleLabel
        // 
        this.evolutionCycleLabel.AutoSize = true;
        this.evolutionCycleLabel.BackColor = System.Drawing.Color.Transparent;
        this.evolutionCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.evolutionCycleLabel.Font = new System.Drawing.Font("Gadugi", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.evolutionCycleLabel.ForeColor = System.Drawing.Color.Gainsboro;
        this.evolutionCycleLabel.Location = new System.Drawing.Point(12, 504);
        this.evolutionCycleLabel.Name = "evolutionCycleLabel";
        this.evolutionCycleLabel.Size = new System.Drawing.Size(183, 27);
        this.evolutionCycleLabel.TabIndex = 11;
        this.evolutionCycleLabel.Text = "Evolution Cycle:";
        this.evolutionCycleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // fpsLabel
        // 
        this.fpsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.fpsLabel.AutoSize = true;
        this.fpsLabel.BackColor = System.Drawing.Color.Transparent;
        this.fpsLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.fpsLabel.Font = new System.Drawing.Font("Gadugi", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.fpsLabel.ForeColor = System.Drawing.Color.CornflowerBlue;
        this.fpsLabel.Location = new System.Drawing.Point(5, 814);
        this.fpsLabel.Name = "fpsLabel";
        this.fpsLabel.Size = new System.Drawing.Size(42, 19);
        this.fpsLabel.TabIndex = 10;
        this.fpsLabel.Text = "FPS: ";
        this.fpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // nextPlanetLabel
        // 
        this.nextPlanetLabel.AutoSize = true;
        this.nextPlanetLabel.BackColor = System.Drawing.Color.Transparent;
        this.nextPlanetLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.nextPlanetLabel.Font = new System.Drawing.Font("Gadugi", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.nextPlanetLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
        this.nextPlanetLabel.Location = new System.Drawing.Point(12, 13);
        this.nextPlanetLabel.Name = "nextPlanetLabel";
        this.nextPlanetLabel.Size = new System.Drawing.Size(144, 27);
        this.nextPlanetLabel.TabIndex = 7;
        this.nextPlanetLabel.Text = "Next Planet:";
        this.nextPlanetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // scoreboardLabel
        // 
        this.scoreboardLabel.AutoSize = true;
        this.scoreboardLabel.BackColor = System.Drawing.Color.Transparent;
        this.scoreboardLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.scoreboardLabel.Font = new System.Drawing.Font("Gadugi", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.scoreboardLabel.ForeColor = System.Drawing.Color.Gainsboro;
        this.scoreboardLabel.Location = new System.Drawing.Point(12, 273);
        this.scoreboardLabel.Name = "scoreboardLabel";
        this.scoreboardLabel.Size = new System.Drawing.Size(134, 27);
        this.scoreboardLabel.TabIndex = 14;
        this.scoreboardLabel.Text = "Top Scores:";
        this.scoreboardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // scoreLabel
        // 
        this.scoreLabel.AutoSize = true;
        this.scoreLabel.BackColor = System.Drawing.Color.Transparent;
        this.scoreLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.scoreLabel.Font = new System.Drawing.Font("Gadugi", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.scoreLabel.ForeColor = System.Drawing.Color.CornflowerBlue;
        this.scoreLabel.Location = new System.Drawing.Point(12, 221);
        this.scoreLabel.Name = "scoreLabel";
        this.scoreLabel.Size = new System.Drawing.Size(96, 27);
        this.scoreLabel.TabIndex = 8;
        this.scoreLabel.Text = "Score: 0";
        this.scoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // topScoresLabel
        // 
        this.topScoresLabel.AutoSize = true;
        this.topScoresLabel.BackColor = System.Drawing.Color.Transparent;
        this.topScoresLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.topScoresLabel.Font = new System.Drawing.Font("Gadugi", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.topScoresLabel.ForeColor = System.Drawing.Color.CornflowerBlue;
        this.topScoresLabel.Location = new System.Drawing.Point(12, 315);
        this.topScoresLabel.Name = "topScoresLabel";
        this.topScoresLabel.Size = new System.Drawing.Size(0, 27);
        this.topScoresLabel.TabIndex = 13;
        this.topScoresLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // canvasPictureBox
        // 
        this.canvasPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.canvasPictureBox.BackColor = System.Drawing.Color.Transparent;
        this.canvasPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.canvasPictureBox.Location = new System.Drawing.Point(280, 13);
        this.canvasPictureBox.Name = "canvasPictureBox";
        this.canvasPictureBox.Size = new System.Drawing.Size(1247, 780);
        this.canvasPictureBox.TabIndex = 0;
        this.canvasPictureBox.TabStop = false;
        this.canvasPictureBox.Click += new System.EventHandler(this.canvasPictureBox_Click);
        this.canvasPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasPictureBox_MouseMove);
        this.canvasPictureBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.canvasPictureBox_MouseWheel);
        this.canvasPictureBox.Resize += new System.EventHandler(this.canvasPictureBox_Resize);
        // 
        // EvolutionCyclePictureBox
        // 
        this.evolutionCyclePictureBox.BackColor = System.Drawing.Color.Transparent;
        this.evolutionCyclePictureBox.Location = new System.Drawing.Point(21, 547);
        this.evolutionCyclePictureBox.Name = "evolutionCyclePictureBox";
        this.evolutionCyclePictureBox.Size = new System.Drawing.Size(240, 240);
        this.evolutionCyclePictureBox.TabIndex = 12;
        this.evolutionCyclePictureBox.TabStop = false;
        // 
        // nextPlanetPictureBox
        // 
        this.nextPlanetPictureBox.BackColor = System.Drawing.Color.Transparent;
        this.nextPlanetPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.nextPlanetPictureBox.Location = new System.Drawing.Point(80, 60);
        this.nextPlanetPictureBox.Name = "nextPlanetPictureBox";
        this.nextPlanetPictureBox.Size = new System.Drawing.Size(130, 130);
        this.nextPlanetPictureBox.TabIndex = 9;
        this.nextPlanetPictureBox.TabStop = false;
        // 
        // gameLoopTimer
        // 
        this.gameLoopTimer.Enabled = true;
        this.gameLoopTimer.Tick += new System.EventHandler(this.gameLoopTimer_Tick);
        // 
        // GameMode2Control
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackgroundImage = global::GingaGame.Resource1.Background2;
        this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.Controls.Add(this.canvasPictureBox);
        this.Controls.Add(this.evolutionCyclePictureBox);
        this.Controls.Add(this.evolutionCycleLabel);
        this.Controls.Add(this.fpsLabel);
        this.Controls.Add(this.nextPlanetLabel);
        this.Controls.Add(this.nextPlanetPictureBox);
        this.Controls.Add(this.scoreLabel);
        this.Controls.Add(this.scoreboardLabel);
        this.Controls.Add(this.topScoresLabel);
        this.Dock = System.Windows.Forms.DockStyle.Fill;
        this.MinimumSize = new System.Drawing.Size(1540, 846);
        this.Name = "GameMode2Control";
        this.Size = new System.Drawing.Size(1540, 846);
        ((System.ComponentModel.ISupportInitialize)(this.canvasPictureBox)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.evolutionCyclePictureBox)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nextPlanetPictureBox)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label evolutionCycleLabel;
    private System.Windows.Forms.Label fpsLabel;
    private System.Windows.Forms.Label nextPlanetLabel;
    private System.Windows.Forms.Label scoreboardLabel;
    private System.Windows.Forms.Label scoreLabel;
    private System.Windows.Forms.Label topScoresLabel;
    private System.Windows.Forms.PictureBox canvasPictureBox;
    private System.Windows.Forms.PictureBox evolutionCyclePictureBox;
    private System.Windows.Forms.PictureBox nextPlanetPictureBox;
    private System.Windows.Forms.Timer gameLoopTimer;
}