using Mangerie.Kaleidoscope;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mangerie
{
    // Resources:
    // Study of a Nude Man - painting attributed to Gustave Courbet (MET, 2021.30) https://commons.wikimedia.org/wiki/File:Study_of_a_Nude_Man_-_MET_DP-20392-001.jpg
    // L'Origine du Monde - Gustave Courbet, https://upload.wikimedia.org/wikipedia/commons/b/b2/Origin_of_the_World_at_Orsay.jpg
    // Study of a Nude Man – painting by French Painter (MET, 52.71) https://commons.wikimedia.org/wiki/File:Study_of_a_Nude_Man_MET_ep52.71.R.jpg
    // A Nude Male Seen from the Back in Clouds (1602) by Daniel Fröschl. https://www.rawpixel.com/image/2577031/free-illustration-image-clouds-paper-vintage
    // Marble texture: https://commons.wikimedia.org/wiki/File:Marble_Stone_Texture.jpg
    // Wood texture: https://commons.wikimedia.org/wiki/File:Light_wood_texture.jpg


    public partial class MainWindow : Window
    {
        private int imageCount = 1;
        private int imageAngle = 0;
        private float imageScale = 1;
        private int currentClip = 0;
        private string imageSource = "/Resources/man.png"; 
        private bool playingMazerie = false;
        public int ImageAngle { get { return imageAngle; } }
        private ClipCoordinates clipPaths;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.clipPaths = new ClipCoordinates(600, 600);
            CombineImages();

        }
        private void CombineImages()
        {
            mainCanvas.Children.Clear();
            for (int i = 0; i < imageCount; i++)
            {
                Facet newFacet = new Facet(clipPaths.fields[this.currentClip], this.imageSource);
                newFacet.ImageAngle = imageAngle;
                newFacet.ImageScale = imageScale;
                RotateTransform rotate = new RotateTransform((360 / imageCount) * i);
                rotate.CenterX = newFacet.container.Width / 2;
                rotate.CenterY = newFacet.container.Width / 2;
                newFacet.container.RenderTransform = rotate;
                mainCanvas.Children.Add(newFacet);

                if (currentClip > 0)
                {
                    Facet mirroredFacet = new Facet(clipPaths.fields[this.currentClip], this.imageSource);
                    mirroredFacet.ImageAngle = imageAngle;
                    mirroredFacet.ImageScale = imageScale;
                    ScaleTransform mirror = new ScaleTransform();
                    mirror.ScaleX = -1;
                    TranslateTransform translate = new TranslateTransform();
                    translate.X = mirroredFacet.container.Width;
                    TransformGroup mirrorGroup = new TransformGroup();
                    mirrorGroup.Children.Add(mirror);
                    mirrorGroup.Children.Add(translate);
                    mirrorGroup.Children.Add(rotate);
                    mirroredFacet.container.RenderTransform = mirrorGroup;
                    mainCanvas.Children.Add(mirroredFacet);
                }
            }
        }

        private void rotateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.imageAngle = (int)rotateSlider.Value;
            this.imageAngleLbl.Text = "(" + this.imageAngle.ToString() + "°)";
            CombineImages();
        }

        private void mirrorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.currentClip = (int)mirrorSlider.Value;
            this.imageCount = Math.Max(1, this.currentClip);
            float mirAngle = 360 / (this.imageCount * 2);
            string angle = (this.currentClip == 0) ? "360" : mirAngle.ToString();
            this.mirrorAngleLbl.Text = "(" + angle + "°)";
            CombineImages();
        }

        private void scaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.imageScale != (float)scaleSlider.Value)
            {
                this.imageScale = (float)scaleSlider.Value;
                this.imageScaleLbl.Text = "(x" + this.imageScale.ToString() + ")";
                CombineImages();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
            {
                this.mazeGame.UserControl_KeyDown(e.Key);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (playingMazerie)
            {
                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
                {
                    mazeGame.UserControl_KeyUp(e.Key);
                }
                if (e.Key == Key.Escape)
                {
                    // Enable sliders
                    scaleSlider.IsEnabled = true;
                    mirrorSlider.IsEnabled = true;
                    rotateSlider.IsEnabled = true;
                    imageListbox.IsEnabled = true;

                    // Remove maze elements
                    Duration duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
                    DoubleAnimation anim = new DoubleAnimation(-1000, duration);
                    mazeTransform.BeginAnimation(TranslateTransform.YProperty, anim);
                    DoubleAnimation anim2 = new DoubleAnimation(0, duration);
                    mazeOverlay.BeginAnimation(Canvas.OpacityProperty, anim2);

                    // Deactivate maze
                    mazeGame.PauzeGame();
                    playingMazerie = false;
                }
            }
        }

        private void MazeMachine_Click(object sender, RoutedEventArgs e)
        {
            // Disable sliders
            scaleSlider.IsEnabled = false;
            mirrorSlider.IsEnabled = false;
            rotateSlider.IsEnabled = false;
            imageListbox.IsEnabled = false;

            // Show maze elements
            Duration duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            DoubleAnimation anim = new DoubleAnimation(0, duration);
            mazeTransform.BeginAnimation(TranslateTransform.YProperty, anim);
            DoubleAnimation anim2 = new DoubleAnimation(0.5, duration);
            anim2.BeginTime = new TimeSpan(0, 0, 0, 0, 500);
            mazeOverlay.BeginAnimation(Canvas.OpacityProperty, anim2);

            mazeGame.StartGame();
            playingMazerie = true;
        }

        private void imageListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (imageListbox.SelectedItem != null)
            {
                string url = imageListbox.SelectedItem.ToString();
                url = url.Split("ListBoxItem: ")[1];
                url = "/Resources/" + url;
                if (url != this.imageSource)
                {
                    this.imageSource = url;
                    CombineImages();
                }
            }
        }
    }
}