using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace nthSpiderWeb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static PointCollection points = new PointCollection();
        static List<Line> lines = new List<Line>();
        static double pointDiameter = 1;
        static int maxPointCount = 500;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Events -------------------------------------------------------------------------------------------
        private void TextBox_Points_TextChanged(object sender, TextChangedEventArgs e)
            => UpdateCanvas();

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
            => UpdateCanvas();

        // Methods ------------------------------------------------------------------------------------------
        private void UpdateCanvas()
        {
            points.Clear();
            GeneratePoints();
            RenderPoints();

            lines.Clear();
            GenerateLines();
            RenderLines();
        }

        private void GeneratePoints()
        {
            double radius = Math.Min(Canvas_Points.ActualWidth, Canvas_Points.ActualHeight) / 2.1;

            if(!int.TryParse(TextBox_Points.Text, out int pointCount))
                return;
            pointCount %= maxPointCount;
            if(pointCount == 0)
                ++pointCount;

            double step = (2 * Math.PI) / pointCount;
            for(int i = 0; i < pointCount; ++i)
            {
                double x = radius * Math.Cos(i * step);
                double y = radius * Math.Sin(i * step);
                points.Add(new Point(x, y));
            }
        }

        private void RenderPoints()
        {
            Canvas_Points.Children.Clear();
            double width = Canvas_Points.ActualWidth;
            double height = Canvas_Points.ActualHeight;

            foreach(Point p in points)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Stroke = Brushes.Black;
                ellipse.Width = pointDiameter;
                ellipse.Height = pointDiameter;

                Canvas.SetLeft(ellipse, p.X + width / 2);
                Canvas.SetTop(ellipse, p.Y + height / 2);
                Canvas_Points.Children.Add(ellipse);
            }
        }

        private void GenerateLines()
        {
            // Width/height of canvas + point radius = offset
            double xOffset = Canvas_Points.ActualWidth / 2 + pointDiameter / 2;
            double yOffset = Canvas_Points.ActualHeight / 2 + pointDiameter / 2;

            for(int x = 0; x < points.Count; ++x)
            {
                Point p1 = points[x];
                for(int y = x+1; y < points.Count; ++y)
                {
                    Point p2 = points[y];
                    // Create line from p1 and p2
                    Line line = new Line()
                    {
                        X1 = p1.X + xOffset,
                        Y1 = p1.Y + yOffset,
                        X2 = p2.X + xOffset,
                        Y2 = p2.Y + yOffset,
                        Stroke = Brushes.Black
                    };
                    lines.Add(line);
                }
            }
        }

        private void RenderLines()
        {
            foreach(Line line in lines)
                Canvas_Points.Children.Add(line);
        }
    }
}
