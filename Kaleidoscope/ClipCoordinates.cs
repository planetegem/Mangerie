using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Mangerie.Kaleidoscope
{
    // Provides coordinates of clip paths adjusted for canvas size
    class ClipCoordinates
    {
        public readonly System.Windows.Point[][] fields;
        private double canvasWidth;
        private double canvasHeight;

        public ClipCoordinates(double canvasWidth, double canvasHeight)
        {
            this.canvasWidth = canvasWidth; this.canvasHeight = canvasHeight;

            // 0 Reflections
            float[,] reflection0 = { { 0.5F, 1F }, { 0.125F, 1F }, { 0F, 0.875F }, { 0F, 0.125F }, { 0.125F, 0F }, { 0.875F, 0F }, { 1F, 0.125F }, { 1F, 0.875F }, { 0.875F, 1F }, { 0.5F, 1F } };

            // 1 Reflections
            float[,] reflection1 = { { 0.5F, 1F }, { 0.125F, 1F }, { 0F, 0.875F }, { 0F, 0.125F }, { 0.125F, 0 }, { 0.5F, 0F } };

            // 3 Reflections
            float[,] reflection2 = { { 0.5F, 1F }, { 0.125F, 1F }, { 0F, 0.875F }, { 0F, 0.5F } };

            // 2 Reflections
            float[,] reflection3 = { { 0.5F, 1F }, { 0.211F, 1F }, { 0F, 0.789F } };

            

            // 7 Reflections
            float[,] reflection7 = { { 0.5F, 1F }, { 0F, 1F } };

            // ? Reflections
            float[,] reflection9 = { { 0.5F, 1F }, { 0.135F, 1F } };
            // 11 Reflections
            float[,] reflection11 = { { 0.5F, 1F }, { 0.211F, 1F } };

            this.fields = new System.Windows.Point[][]
            {
                ConvertCoords(reflection0),
                ConvertCoords(reflection1),
                ConvertCoords(reflection2),
                ConvertCoords(reflection3),
                ConvertCoords(reflection7),
                ConvertCoords(reflection9),
                ConvertCoords(reflection11),
            };
        }
        private System.Windows.Point[] ConvertCoords(float[,] path)
        {
            System.Windows.Point[] result = new System.Windows.Point[path.GetLength(0)];
            for (int i = 0; i < path.GetLength(0); i++)
            {
                System.Windows.Point point = new System.Windows.Point();
                point.X = (int)Math.Floor(path[i, 0] * this.canvasWidth);
                point.Y = (int)Math.Floor(path[i, 1] * this.canvasHeight);
                result[i] = point;
            }
            return result;
        }



    }
}
