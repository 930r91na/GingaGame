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
        
        private Planet selectedPlanet = null;
        private bool isDragging = false;
        Planet planetaTierra = new Planet(new Vector2(100, 100), Resource1.Tierra, 1f, 20f);


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

        private void PCT_CANVAS_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var element in scene.Elements)
            {
                if (element is Planet planet)
                {
                    float distance = Vector2.Distance(new Vector2(e.X, e.Y), planet.Position);
                    if (distance <= planet.Radius)
                    {
                        selectedPlanet = planet;
                        isDragging = true;
                        break;
                    }
                }
            }
        }


        private void PCT_CANVAS_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedPlanet != null)
            {
                selectedPlanet.Position = new Vector2(e.X, e.Y);
                canvas.Render(scene, delta);
            }
        }


        private void PCT_CANVAS_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                selectedPlanet = null;

                canvas.Render(scene, delta);
            }
        }


    }
}
