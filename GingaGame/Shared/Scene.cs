using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GingaGame.GameMode2;

namespace GingaGame.Shared;

public class Scene
{
    private Container _container;
    public List<Planet> Planets { get; } = [];
    public List<Floor> Floors { get; } = [];

    public void AddPlanet(Planet planet)
    {
        Planets.Add(planet);
    }

    public void RemovePlanet(Planet planet)
    {
        Planets.Remove(planet);
    }

    private void AddFloor(Floor floor)
    {
        Floors.Add(floor);
    }

    public void AddContainer(Container container)
    {
        _container = container;
    }

    public void InitializeFloors(int floorHeight, int verticalTopMargin)
    {
        var nextPlanetIndex = PlanetSizes.Sizes.Count - 2; // We start from the second last planet
        for (var i = 0; i < PlanetSizes.Sizes.Count; i++)
        {
            var floor = new Floor
            {
                StartPositionY = i * floorHeight + verticalTopMargin,
                EndPositionY = (i + 1) * floorHeight + verticalTopMargin,
                Index = i,
                NextPlanetIndex = nextPlanetIndex--
            };
            AddFloor(floor);
        }
    }

    public void Render(Graphics g, float canvasHeight, float yOffset = 0)
    {
        // Calculate the visible range
        var visibleStartY = yOffset;
        var visibleEndY = yOffset + canvasHeight;

        // Check if the planets are within the visible range
        foreach (var planet in Planets.Where(planet =>
                     planet.Position.Y + planet.Radius >= visibleStartY &&
                     planet.Position.Y - planet.Radius <= visibleEndY))
            planet.Render(g, yOffset);

        // Check if the floor is within the visible range
        foreach (var floor in Floors.Where(floor =>
                     floor.EndPositionY >= visibleStartY && floor.StartPositionY <= visibleEndY))
            floor.Render(g, _container, yOffset);

        // Render the container if it's within the visible range
        if (_container.BottomLeft.Y >= visibleStartY && _container.TopLeft.Y <= visibleEndY)
            _container.Render(g, yOffset);
    }

    public void Clear()
    {
        Planets.Clear();
    }
}