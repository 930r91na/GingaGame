#nullable enable
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GingaGame;

public class Canvas
{
    private byte[] _bits = null!;
    private int _stride, _pixelFormatSize;
    public Bitmap? Bitmap;
    public Container? Container;
    public int Height;
    public float Width;

    public Canvas(Size size)
    {
        Init(size.Width, size.Height);
    }

    public Graphics? Graphics { get; private set; }

    private void Init(int initWidth, int initHeight)
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
        Graphics = Graphics.FromImage(Bitmap);
    }

    public void FastClear()
    {
        // Use unsafe code to allow pointer manipulation
        unsafe
        {
            // Lock the bitmaps bits in system memory so that they can be changed
            var bitmapData = Bitmap!.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadWrite,
                Bitmap.PixelFormat);

            // Calculate the number of bytes per pixel
            var bytesPerPixel = Image.GetPixelFormatSize(Bitmap.PixelFormat) / 8;

            // Get the height of the bitmap in pixels
            var heightInPixels = bitmapData.Height;

            // Get the width of the bitmap in bytes
            var widthInBytes = bitmapData.Width * bytesPerPixel;

            // Get a pointer to the first pixel in the bitmap
            var ptrFirstPixel = (byte*)bitmapData.Scan0;

            // Use parallel processing to clear each row of pixels
            Parallel.For(0, heightInPixels, y =>
            {
                // Get a pointer to the first pixel in the current row
                var currentLine = ptrFirstPixel + y * bitmapData.Stride;

                // Clear each pixel in the current row
                for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    // Set the blue component to 0
                    currentLine[x] = 0;

                    // Set the green component to 0
                    currentLine[x + 1] = 0;

                    // Set the red component to 0
                    currentLine[x + 2] = 0;

                    // Set the alpha component to 0
                    currentLine[x + 3] = 0;
                }
            });

            // Unlock the bitmaps bits from system memory
            Bitmap.UnlockBits(bitmapData);
        }
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
        
        bool blinkOn = (DateTime.Now.Millisecond % 2) == 0;

        Pen currentPen = blinkOn ? Pens.White : Pens.Red; // Alternate between white and red
        
        // Draw the end line
        if (rendered)
        {
            Graphics?.DrawLine(currentPen, topLeft, topRight);
        }
        else
        {
            Graphics?.DrawLine(Pens.Black, topLeft, topRight);
        }

    }
}