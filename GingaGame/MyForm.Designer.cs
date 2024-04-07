namespace GingaGame
{
    partial class MyForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PNL_MAIN = new System.Windows.Forms.Panel();
            this.scoreboardLabel = new System.Windows.Forms.Label();
            this.topScoresLabel = new System.Windows.Forms.Label();
            this.EvolutionCyclePictureBox = new System.Windows.Forms.PictureBox();
            this.evolutionCycleLabel = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.nextPlanetPictureBox = new System.Windows.Forms.PictureBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.nextPlanetLabel = new System.Windows.Forms.Label();
            this.PCT_CANVAS = new System.Windows.Forms.PictureBox();
            this.TIMER = new System.Windows.Forms.Timer(this.components);
            this.PNL_MAIN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EvolutionCyclePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextPlanetPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PCT_CANVAS)).BeginInit();
            this.SuspendLayout();
            // 
            // PNL_MAIN
            // 
            this.PNL_MAIN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.PNL_MAIN.BackgroundImage = global::GingaGame.Resource1.Background2;
            this.PNL_MAIN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PNL_MAIN.Controls.Add(this.scoreboardLabel);
            this.PNL_MAIN.Controls.Add(this.topScoresLabel);
            this.PNL_MAIN.Controls.Add(this.EvolutionCyclePictureBox);
            this.PNL_MAIN.Controls.Add(this.evolutionCycleLabel);
            this.PNL_MAIN.Controls.Add(this.fpsLabel);
            this.PNL_MAIN.Controls.Add(this.nextPlanetPictureBox);
            this.PNL_MAIN.Controls.Add(this.scoreLabel);
            this.PNL_MAIN.Controls.Add(this.nextPlanetLabel);
            this.PNL_MAIN.Controls.Add(this.PCT_CANVAS);
            this.PNL_MAIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PNL_MAIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PNL_MAIN.ForeColor = System.Drawing.Color.Silver;
            this.PNL_MAIN.Location = new System.Drawing.Point(0, 0);
            this.PNL_MAIN.Margin = new System.Windows.Forms.Padding(4);
            this.PNL_MAIN.Name = "PNL_MAIN";
            this.PNL_MAIN.Size = new System.Drawing.Size(1540, 846);
            this.PNL_MAIN.TabIndex = 0;
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
            this.scoreboardLabel.Size = new System.Drawing.Size(113, 21);
            this.scoreboardLabel.TabIndex = 14;
            this.scoreboardLabel.Text = "Top Scores:";
            this.scoreboardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.topScoresLabel.Size = new System.Drawing.Size(0, 21);
            this.topScoresLabel.TabIndex = 13;
            this.topScoresLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EvolutionCyclePictureBox
            // 
            this.EvolutionCyclePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.EvolutionCyclePictureBox.Location = new System.Drawing.Point(21, 547);
            this.EvolutionCyclePictureBox.Name = "EvolutionCyclePictureBox";
            this.EvolutionCyclePictureBox.Size = new System.Drawing.Size(240, 240);
            this.EvolutionCyclePictureBox.TabIndex = 12;
            this.EvolutionCyclePictureBox.TabStop = false;
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
            this.evolutionCycleLabel.Size = new System.Drawing.Size(151, 21);
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
            this.fpsLabel.Size = new System.Drawing.Size(33, 16);
            this.fpsLabel.TabIndex = 10;
            this.fpsLabel.Text = "FPS: ";
            this.fpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.scoreLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scoreLabel.Font = new System.Drawing.Font("Gadugi", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.scoreLabel.Location = new System.Drawing.Point(12, 221);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(81, 21);
            this.scoreLabel.TabIndex = 8;
            this.scoreLabel.Text = "Score: 0";
            this.scoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.nextPlanetLabel.Size = new System.Drawing.Size(119, 21);
            this.nextPlanetLabel.TabIndex = 7;
            this.nextPlanetLabel.Text = "Next Planet:";
            this.nextPlanetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PCT_CANVAS
            // 
            this.PCT_CANVAS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PCT_CANVAS.BackColor = System.Drawing.Color.Transparent;
            this.PCT_CANVAS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PCT_CANVAS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PCT_CANVAS.Location = new System.Drawing.Point(280, 13);
            this.PCT_CANVAS.Margin = new System.Windows.Forms.Padding(4);
            this.PCT_CANVAS.Name = "PCT_CANVAS";
            this.PCT_CANVAS.Size = new System.Drawing.Size(1247, 820);
            this.PCT_CANVAS.TabIndex = 6;
            this.PCT_CANVAS.TabStop = false;
            this.PCT_CANVAS.Click += new System.EventHandler(this.PCT_CANVAS_Click);
            this.PCT_CANVAS.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PCT_CANVAS_MouseMove);
            // 
            // TIMER
            // 
            this.TIMER.Enabled = true;
            this.TIMER.Tick += new System.EventHandler(this.TIMER_Tick);
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1540, 846);
            this.Controls.Add(this.PNL_MAIN);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MyForm";
            this.Text = "PLAYGROUND || VERLETS";
            this.Load += new System.EventHandler(this.MyForm_Load);
            this.SizeChanged += new System.EventHandler(this.MyForm_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MyForm_KeyUp);
            this.PNL_MAIN.ResumeLayout(false);
            this.PNL_MAIN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EvolutionCyclePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextPlanetPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PCT_CANVAS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PNL_MAIN;
        private System.Windows.Forms.PictureBox PCT_CANVAS;
        private System.Windows.Forms.Timer TIMER;
        private System.Windows.Forms.Label nextPlanetLabel;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.PictureBox nextPlanetPictureBox;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.PictureBox EvolutionCyclePictureBox;
        private System.Windows.Forms.Label evolutionCycleLabel;
        private System.Windows.Forms.Label scoreboardLabel;
        private System.Windows.Forms.Label topScoresLabel;
    }
}

