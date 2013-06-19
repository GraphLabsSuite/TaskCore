using System.Collections.ObjectModel;
using System.Globalization;
using GraphLabs.Components.Controls.ViewModels;
using GraphLabs.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Vertex = GraphLabs.Components.Visualization.Vertex;
using Edge = GraphLabs.Components.Visualization.Edge;

namespace GraphLabs.Tests.UI
{
    public partial class MainPage : UserControl
    {

        #region CrazyAnimation


        public static DependencyProperty FiProperty =
            DependencyProperty.Register("Fi", typeof(double), typeof(MainPage), new PropertyMetadata(FiChanged));

        private static void FiChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            const double p = 100;
            const double eps = 0.2;
            var fi = (double)e.NewValue;
            var r = p / (1 - eps * Math.Cos(fi));
            var page = (MainPage)d;
            page.VertexA.X = page.B.X - r * Math.Cos(fi);
            page.VertexA.Y = page.B.Y + r * Math.Sin(fi);
            page.VertexA.Radius = 25 - 15 * Math.Cos(fi);

            page.B.Radius = 25 + 25 * Math.Cos(fi);
            const double p_b = 100;
            const double eps_b = 0.4;
            var fiB = (double)e.NewValue + Math.PI;
            var rB = p_b / (1 - eps_b * Math.Cos(fiB));
            page.B.X = 271.4415 + rB * Math.Cos(fiB);
            page.B.Y = 200 + rB * Math.Sin(fiB);
        }

        private static Timeline GetAnimation(object animateTo, Duration duration)
        {
            Timeline animation;

            if (animateTo is double)
                animation = new DoubleAnimation { To = (double)animateTo };
            else if (animateTo is Color)
                animation = new ColorAnimation { To = (Color)animateTo };
            else 
                throw new NotSupportedException();

            animation.Duration = duration;
            return animation;
        }

        private Storyboard GetStoryboard(DependencyObject targetObject, string targetProperty,
            object animateTo, double duration, Action<object, EventArgs> onCompletedAction)
        {
            var dur = new Duration(TimeSpan.FromSeconds(duration));
            var animation = GetAnimation(animateTo, dur);
            var storyboard = new Storyboard { Duration = dur };
            Storyboard.SetTarget(storyboard, targetObject);
            Storyboard.SetTargetProperty(storyboard, new PropertyPath(targetProperty));
            storyboard.Children.Add(animation);
            storyboard.AutoReverse = false;
            if (onCompletedAction != null)
                storyboard.Completed += (sender, args) => onCompletedAction(sender, args);
            return storyboard;
        }

        private void Circulate()
        {
            SetValue(FiProperty, (double)0);
            GetStoryboard(this, "Fi", 2 * Math.PI, 3, (s, a) => Circulate()).Begin();
            var color = GetStoryboard(B, "(Edge.Background).(SolidColorBrush.Color)", Colors.Red, 1.5, null);
            color.AutoReverse = true;
            color.Begin();
            color = GetStoryboard(VertexA, "(Edge.Background).(SolidColorBrush.Color)", Colors.Purple, 1.5, null);
            color.AutoReverse = true;
            color.Begin();
        }

        #endregion CrazyAnimation


        public MainPage()
        {
            InitializeComponent();     
        }


        #region buttonsClicks

        private void MoveVertexClick(object sender, RoutedEventArgs e)
        {
            const double duration = 1;
            GetStoryboard(VertexA, "X", 75.0, duration, null).Begin();
            GetStoryboard(VertexA, "Y", 200.0, duration, null).Begin();
            GetStoryboard(VertexA, "Radius", 10.0, duration, (s, a) => Circulate()).Begin();
            GetStoryboard(B, "(Edge.Background).(SolidColorBrush.Color)", Colors.Orange, duration, null).Begin();
            ResizeAndMoveBtn.IsEnabled = false;
        }

        private void AddEdgeClick(object sender, RoutedEventArgs e)
        {
            var newEdge = new Edge
                              {
                                  Vertex1 = VertexA,
                                  Vertex2 = B,
                                  Directed = true,
                              };
            Layout.Children.Add(newEdge);
            IsDirectedCheckBox.Visibility = Visibility.Visible;
            newEdge.SetBinding(Edge.DirectedProperty, new Binding
                                                          {
                                                              Path = new PropertyPath("IsChecked"),
                                                              Source = IsDirectedCheckBox
                                                          });
            AddEdgeBtn.IsEnabled = false;
        }


        private void RunClick(object sender, RoutedEventArgs e)
        {
            var graph = DirectedGraph.CreateEmpty(10);
            graph.AddEdge(new DirectedEdge(graph.Vertices[0], graph.Vertices[1]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[1], graph.Vertices[2]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[2], graph.Vertices[3]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[3], graph.Vertices[4]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[4], graph.Vertices[0]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[5], graph.Vertices[6]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[6], graph.Vertices[7]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[7], graph.Vertices[8]));
            Visualizer.Graph = graph;
            Visualizer.DefaultVertexRadius = 10.0;
            Visualizer.DefaultVertexBackground = new SolidColorBrush(Colors.LightGray);
            //((Vertex)Visualizer.Vertices[3]).Background = new SolidColorBrush(Colors.Magenta);
            //((Vertex)Visualizer.Vertices[1]).Radius = 30;
            ((Button)sender).Visibility = Visibility.Collapsed;
        }


        private void FitAutosizeClick(object sender, RoutedEventArgs e)
        {
            LargeVertexNameHere.Radius = LargeVertexNameHere.GetDesiredRadius();
        }

        private void RunMatrixClick(object sender, RoutedEventArgs e)
        {
            var source = new ObservableCollection<MatrixRowViewModel<string>>();

            const int UPPER = 10;
            var rnd = new Random(UPPER);
            for (var i = 0; i < UPPER; ++i)
            {
                var row = new ObservableCollection<string> { i.ToString(CultureInfo.InvariantCulture) };
                for (var j = 0; j < UPPER; ++j)
                {
                    row.Add(rnd.Next(UPPER).ToString(CultureInfo.InvariantCulture));
                }
                source.Add(new MatrixRowViewModel<string>(row));
            }

            Grid.DataSource = source;
        }

        #endregion // buttonsClicks
    }
}
