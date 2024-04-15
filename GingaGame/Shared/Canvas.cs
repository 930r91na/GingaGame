#nullable enable
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GingaGame.Shared;

public class Canvas
{
    private byte[] _bits = null!;
    private int _stride, _pixelFormatSize;
    public Bitmap? Bitmap;
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
}