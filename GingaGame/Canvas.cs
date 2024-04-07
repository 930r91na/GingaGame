﻿#nullable enable
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GingaGame;

public class Canvas
{
    Scene scene;
    PictureBox pct;
    private byte[] _bits = null!; 
    private int _stride, _pixelFormatSize;
    public Bitmap? Bitmap;
    public Container? Container;
    public int Height;
    public float Width;

    public Canvas(Scene scene, PictureBox pct)
    {
        this.scene = scene;
        Init(pct, pct.Width, pct.Height);
    }

    public Graphics? g { get; private set; }

    private void Init(PictureBox pct, int initWidth, int initHeight)
    {
        // Define the pixel format for the bitmap
        const PixelFormat format = PixelFormat.Format32bppArgb;

        // Create a new bitmap with the specified width, height, and pixel format
        Bitmap = new Bitmap(initWidth, initHeight, format);

        // Create a Graphics object from the bitmap
        Graphics.FromImage(Bitmap);

        // Set the width and height of the canvas
        Width = initWidth;
        Height = initHeight;

        // Calculate the size of each pixel in bytes
        _pixelFormatSize = Image.GetPixelFormatSize(format) / 8;

        // Calculate the stride (the width of a single row of pixels, in bytes)
        _stride = _pixelFormatSize * initWidth;

        // Calculate any necessary padding to ensure that each row aligns to a 4-byte boundary
        var padding = _stride % 4;
        _stride += padding == 0 ? 0 : 4 - padding;

        // Create a byte array to hold the pixel data
        _bits = new byte[_stride * initHeight];

        // Pin the byte array in memory so that it can't be moved by the garbage collector
        var handle = GCHandle.Alloc(_bits, GCHandleType.Pinned);

        // Get a pointer to the first element of the pinned byte array
        var bitsPtr = handle.AddrOfPinnedObject();

        // Create a new bitmap using the byte array for pixel data
        Bitmap = new Bitmap(initWidth, initHeight, _stride, format, bitsPtr);

        // Create a Graphics object from the bitmap
        g = Graphics.FromImage(Bitmap);


        // TODO: Check this
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        this.pct = pct;
        this.pct.Image = Bitmap;
    }

    public void InitializeContainer()
    {
        const float verticalTopMargin = 70;
        const float verticalBottomMargin = 20;
        var horizontalMargin = (Width - Width / 3) / 2;
        var topLeft = new PointF(horizontalMargin, verticalTopMargin);
        var topRight = new PointF(Width - horizontalMargin, verticalTopMargin);
        var bottomLeft = new PointF(horizontalMargin, Height - verticalBottomMargin);
        var bottomRight = new PointF(Width - horizontalMargin, Height - verticalBottomMargin);

        Container = new Container(topLeft, topRight, bottomLeft, bottomRight);
    }

    public void RenderEndLine(bool rendered)
    {
        const float verticalTopMargin = 70;
        var horizontalMargin = (Width - Width / 3) / 2;

        var topLeft = new PointF(horizontalMargin, verticalTopMargin);
        var topRight = new PointF(Width - horizontalMargin, verticalTopMargin);

        var blinkOn = DateTime.Now.Second % 2 == 0;

        var currentPen = blinkOn ? Pens.Red : Pens.Transparent;

        // Draw the end line
        g?.DrawLine(rendered ? currentPen : Pens.Transparent, topLeft, topRight);
    }
}