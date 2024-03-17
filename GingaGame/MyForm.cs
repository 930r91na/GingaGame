using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GingaGame;

namespace GingaGame
{
    public partial class MyForm : Form
    {
        Scene scene;
        Canvas canvas;
        float delta;

        public MyForm()
        {
            InitializeComponent();
        }

        private void Init()
        {
            canvas = new Canvas(PCT_CANVAS);
            scene = new Scene();
            delta = 0;
        }

        private void MyForm_SizeChanged(object sender, EventArgs e)
        {
            Init();
        }
                
        private void TIMER_Tick(object sender, EventArgs e)
        {
            canvas.Render(scene, delta);
            delta += 0.001f;
        }

        private void MyForm_Load(object sender, EventArgs e)
        {

        }

        private void PCT_CANVAS_Click(object sender, EventArgs e)
        {

        }
    }
}
