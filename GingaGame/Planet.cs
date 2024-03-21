using System;
using System.Collections.Generic;
using System.Drawing;

namespace GingaGame;

public sealed class Planet(
    int planetType,
    float x,
    float y,
    Canvas canvas
)
    : VPoint(x, y, canvas, PlanetSizes.Sizes[planetType])
{
    public int PlanetType { get; } = planetType;
    public float Radius { get; } = PlanetSizes.Sizes[planetType];
    public int Points { get; private set; } = PlanetPoints.PointsPerPlanet[planetType];

    public void Render(Graphics g)
    {
        var imageWidth = Radius * 2;
        var imageHeight = Radius * 2;
        var texture = PlanetTextures.GetCachedTexture(PlanetType); // Use the cached version
        g?.DrawImage(texture, Position.X - imageWidth / 2, Position.Y - imageHeight / 2, imageWidth, imageHeight);
    }
}

public static class PlanetTextures
{
    private static readonly Dictionary<int, Image> CachedTextures = new(); 

    public static Image GetCachedTexture(int planetType)
    {
        if (CachedTextures.TryGetValue(planetType, out var texture)) return texture;
        // Load, resize, and cache the texture
        var image = LoadTexture(planetType);
        CachedTextures[planetType] = ResizeImage(image, PlanetSizes.Sizes[planetType]);

        return CachedTextures[planetType];
    }
    
    private static Image LoadTexture(int planetType)
    {
        return planetType switch
        {
            0 => Resource1.Pluto,
            1 => Resource1.Moon,
            2 => Resource1.Mercury,
            3 => Resource1.Mars,
            4 => Resource1.Venus,
            5 => Resource1.Earth,
            6 => Resource1.Neptune,
            7 => Resource1.Uranus,
            8 => Resource1.Saturn,
            9 => Resource1.Jupiter,
            10 => Resource1.Sun,
            _ => throw new ArgumentException("Invalid planetType")
        };
    }

    private static Image ResizeImage(Image image, float size)
    {
        var newSize = new Size((int)size * 2, (int)size * 2);
        var newImage = new Bitmap(image, newSize);
        return newImage;
    }
}

public static class PlanetSizes
{
    public static Dictionary<int, float> Sizes { get; } = new()
    {
        { 0, 10f },
        { 1, 15f },
        { 2, 20f },
        { 3, 25f },
        { 4, 30f },
        { 5, 35f },
        { 6, 40f },
        { 7, 45f },
        { 8, 50f },
        { 9, 55f },
        { 10, 60f }
    };
}

public static class PlanetPoints
{
    public static Dictionary<int, int> PointsPerPlanet { get; } = new()
    {
        { 0, 10 },
        { 1, 12 },
        { 2, 14 },
        { 3, 16 },
        { 4, 18 },
        { 5, 20 },
        { 6, 22 },
        { 7, 24 },
        { 8, 26 },
        { 9, 28 },
        { 10, 30 }
    };
}