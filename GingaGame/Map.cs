using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingaGame
{
    public class Map
    {
        public string map;
        public Bitmap BMP;
        Size size;
        public static int Unit;     // map unit
        public static int K = 5;    // constant increment
        public int xTiles = 21;
        public int yTiles = 12;
        public Map()
        {
            map+= "0...................0";
            map+= "0......#.....#......0";
            map+= "0......#.....#......0";
            map+= "0......#.....#......0";
            map+= "0......#.....#......0";
            map+= "0......#.....#......0";
            map+= "0......#.....#......0";
            map+= "0......#.....#......0";
            map+= "0......#.....#......0";
            map+= "0......#.....#......0";
            map+= "0......#######......0";
            map+= "0...................0";
            map+= "0...................0";

            GenerateMap();
        }

        private void GenerateMap()
        {
            char v;
            Graphics g;

            // Map properties
            size = new Size(xTiles, yTiles);
            BMP = new Bitmap(size.Width * Unit, size.Height * Unit);
            g = Graphics.FromImage(BMP);


            // Draw map
            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    v = map[y * size.Width + x];
                    if (v == '#')
                    {
                        g.FillRectangle(Brushes.Blue, x * Unit, y * Unit, Unit, Unit);
                    }else if (v == '0')
                    {
                        g.FillRectangle(Brushes.White, x * Unit, y * Unit, Unit, Unit);
                    }
                }
            }
        }
    }
}
