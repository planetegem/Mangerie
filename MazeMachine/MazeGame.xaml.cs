using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Mangerie.MazeMachine
{
    public partial class MazeGame : UserControl
    {
        // Helper objects
        private DispatcherTimer ticker = new DispatcherTimer();
        private Random rnd = new Random();

        // Gameplay objects
        private Maze maze;
        private Player player;
        private Exit exit;
        private Cell entrance;
        private int currentTick = 0;

        private GameStates gamestate;
        private bool mazeGenerationComplete;
        public MazeGame()
        {
            InitializeComponent();

            // Create maze object
            maze = new Maze(12, mazeCanvas.Width);
            mazeGenerationComplete = false;
            maze.Draw(mazeCanvas);
            gamestate = GameStates.levelStart;
            entrance = maze.Entrance;

            // Create player
            player = new Player(entrance.minX + entrance.size * 0.5, entrance.minY + entrance.size * 0.5, entrance.size * 0.4);
            exit = new Exit(maze.Exit.minX, maze.Exit.minY, maze.Exit.size);

            // Initialize tickers
            ticker.Tick += Ticker_Tick;
            ticker.Interval = TimeSpan.FromMilliseconds(5);
            ticker.IsEnabled = true;
            ticker.Stop();

        }
        public void StartGame()
        {
            ticker.Start();
        }
        public void PauzeGame()
        {
            ticker.Stop();
        }


        private void Ticker_Tick(object sender, EventArgs e)
        {
            currentTick++;
            switch (gamestate)
            {
                case GameStates.levelStart:
                    if (currentTick > 75)
                    {
                        if (!mazeGenerationComplete)
                        {
                            mazeGenerationComplete = maze.RecurseMaze();
                            maze.Draw(mazeCanvas);
                        }
                        else
                        {
                            gamestate = GameStates.gameplay;
                            ticker.Interval = TimeSpan.FromMilliseconds(25);
                            player = new Player(entrance.minX + entrance.size * 0.5, entrance.minY + entrance.size * 0.5, entrance.size * 0.4);
                            exit = new Exit(maze.Exit.minX, maze.Exit.minY, maze.Exit.size);
                        }
                    }
                    break;

                case GameStates.levelEnd:
                    ticker.Interval = TimeSpan.FromMilliseconds(5);
                    maze.Draw(mazeCanvas);
                    player.Draw(mazeCanvas);
                    exit.Explode(mazeCanvas);
                    maze.DrawBorders(mazeCanvas);

                    if(exit.Size > mazeCanvas.Width * 3)
                    {
                        maze.ResetField();
                        mazeGenerationComplete = false;
                        gamestate = GameStates.levelStart;
                        entrance = maze.Entrance;
                    }

                    break;

                case GameStates.gameplay:
                    // Check if exit has been reached
                    bool checkCollision = CommonStatics.TryCollisionDetection(player.X, player.Y, player.Radius, exit.X, exit.Y, exit.Size);
                    if (checkCollision == true) 
                    {
                        gamestate = GameStates.levelEnd;
                        break;
                    }
                    else
                    {
                        maze.Draw(mazeCanvas);
                        player.Move(maze);
                        player.Draw(mazeCanvas);
                        exit.Draw(mazeCanvas);
                        break;
                    }

            }
        }
       
        public void UserControl_KeyDown(Key key)
        {
            if (gamestate.Equals(GameStates.gameplay))
            {
                switch (key)
                {
                    case Key.Up:
                        player.Up = true;
                        break;
                    case Key.Down:
                        player.Down = true;     
                        break;
                    case Key.Left:
                        player.Left = true;
                        break;
                    case Key.Right:
                        player.Right = true;
                        break;
                }
            }
        }
        public void UserControl_KeyUp(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    player.Up = false;
                    break;
                case Key.Down:
                    player.Down = false;
                    break;
                case Key.Left:
                    player.Left = false;
                    break;
                case Key.Right:
                    player.Right = false;
                    break;
            }
        }
        public void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }
    }
}
