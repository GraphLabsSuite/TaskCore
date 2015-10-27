using System.Collections.ObjectModel;
using System.Globalization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Autofac;
using GraphLabs.Common;
using GraphLabs.Common.UserActionsRegistrator;
using GraphLabs.CommonUI.Controls;
using GraphLabs.CommonUI.Controls.ViewModels;
using GraphLabs.Graphs;
using GraphLabs.Graphs.UIComponents.Visualization;
using GraphLabs.Utils.Services;
using Moq;
using Edge = GraphLabs.Graphs.UIComponents.Visualization.Edge;

namespace GraphLabs.Tests.UI
{
    public partial class MainPage : UserControl, IDisposable
    {
        /// <summary> Ioc-контейнер </summary>
        protected IContainer Container { get; private set; }


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
        

        public MainPage(IContainer container)
        {
            InitializeComponent();

            Container = container;
            
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Loose);
            registratorMock.Setup(reg => reg.RegisterUserActionsAsync(
                It.IsAny<long>(),
                It.IsAny<Guid>(),
                It.Is<ActionDescription[]>(d => d.Count() == 1 && d[0].Penalty == 0),
                It.IsAny<bool>()))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] { _currentScore }, null, false, null)));
            registratorMock.Setup(reg => reg.RegisterUserActionsAsync(
                It.IsAny<long>(),
                It.IsAny<Guid>(),
                It.Is<ActionDescription[]>(d => d.Count() == 1 && d[0].Penalty != 0),
                It.IsAny<bool>()))
                .Callback<long, Guid, ActionDescription[], bool>((l, g, d, b) => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] { _currentScore = _currentScore - d[0].Penalty }, null, false, null)));

            _registratorWrapper = DisposableWcfClientWrapper.Create(registratorMock.Object);
            UserActionsManager = new UserActionsManager(0, new Guid(), _registratorWrapper, Container.Resolve<IDateTimeService>())
            {
                SendReportOnEveryAction = true
            };
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


        private DirectedWeightedGraph _visualizerGraphProto;
        private void RunClick(object sender, RoutedEventArgs e)
        {
            var graph = DirectedWeightedGraph.CreateEmpty(8);
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[0], graph.Vertices[1], 2));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[0], graph.Vertices[6], 6));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[1], graph.Vertices[2], 7));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[1], graph.Vertices[4], 2));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[2], graph.Vertices[3], 3));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[4], graph.Vertices[5], 2));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[4], graph.Vertices[6], 1));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[5], graph.Vertices[2], 1));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[5], graph.Vertices[7], 2));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[6], graph.Vertices[7], 4));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[7], graph.Vertices[3], 1));
            Visualizer.Graph = graph;
            //((Vertex)Visualizer.Vertices[3]).Background = new SolidColorBrush(Colors.Magenta);
            //((Vertex)Visualizer.Vertices[1]).Radius = 30;
            _visualizerGraphProto = graph;
            ((Button)sender).Visibility = Visibility.Collapsed;
        }

        private void ChangeLabelsClick(object sender, RoutedEventArgs e)
        {
            if (_visualizerGraphProto == null)
                return;

            var rnd = new Random();
            foreach (var v in _visualizerGraphProto.Vertices)
            {
                v.Label = rnd.Next(0, 10).ToString();
            }
        }

        private void ChangeStateClick(object sender, RoutedEventArgs e)
        {
            if (_visualizerGraphProto == null) return;
            if (Visualizer.GetType() != typeof (GraphVisualizer)) return;

            var t = (bool)Visualizer.GetValue(GraphVisualizer.IsMouseVerticesMovingEnebledProperty);
            Visualizer.SetValue(GraphVisualizer.IsMouseVerticesMovingEnebledProperty, !t);

            t = (bool)Visualizer.GetValue(GraphVisualizer.IsEdgesAddingEnabledProperty);
            Visualizer.SetValue(GraphVisualizer.IsEdgesAddingEnabledProperty, !t);
            Debug.WriteLine("Edges: {0}", !t);
        }

        private void EditVerticesClick(object sender, RoutedEventArgs e)
        {
            var dialog = new EditVerticesDialog(Visualizer.Graph, Visualizer.Vertices);
            dialog.Show();
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


        #region Log

        private int _currentScore = UserActionsManager.StartingScore;
        private readonly DisposableWcfClientWrapper<IUserActionsRegistratorClient> _registratorWrapper;

        public static readonly DependencyProperty LogProperty =
            DependencyProperty.Register("UserActionsManager", typeof(UserActionsManager), typeof(MainPage), new PropertyMetadata(null));

        public UserActionsManager UserActionsManager
        {
            get { return (UserActionsManager)GetValue(LogProperty); }
            set { SetValue(LogProperty, value); }
        }

        private readonly Random _rnd = new Random();
        private void AddMessageClick(object sender, RoutedEventArgs e)
        {
            var hasPenalty = _rnd.NextDouble() > 0.5;

            if (hasPenalty)
            {
                UserActionsManager.RegisterMistake("Ошибочка!", 1);
            }
            else
            {
                UserActionsManager.RegisterInfo("Информация");
            }
        }

        #endregion // Log


        public void Dispose()
        {
            _registratorWrapper.Dispose();
        }

        private void VisualizerVertexClick(object sender, VertexClickEventArgs e)
        {
            MessageBox.Show(e.Control.Name);
        }

        private void VisualizeEdgeClick(object sender, EdgeClickEventArgs e)
        {
            MessageBox.Show(e.Control.ToString());
        }
    }
}
