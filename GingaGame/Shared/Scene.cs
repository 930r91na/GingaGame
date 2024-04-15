using System.Collections.Generic;
using System.Drawing;
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
                NextPlanetIndex = nextPlanetIndex--
            };
            AddFloor(floor);
        }
    }

    public void Render(Graphics g)
    {
        foreach (var planet in Planets) planet.Render(g);
        foreach (var floor in Floors) floor.Render(g, _container);
        _container.Render(g);
    }

    public void Clear()
    {
        Planets.Clear();
    }
}