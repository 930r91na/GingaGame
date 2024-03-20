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

    private void DrawPixel(int x, int y, Color color)
    {
        // Check if the x and y coordinates are within the valid range
        if (x < 0 || x >= Width || y < 0 || y >= Height) return;

        // Calculate the index of the first byte of the pixel in the byte array
        var index = x * _pixelFormatSize + y * _stride;

        _bits[index] = color.B; // (byte)Blue
        _bits[index + 1] = color.G; // (byte)Green
        _bits[index + 2] = color.R; // (byte)Red
        _bits[index + 3] = color.A; // (byte)Alpha
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

    public void DrawLine(int x1, int y1, int x2, int y2, Color color)
    {
        // Bresenham's line algorithm is used to draw a line between two points
        // This algorithm chooses the pixel at each step that will be closest to the true line

        // Calculate the absolute difference in x and y coordinates
        var dx = Math.Abs(x2 - x1);
        var dy = Math.Abs(y2 - y1);

        // Determine the direction of the line in x and y coordinates
        var sx = x1 < x2 ? 1 : -1;
        var sy = y1 < y2 ? 1 : -1;

        // Initialize the error term to compensate for the difference in variation in x and y
        var err = dx - dy;

        // Loop until the line is drawn from (x1, y1) to (x2, y2)
        while (true)
        {
            // Draw a pixel at the current position
            DrawPixel(x1, y1, color);

            // If the current position is the end position, break the loop
            if (x1 == x2 && y1 == y2) break;

            // Calculate the double of the error term
            var e2 = 2 * err;

            // Adjust the error term and the current position based on the slope of the line
            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }

            // If the double of the error term is greater than or equal to the difference in x, continue to the next iteration
            if (e2 >= dx) continue;

            // Adjust the error term and the current position based on the slope of the line
            err += dx;
            y1 += sy;
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
}