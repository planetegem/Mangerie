using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mangerie.MazeMachine
{
    internal class Cell
    {
        // Cell coordinates
        private int x, y;
        public int X { get { return x; } }
        public int Y { get { return y; } }

        // Cell booleans
        public bool Wall { get; set; }
        public bool Visited { get; set; }
        public bool Frontier { get; set; }

        // Geometry coordinates (used when drawing cell on canvas)
        public readonly double minX, maxX, minY, maxY, size;

        // Constructor
        public Cell(int x, int y, double cellSize)
        {
            this.x = x; this.y = y; this.size = cellSize;
            this.minX = x * cellSize;
            this.maxX = (x + 1) * cellSize;
            this.minY = y * cellSize;
            this.maxY = (y + 1) * cellSize;

            Wall = true; 
            Visited = false;
            Frontier = false;
        }
    }
}
