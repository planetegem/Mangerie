using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mangerie.Kaleidoscope
{
    public partial class Facet : UserControl
    {
        public int ImageAngle { get; set; }
        public float ImageScale { get; set; }
        public string ImageSource { get; set; }

        public Facet(System.Windows.Point[] path, string source)
        {
            InitializeComponent();
            this.ImageSource = source;
            this.DataContext = this;

            // Make clip path
            clipPath.Clear();
            for (int i = 0; i < path.Length; i++)
            {
                LineSegment segment = new LineSegment();
                segment.Point = path[i];
                clipPath.Add(segment);
            }
        }
        
    }
}
