using System.Drawing;
using GingaGame.Shared;

namespace GingaGame.GameMode2;

public class Floor
{
    public int StartPositionY { get; set; }
    public int EndPositionY { get; set; }
    public int Index { get; set; }
    public int NextPlanetIndex { get; set; } // The index of the planet that the next level will allow

    public void Render(Graphics g, Container container, float yOffset = 0)
    {
        // Adjust the Y position with the offset
        var adjustedEndPositionY = EndPositionY - yOffset;

        var isLastFloor = NextPlanetIndex == -1; // Check if the current floor is the last one

        // Set the color to red if it's the last floor, otherwise set it to white
        var rectangleColor = isLastFloor ? Color.FromArgb(50, 255, 0, 0) : Color.FromArgb(50, 255, 255, 255);

        const int rectangleHeight = 30; // The height of the rectangle
        const int planetRadius = 15; // The radius of the planet
        var rectangleY = adjustedEndPositionY - rectangleHeight; // The Y position of the rectangle

        DrawFloorRectangle(g, container, rectangleColor, rectangleY, rectangleHeight);

        // If it's not the last floor, draw the planet with a fixed radius to the left of the rectangle
        if (isLastFloor) return;
        DrawNextFloorPlanet(g, container, planetRadius, rectangleY);
    }

    private static void DrawFloorRectangle(Graphics g, Container container, Color rectangleColor, float rectangleY,
        int rectangleHeight)
    {
        // Draw the rectangle
        var brush = new SolidBrush(rectangleColor);
        var pen = new Pen(Color.White);
        g.FillRectangle(brush, container.TopLeft.X, rectangleY, container.BottomRight.X - container.TopLeft.X,
            rectangleHeight);

        // Draw an outline around the rectangle
        g.DrawRectangle(pen, container.TopLeft.X, rectangleY, container.BottomRight.X - container.TopLeft.X,
            rectangleHeight);
    }

    private void DrawNextFloorPlanet(Graphics g, Container container, int planetRadius, float rectangleY)
    {
        var planetX = container.TopLeft.X - planetRadius * 2;
        var planetY = rectangleY + planetRadius;

        // Draw the planet
        var planet = new Planet(NextPlanetIndex, new Vector2(planetX, planetY));
        planet.RenderWithSize(g, planetRadius);
    }
}