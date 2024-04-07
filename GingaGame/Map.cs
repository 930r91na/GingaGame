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
        private string map;
        public Bitmap BMP;
        Size size;
        public static int Unit;     // map unit
        public static int K = 5;    // constant increment
    
        public Map()
        {
            map = "";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#.....#........";
            map+= "........#######........";
            map+= ".......................";
            map+= ".......................";

            GenerateMap();
        }

        private void GenerateMap()
        {
            char v;
            Graphics g;

            // Map properties
            size = new Size(23, 14);
            BMP = new Bitmap(size.Width * Unit, size.Height * Unit);
            g = Graphics.FromImage(BMP);

            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    v = map[y * size.Width + x];
                    if (v == '#')
                    {
                        g.FillRectangle(Brushes.Red, x * Unit, y * Unit, Unit, Unit);
                    }
                }
            }
        }
    }
}
