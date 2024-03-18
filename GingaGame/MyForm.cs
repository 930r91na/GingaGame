using System;
using System.Windows.Forms;

namespace GingaGame;

public partial class MyForm : Form
{
    private readonly VPoint _earth;
    private readonly VPoint _moon;
    private readonly Canvas _canvas;
    private Container _container;
    private float delta;
    private bool isDragging;
    private readonly Scene scene;


    public MyForm()
    {
        InitializeComponent();
        _canvas = new Canvas(PCT_CANVAS.Size);
        PCT_CANVAS.Image = _canvas.Bitmap;
        _earth = new VPoint(100, 100, _canvas, Resource1.Tierra, 1f, 10f) { IsVisible = true };
        _moon = new VPoint(200, 200, _canvas, Resource1.Luna, 0.5f, 5f) { IsVisible = true };
        scene = new Scene();
        InitializeContainer();
        delta = 0;
        
        scene.AddElement(_earth);
        scene.AddElement(_moon);
    }

    private void InitializeContainer()
    {
        const float verticalMargin = 50;
        float horizontalMargin = (Width - Width / 3) / 2;
        var topLeft = new VPoint(horizontalMargin, verticalMargin, _canvas, null, 1, 5);
        var topRight = new VPoint(Width - horizontalMargin, verticalMargin, _canvas, null, 1, 5);
        var bottomLeft = new VPoint(horizontalMargin, Height - verticalMargin, _canvas, null, 1, 5);
        var bottomRight = new VPoint(Width - horizontalMargin, Height - verticalMargin, _canvas, null, 1, 5);
        _container = new Container(topLeft, topRight, bottomLeft, bottomRight);
    }


    private void MyForm_SizeChanged(object sender, EventArgs e)
    {
        MyForm_Load(sender, e);
    }

    private void TIMER_Tick(object sender, EventArgs e)
    {
        _canvas.FastClear();
        _container.Render(_canvas.Graphics);
        scene.Render(_canvas.Graphics, PCT_CANVAS.Size);
        PCT_CANVAS.Invalidate();
        delta += 0.001f;
    }

    private void MyForm_Load(object sender, EventArgs e)
    {
    }

    private void PCT_CANVAS_Click(object sender, EventArgs e)
    {
    }
}