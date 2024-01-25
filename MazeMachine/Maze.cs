using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mangerie.MazeMachine
{
    internal class Maze: EmptyMaze
    {
        private List<Cell> frontier = new List<Cell>();
        private Cell currentCell;
      
        public Maze(int mazeSize, double canvasSize) : base(mazeSize, canvasSize)
        {
            this.currentCell = this.Entrance;
        }

        // Update frontier (= possible unvisited neighbours) each time a new cell is added to maze
        private void UpdateFrontier()
        {
            int currentX = this.currentCell.X;
            int currentY = this.currentCell.Y;

            // check to the left
            if (currentX > 2)
            {
                Cell neighbour = this.Field[currentX - 2, currentY];
                if (neighbour.Frontier == false && neighbour.Visited == false)
                {
                    neighbour.Frontier = true;
                    this.frontier.Add(neighbour);
                }
            }
            // check to the right
            if (currentX < this.width - 2)
            {
                Cell neighbour = this.Field[currentX + 2, currentY];
                if (neighbour.Frontier == false && neighbour.Visited == false)
                {
                    neighbour.Frontier = true;
                    this.frontier.Add(neighbour);
                }
            }
            // check above
            if (currentY > 2)
            {
                Cell neighbour = this.Field[currentX, currentY - 2];
                if (neighbour.Frontier == false && neighbour.Visited == false)
                {
                    neighbour.Frontier = true;
                    this.frontier.Add(neighbour);
                }
            }
            // check below
            if (currentY < this.height - 2)
            {
                Cell neighbour = this.Field[currentX, currentY + 2];
                if (neighbour.Frontier == false && neighbour.Visited == false)
                {
                    neighbour.Frontier = true;
                    this.frontier.Add(neighbour);
                }
            }
        }

        // Contemplate next step in maze: call this every tick
        public bool RecurseMaze()
        {
            int currentX = this.currentCell.X;
            int currentY = this.currentCell.Y;

            // Mark current cell as visited
            this.currentCell.Visited = true;
            this.currentCell.Wall = false;

            // Add walls to maze
            this.Field[currentX - 1, currentY - 1].Visited = true;
            this.Field[currentX, currentY - 1].Visited = true;
            this.Field[currentX + 1, currentY - 1].Visited = true;
            this.Field[currentX - 1, currentY].Visited = true;
            this.Field[currentX + 1, currentY].Visited = true;
            this.Field[currentX - 1, currentY + 1].Visited = true;
            this.Field[currentX, currentY + 1].Visited = true;
            this.Field[currentX + 1, currentY + 1].Visited = true;

            // Check for visited neighbours to bridge to
            List<Cell> visitedNeighbours = new List<Cell>();
            if (currentX > 2)
            {
                Cell neighbour = this.Field[currentX - 2, currentY];
                if (neighbour.Visited == true)
                {
                    visitedNeighbours.Add(neighbour);
                }
            }
            if (currentX < this.width - 2)
            {
                Cell neighbour = this.Field[currentX + 2, currentY];
                if (neighbour.Visited == true)
                {
                    visitedNeighbours.Add(neighbour);
                }
            }
            if (currentY > 2)
            {
                Cell neighbour = this.Field[currentX, currentY - 2];
                if (neighbour.Visited == true)
                {
                    visitedNeighbours.Add(neighbour);
                }
            }
            if (currentY < this.height - 2)
            {
                Cell neighbour = this.Field[currentX, currentY + 2];
                if (neighbour.Visited == true)
                {
                    visitedNeighbours.Add(neighbour);
                }
            }

            // If visited neighbours, build bridge (=open wall)
            if (visitedNeighbours.Count > 0)
            {
                int rndIndex = rnd.Next(visitedNeighbours.Count);
                Cell neighbour = visitedNeighbours[rndIndex];

                // Build bridge to unvisited neighbour
                int bridgeX = currentX + (neighbour.X - currentX) / 2;
                int bridgeY = currentY + (neighbour.Y - currentY) / 2;
                this.Field[bridgeX, bridgeY].Wall = false;
            }

            // Add neighbours to frontier: frontier decides which cells can be visited next
            this.UpdateFrontier();

            // Select next cell from fontier
            if (this.frontier.Count > 0)
            {
                int rndIndex = rnd.Next(this.frontier.Count);

                // Mark selected cell as current cell and remove from frontier
                this.currentCell = this.frontier[rndIndex];
                this.frontier.RemoveAt(rndIndex);

                return false; // Return false if next cell has been found 
            }
            return true; // Return true if recursion has finished
        }
        public void ResetField()
        {
            foreach (Cell cell in this.Field)
            {
                if (cell.X > 0 && cell.X < this.width - 1 && cell.Y > 0 && cell.Y < this.height - 1)
                {
                    cell.Visited = false;
                    cell.Wall = true;
                    cell.Frontier = false;
                }
            }
            this.Entrance = this.Exit;
            this.Exit = this.GetExit();
            this.currentCell = this.Entrance;
            frontier = new List<Cell>();
        }
    }
}
