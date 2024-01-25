using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mangerie.MazeMachine
{
    internal class Player
    {
        private readonly double radius;
        public double Radius { get { return radius; } }
        public double X { get; private set; }
        public double Y { get; private set; }

        // Speeds
        private float xMovement = 0F;
        private float yMovement = 0F;
        private float acceleration = 0.5F;
        private float inertia = 0.95F;
        private float maxSpeed = 5F;
        private float bounce = -0.05F;
        private float invert = 1F;

        // Movement instruction received
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up {  get; set; }
        public bool Down { get; set; }

        public Player(double x, double y, double radius)
        {
            this.X = x; this.Y = y;
            this.radius = radius;
        }

        public void Move(Maze maze)
        {
            // Apply inertia to movement
            this.xMovement *= this.inertia;
            this.yMovement *= this.inertia;

            // Apply acceleration 
            if (this.Left)
            {
                this.xMovement -= this.acceleration;
            }
            if (this.Right)
            {
                this.xMovement += this.acceleration;
            }
            if (this.Up)
            {
                this.yMovement -= this.acceleration;
            }
            if (this.Down)
            {
                this.yMovement += this.acceleration;
            }

            // Limit speed
            this.xMovement = CommonStatics.Clamp(this.xMovement, -this.maxSpeed, this.maxSpeed);
            this.yMovement = CommonStatics.Clamp(this.yMovement, -this.maxSpeed, this.maxSpeed);

            // Collision detection: deduce which cell is currently inhabited & check surrounding cells for collision
            int reducedX = (int)(this.X / maze.CellSize);
            reducedX = CommonStatics.Clamp(reducedX, 1, maze.width - 2);
            int reducedY = (int)(this.Y / maze.CellSize);
            reducedY = CommonStatics.Clamp(reducedY, 1, maze.height - 2);

            for (int x = reducedX - 1; x <= reducedX + 1; x++)
            {
                for (int y = reducedY - 1; y <= reducedY + 1; y++)
                {
                    Cell square = maze.Field[x, y];
                    if (square.Wall)
                    {
                        // Seperately check X and Y movement; if collision, apply bounce
                        bool collision = CommonStatics.TryCollisionDetection(this.X + this.xMovement, this.Y, this.radius, square.minX, square.minY, square.size);
                        if (collision == true)
                        {
                            this.xMovement *= bounce;
                        }
                        collision = CommonStatics.TryCollisionDetection(this.X, this.Y + this.yMovement, this.radius, square.minX, square.minY, square.size);
                        if (collision == true)
                        {
                            this.yMovement *= bounce;
                        }
                    }
                }
            }

            // Fallback: do another check to avoid bouncing inside wall
            for (int x = reducedX - 1; x <= reducedX + 1; x++)
            {
                for (int y = reducedY - 1; y <= reducedY + 1; y++)
                {
                    Cell square = maze.Field[x, y];
                    if (square.Wall)
                    {
                        // Seperately check X and Y movement; if collision, apply bounce
                        bool collision = CommonStatics.TryCollisionDetection(this.X + this.xMovement, this.Y + this.yMovement, this.radius, square.minX, square.minY, square.size);
                        if (collision == true)
                        {
                            this.xMovement = 0;
                            this.yMovement = 0;
                        }
                    }
                }
            }

            // Adjust coordinates
            this.X += this.xMovement;
            this.Y += this.yMovement;
        }
        public void Draw(Canvas canvas)
        {
            if (this.invert > 15)
            {
                this.invert = 0;
            }
            
            Ellipse model = new Ellipse();
            model.Width = 2 * this.radius;
            model.Height = 2 * this.radius;
            Canvas.SetLeft(model, this.X - this.radius);
            Canvas.SetTop(model, this.Y - this.radius);
            model.Fill = new SolidColorBrush(Colors.Black);
            canvas.Children.Add(model);

            float modifier = this.invert / 15;
            Ellipse model2 = new Ellipse();
            model2.Width = (2 * this.radius) * modifier;
            model2.Height = (2 * this.radius) * modifier;
            Canvas.SetLeft(model2, this.X - this.radius * modifier);
            Canvas.SetTop(model2, this.Y - this.radius * modifier);
            model2.Fill = new SolidColorBrush(Colors.White);
            canvas.Children.Add(model2);

            modifier = this.invert / 22;
            Ellipse model3 = new Ellipse();
            model3.Width = (2 * this.radius) * modifier;
            model3.Height = (2 * this.radius) * modifier;
            Canvas.SetLeft(model3, this.X - this.radius * modifier);
            Canvas.SetTop(model3, this.Y - this.radius * modifier);
            model3.Fill = new SolidColorBrush(Colors.Black);
            canvas.Children.Add(model3);

            this.invert++;
        }
    }
}
