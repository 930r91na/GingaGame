using System.Collections.Generic;
using System.Drawing;

namespace GingaGame;

public class Scene
{
    public float elapsed;
    public Camera camera;
    public Layer back;
    public Map map;
    public List<Planet> Planets { get; } = [];

    public Scene(Camera camera, Layer back, Map map)
    {
        this.camera = camera;
        this.back = back;
        this.map = map;
    }
    public void Update()
    {
        camera.Update(elapsed);
        back.Camera = camera.Pos;
    }

    public void AddElement(Planet planet)
    {
        Planets.Add(planet);
    }

    public void RemoveElement(Planet planet)
    {
        Planets.Remove(planet);
    }

    public void Render(Graphics g)
    {
        Update();
        back.Display(g);
        foreach (var planet in Planets) planet.Render(g);
    }

    public void Clear()
    {
        Planets.Clear();
    }
}