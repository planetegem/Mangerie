using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mangerie.MazeMachine
{
    internal class EmptyMaze
    {
        public readonly int width;
        public readonly int height;
        protected Random rnd = new Random();
        public double CellSize { get; protected set; }

        private Cell[,] field;
        public Cell[,] Field { get { return field; } }
        public Cell Entrance { get; protected set; }
        public Cell Exit { get; protected set; }

        public EmptyMaze(int mazeSize, double canvasSize)
        {
            // Widt & Height need to be uneven
            this.width = mazeSize * 2 + 1;
            this.height = mazeSize * 2 + 1;
            this.CellSize = canvasSize/this.width;

            // Create a blank field
            this.field = new Cell[this.width, this.height];
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    Cell newCell = new Cell(x, y, this.CellSize);
                    if (x == 0 || y == 0 || x == this.width - 1 || y == this.height - 1)
                    {
                        newCell.Visited = true;
                    }
                    this.field[x, y] = newCell;
                }
            }

            // Prepare entrance (any position in maze) and exit (on extremity)
            int startX = rnd.Next(this.width / 2) * 2 + 1;
            int startY = rnd.Next(this.height / 2) * 2 + 1;
            this.Entrance = Field[startX, startY];
            this.Exit = this.GetExit();
        }

        // Select random outside cell
        public Cell GetExit()
        {
            var direction = CommonStatics.RandomEnumValue<Directions>(this.rnd);
            int startX, startY;

            switch (direction)
            {
                case Directions.up:
                    startX = rnd.Next(this.width / 2) * 2 + 1;
                    startY = 1;
                    break;
                case Directions.down:
                    startX = rnd.Next(this.width / 2) * 2 + 1;
                    startY = this.height - 2;
                    break;
                case Directions.left:
                    startX = 1;
                    startY = rnd.Next(this.height / 2) * 2 + 1;
                    break;
                case Directions.right:
                    startX = this.width - 2;
                    startY = rnd.Next(this.height / 2) * 2 + 1;
                    break;
                default:
                    // Top left corner
                    startX = 1;
                    startY = 1;
                    break;
            }

            // Compare distance of possible Exit to Entrance
            var distance = Math.Sqrt(Math.Pow(startX - this.Entrance.X, 2) + Math.Pow(startY - this.Entrance.Y, 2));

            // Too close? Try again
            if (distance < this.width * 0.4)
            {
                return GetExit();
            }
            return Field[startX, startY];
        }

        public void Draw(Canvas canvas)
        {
            canvas.Children.Clear();
            foreach (Cell cell in this.field)
            {
                Rectangle block = new Rectangle();
                block.Width = cell.size;
                block.Height = cell.size;
                Canvas.SetLeft(block, cell.X * cell.size);
                Canvas.SetTop(block, cell.Y * cell.size);

                if (cell.Visited && cell.Wall)
                {
                    block.Fill = new SolidColorBrush(System.Windows.Media.Colors.PaleVioletRed);
                }
                else if (!cell.Visited)
                {
                    block.Fill = new SolidColorBrush(System.Windows.Media.Colors.AntiqueWhite);
                }
                canvas.Children.Add(block);
            }
        }
        public void DrawBorders(Canvas canvas)
        {
            foreach (Cell cell in this.field)
            {
                if (cell.X == 0 || cell.X == this.width - 1 || cell.Y == 0 || cell.Y == this.height - 1)
                {
                    Rectangle block = new Rectangle();
                    block.Width = cell.size;
                    block.Height = cell.size;
                    Canvas.SetLeft(block, cell.X * cell.size);
                    Canvas.SetTop(block, cell.Y * cell.size);
                    block.Fill = new SolidColorBrush(System.Windows.Media.Colors.PaleVioletRed);
                    canvas.Children.Add(block);
                }
            }
        }
    }
}
