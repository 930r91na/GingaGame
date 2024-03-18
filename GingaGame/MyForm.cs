using System;
using System.Windows.Forms;

namespace GingaGame;

public partial class MyForm : Form
{
    private readonly Canvas _canvas;
    private Container _container;
    private readonly Scene _scene;
    
    public MyForm()
    {
        InitializeComponent();
        _canvas = new Canvas(PCT_CANVAS.Size);
        PCT_CANVAS.Image = _canvas.Bitmap;
        var middle = _canvas.Width / 2;
        var earth = new VPoint(middle, 100, _canvas, Resource1.Tierra, 1f, 20f);
        var moon = new VPoint(middle, 20, _canvas, Resource1.Luna, 0.5f, 10f);
        _scene = new Scene();
        InitializeContainer();
        _scene.AddElement(earth);
        _scene.AddElement(moon);
    }

    private void InitializeContainer()
    {
        const float verticalMargin = 50;
        var horizontalMargin = (_canvas.Width - _canvas.Width / 3) / 2;
        var topLeft = new VPoint(horizontalMargin, verticalMargin, _canvas, null, 1, 5);
        var topRight = new VPoint(_canvas.Width - horizontalMargin, verticalMargin, _canvas, null, 1, 5);
        var bottomLeft = new VPoint(horizontalMargin, _canvas.Height - verticalMargin, _canvas, null, 1, 5);
        var bottomRight = new VPoint(_canvas.Width - horizontalMargin, _canvas.Height - verticalMargin, _canvas, null, 1, 5);
        _container = new Container(topLeft, topRight, bottomLeft, bottomRight);
    }
    
    private void TIMER_Tick(object sender, EventArgs e)
    {
        _canvas.FastClear();
        _container.Render(_canvas.Graphics);
        _scene.Render(_canvas.Graphics);
        PCT_CANVAS.Invalidate();
    }
    
    private void MyForm_SizeChanged(object sender, EventArgs e)
    {
        
    }

    private void MyForm_Load(object sender, EventArgs e)
    {
        
    }

    private void PCT_CANVAS_Click(object sender, EventArgs e)
    {
        
    }
}