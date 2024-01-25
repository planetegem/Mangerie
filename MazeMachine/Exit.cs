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
    class Exit
    {
        private double x;
        public double X { get { return x; } }

        private double y;
        public double Y { get { return y; } }

        private double size;
        public double Size { get { return size; } }

        private float rotation = 0;

        public Exit(double x, double y, double size)
        {
            this.x = x; this.y = y; this.size = size;
        }
        public void Draw(Canvas canvas)
        {
            Rectangle block = new Rectangle();
            block.Width = this.size;
            block.Height = this.size;
            RotateTransform rotate = new RotateTransform(this.rotation);
            rotate.CenterX = this.size / 2;
            rotate.CenterY = this.size / 2;
            block.RenderTransform = rotate;

            Canvas.SetLeft(block, this.x);
            Canvas.SetTop(block, this.y);
            
            block.Fill = new SolidColorBrush(System.Windows.Media.Colors.DarkRed);

            Rectangle newBlock = new Rectangle();
            newBlock.Width = this.size * 0.7;
            newBlock.Height = this.size * 0.7;
            RotateTransform rotate2 = new RotateTransform(this.rotation);
            rotate2.CenterX = this.size * 0.35;
            rotate2.CenterY = this.size * 0.35;
            newBlock.RenderTransform = rotate2;
            Canvas.SetLeft(newBlock, this.x + this.size * 0.15);
            Canvas.SetTop(newBlock, this.y + this.size * 0.15);

            if (this.rotation % 24 == 0) 
            {
                newBlock.Fill = new SolidColorBrush(System.Windows.Media.Colors.Pink);
            }
            else
            {
                newBlock.Fill = new SolidColorBrush(System.Windows.Media.Colors.White);
            }

            canvas.Children.Add(block);
            canvas.Children.Add(newBlock);

            this.rotation += 12;
        }
        public void Explode(Canvas canvas)
        {
            // Grow object size
            this.x -= this.size * 0.25;
            this.y -= this.size * 0.25;
            this.size *= 1.5;

            Ellipse circle = new Ellipse();
            circle.Width = this.size;
            circle.Height = this.size;
            Canvas.SetLeft(circle, this.x);
            Canvas.SetTop(circle, this.y);
            circle.Fill = new SolidColorBrush(System.Windows.Media.Colors.White);

            RectangleGeometry clip = new RectangleGeometry();
            clip.Rect = new System.Windows.Rect(0, 0, canvas.Width, canvas.Height);

            canvas.Clip = clip;
            canvas.Children.Add(circle);
        }
    }
}
