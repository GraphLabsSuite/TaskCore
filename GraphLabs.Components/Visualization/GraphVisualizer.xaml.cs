using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GraphLabs.Tasks.Components.Helpers;
using GraphLabs.Tasks.Core;
using GraphLabs.Tasks.Core.Helpers;
using Vertex = GraphLabs.Tasks.Components.Visualization.Vertex;

namespace GraphLabs.Tasks.Components.Visualization
{
    /// <summary> Контрол для визуализации графов </summary>
    public sealed partial class GraphVisualizer : UserControl, IGraph
    {
        #region Внешность

        // ReSharper disable InconsistentNaming
        private const double DEFAULT_VERTEX_RADIUS = 10.0;
        private static readonly Color DEFAULT_VERTEX_BACKGROUND_COLOR = Colors.LightGray;
        private static readonly Color DEFAULT_VERTEX_BORDER_COLOR = Colors.Black;
        private const double DEFAULT_VERTEX_BORDER_THICKNESS = 2.0;
        private static readonly Color DEFAULT_EDGE_STROKE_COLOR = Colors.Black;
        private const double DEFAULT_EDGE_STROKE_THICKNESS = 1.0;
        // ReSharper restore InconsistentNaming

        /// <summary> Радиус вершины по-умолчанию </summary>
        public double DefaultVertexRadius
        {
            get { return (double)GetValue(DefaultVertexRadiusProperty); }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(!double.IsNaN(value) && !double.IsInfinity(value));
                SetValue(DefaultVertexRadiusProperty, value);
            }
        }

        /// <summary> Кисть для фона вершины по-умолчанию </summary>
        public Brush DefaultVertexBackground
        {
            get { return (Brush)GetValue(DefaultVertexBackgroundProperty); }
            set { SetValue(DefaultVertexBackgroundProperty, value); }
        }

        /// <summary> Кисть для границы вершины по-умолчанию </summary>
        public Brush DefaultVertexBorderBrush
        {
            get { return (Brush)GetValue(DefaultVertexBorderBrushProperty); }
            set { SetValue(DefaultVertexBorderBrushProperty, value); }
        }

        /// <summary> Толщина границы вершиныпо-умолчанию </summary>
        public double DefaultVertexBorderThickness
        {
            get { return (double)GetValue(DefaultVertexBorderThicknessProperty); }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(!double.IsNaN(value) && !double.IsInfinity(value));
                SetValue(DefaultVertexBorderThicknessProperty, value);
            }
        }

        /// <summary> Кисть ребра по-умолчанию </summary>
        public Brush DefaultEdgeStroke
        {
            get { return (Brush)GetValue(DefaultEdgeStrokeProperty); }
            set { SetValue(DefaultEdgeStrokeProperty, value); }
        }

        /// <summary> Толщина ребра по-умолчанию </summary>
        public double DefaultEdgeStrokeThickness
        {
            get { return (double)GetValue(DefaultEdgeStrokeThicknessProperty); }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(!double.IsNaN(value) && !double.IsInfinity(value));
                SetValue(DefaultEdgeStrokeThicknessProperty, value);
            }
        }

        /// <summary> Темплейт вершины по-умолчанию </summary>
        public Style DefaultVertexStyle
        {
            get { return (Style)GetValue(DefaultVertexStyleProperty); }
            set { SetValue(DefaultVertexStyleProperty, value); }
        }

        /// <summary> Радиус вершины по-умолчанию </summary>
        public static DependencyProperty DefaultVertexRadiusProperty =
            DependencyProperty.Register(
                "DefaultVertexRadius",
                typeof(double),
                typeof(GraphVisualizer),
                new PropertyMetadata(DEFAULT_VERTEX_RADIUS, DefaultVertexRadiusChanged)
                );

        /// <summary> Кисть для фона вершины по-умолчанию </summary>
        public static DependencyProperty DefaultVertexBackgroundProperty =
            DependencyProperty.Register(
                "DefaultVertexBackground",
                typeof(Brush),
                typeof(GraphVisualizer),
                new PropertyMetadata(new SolidColorBrush(DEFAULT_VERTEX_BACKGROUND_COLOR), DefaultVertexBackgroundChanged)
                );

        /// <summary> Кисть для фона вершины по-умолчанию </summary>
        public static DependencyProperty DefaultVertexBorderBrushProperty =
            DependencyProperty.Register(
                "DefaultVertexBorderBrush",
                typeof(Brush),
                typeof(GraphVisualizer),
                new PropertyMetadata(new SolidColorBrush(DEFAULT_VERTEX_BORDER_COLOR), DefaultVertexBorderBrushChanged)
                );

        /// <summary> Радиус вершины по-умолчанию </summary>
        public static DependencyProperty DefaultVertexBorderThicknessProperty =
            DependencyProperty.Register(
                "DefaultVertexBorderThickness",
                typeof(double),
                typeof(GraphVisualizer),
                new PropertyMetadata(DEFAULT_VERTEX_BORDER_THICKNESS, DefaultVertexBorderThicknessChanged)
                );

        /// <summary> Кисть для ребра по-умолчанию </summary>
        public static DependencyProperty DefaultEdgeStrokeProperty =
            DependencyProperty.Register(
                "DefaultEdgeStroke",
                typeof(Brush),
                typeof(GraphVisualizer),
                new PropertyMetadata(new SolidColorBrush(DEFAULT_EDGE_STROKE_COLOR), DefaultEdgeStrokeChanged)
                );

        /// <summary> Толщина ребра по-умолчанию </summary>
        public static DependencyProperty DefaultEdgeStrokeThicknessProperty =
            DependencyProperty.Register(
                "DefaultEdgeStrokeThickness",
                typeof(double),
                typeof(GraphVisualizer),
                new PropertyMetadata(DEFAULT_EDGE_STROKE_THICKNESS, DefaultEdgeStrokeThicknessChanged)
                );

        /// <summary> Темплейт вершины по-умолчанию </summary>
        public static DependencyProperty DefaultVertexStyleProperty =
            DependencyProperty.Register(
                "DefaultVertexStyle",
                typeof(Style),
                typeof(GraphVisualizer),
                new PropertyMetadata(null, DefaultVertexStyleChanged)
                );

        private static void DefaultVertexRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (double)args.NewValue;
            
            visualizer._vertices.Cast<Vertex>().ForEach(v => v.Radius = newValue);
        }

        private static void DefaultVertexBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (Brush)args.NewValue;

            visualizer._vertices.Cast<Vertex>().ForEach(v => v.Background = newValue);
        }

        private static void DefaultEdgeStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (Brush)args.NewValue;

            visualizer._edges.Cast<Edge>().ForEach(e => e.Stroke = newValue);
        }

        private static void DefaultEdgeStrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (double)args.NewValue;

            visualizer._edges.Cast<Edge>().ForEach(e => e.StrokeThickness = newValue);
        }

        private static void DefaultVertexBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (Brush)args.NewValue;

            visualizer._edges.Cast<Vertex>().ForEach(v => v.BorderBrush = newValue);
        }

        private static void DefaultVertexBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (double)args.NewValue;

            visualizer._edges.Cast<Vertex>().ForEach(v => v.BorderThickness = new Thickness(newValue));
        }

        private static void DefaultVertexStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (Style)args.NewValue;

            visualizer._edges.Cast<Vertex>().ForEach(v => v.Style = newValue ?? v.Resources[typeof(Vertex)] as Style);
        }

        #endregion // Внешность


        #region Отображаемый граф

        /// <summary> Отображаемый граф </summary>
        public static DependencyProperty GraphProperty =
            DependencyProperty.Register(
                "Graph",
                typeof(IObservableGraph),
                typeof(GraphVisualizer),
                new PropertyMetadata(GraphChanged));

        /// <summary> Callback на изменение Graph </summary>
        private static void GraphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Contract.Assert(d != null);

            var visualizer = (GraphVisualizer)d;
            if (e.OldValue != null)
            {
                var oldGraph = (IObservableGraph)e.OldValue;
                oldGraph.GraphChanged -= visualizer.DisplayedGraphChanged;
            }
            if (e.NewValue != null)
            {
                var newGraph = (IObservableGraph)e.NewValue;
                newGraph.GraphChanged += visualizer.DisplayedGraphChanged;
            }
            ((GraphVisualizer)d).Refresh();
        }

        private void DisplayedGraphChanged(object sender, GraphChangedEventArgs args)
        {
            if (_suspendNotifications)
                return;

            var graph = Graph;
            
            Contract.Assert(graph != null);

            _suspendNotifications = true;

            if (args.OldEdges != null)
            {
                args.OldEdges.ForEach(e =>
                    {
                        var toRemove = _edges.Single(p => EdgesComparer.Comparer.Equals(p, e));
                        RemoveEdge(toRemove);
                    });
            }
            if (args.OldVertices != null)
            {
                args.OldVertices.ForEach(e =>
                {
                    var toRemove = _vertices.Single(p => VerticesComparer.Comparer.Equals(p, e));
                    RemoveVertex(toRemove);
                });
            }
            if (args.NewVertices != null)
            {
                args.NewVertices.ForEach(AddVertex);
            }
            if (args.NewEdges != null)
            {
                args.NewEdges.ForEach(AddEdge);
            }

            _suspendNotifications = false;
        }

        /// <summary> Отображаемый граф </summary>
        public IObservableGraph Graph
        {
            get { return (IObservableGraph)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        #endregion // Отображаемый граф


        #region Методы и свойства визуализатора

        /// <summary> Анимация приостановлена? </summary>
        public static readonly DependencyProperty IsAnimationSuspendedProperty = DependencyProperty.Register(
            "IsAnimationSuspended",
            typeof(bool),
            typeof(GraphVisualizer),
            new PropertyMetadata(false));

        /// <summary> Анимация приостановлена? </summary>
        public bool IsAnimationSuspended
        {
            get { return (bool)GetValue(IsAnimationSuspendedProperty); }
            set { SetValue(IsAnimationSuspendedProperty, value); }
        }
        
        //TODO: переделать на нормальные методы типа SuspendNotifications() и ResumeNotifications()
        private bool _suspendNotifications = false;

        /// <summary> Алгоритм, используемый для визуализации </summary>
        public VisualizationAlgorithm VisualizationAlgorithm
        {
            get { return _visualizationAlgorithm; }
            set
            {
                if (value == _visualizationAlgorithm)
                {
                    return;
                }
                _visualizationAlgorithm = value;
                Refresh();
            }
        }
        private VisualizationAlgorithm _visualizationAlgorithm;

        /// <summary> Полностью обновляет состояние визуализатора, перечитывая отображаемый граф </summary>
        public void Refresh()
        {
            _vertices.Clear();
            _edges.Clear();
            LayoutRoot.Children.Clear();
            _suspendNotifications = true;
            

            if (Graph == null) return;
            AllowMultipleEdges = Graph.AllowMultipleEdges;
            Directed = Graph.Directed;

            var newGraph = Graph;
            foreach (var vertex in newGraph.Vertices)
            {
                AddVertex(vertex);
            }
            foreach (var edge in newGraph.Edges)
            {
                AddEdge(edge);
            }
            CalculateVertexPositions();
            _suspendNotifications = false;
        }

        /// <summary> Клик по вершине </summary>
        public event EventHandler<VertexClickEventArgs> VertexClick;

        private void OnVertexClick(Vertex vertex)
        {
            if (VertexClick != null)
            {
                VertexClick(this, new VertexClickEventArgs(vertex));
            }
        }

        #endregion // Методы и свойства визуализатора


        #region Вычисление коориднат вершин

        private DispatcherTimer _animationTimer;

        /// <summary> Устанавливает начальное рандомное положение вершин </summary>
        private void SetStartLocations()
        {
            //TODO: Заменить эту тупую проверку на нормальное масштабирование
            if (LayoutRoot.ActualHeight == 0 || LayoutRoot.ActualWidth == 0)
            {
                throw new InvalidOperationException("Размер визуализатора равен нулю!!!");
            }

            if (!_vertices.Any())
                return;

            var rnd = new Random();

            for (var i = 0; i < _vertices.Count; ++i)
            {
                var vertex = _vertices[i];
                vertex.X = rnd.NextDouble() * LayoutRoot.ActualWidth;
                vertex.Y = rnd.NextDouble() * LayoutRoot.ActualHeight;

                var j = 0;
                for (; j < i; j++)
                {
                    if (AreIntersecting(vertex, (Vertex)_vertices[j]))
                        break;
                }
                if (j < i)
                    --i;
            }
            
            var minX = _vertices.Min(v => v.X);
            var minY = _vertices.Min(v => v.Y);
            var maxX = _vertices.Max(v => v.X);
            var maxY = _vertices.Max(v => v.Y);

            var graphCenter = new Point((maxX + minX) / 2, (maxY + minY) / 2);
            var layoutCenter = new Point(LayoutRoot.ActualWidth / 2, LayoutRoot.ActualHeight / 2);

            var deltaX = layoutCenter.X - graphCenter.X;
            var deltaY = layoutCenter.Y - graphCenter.Y;

            foreach (Vertex vertex in _vertices)
            {
                vertex.X += deltaX;
                vertex.Y += deltaY;
            }
        }

        /// <summary> Рассотяние между v1 и v2 </summary>
        private static double Distance(Vertex v1, Vertex v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }

        /// <summary> Оперделяет, пересекаются ли вершины </summary>
        private static bool AreIntersecting(Vertex v1, Vertex v2)
        {
            return Distance(v1, v2) <= v1.Radius + v2.Radius;
        }

        private const double G = 1000000;
        private const double K = 0.5;
        private const double L = 200;
        private const double TICK_INTERVAL = 0.05;
        private const double ANIMATION_INTERVAL = 0.01;

        private void MoveVertices()
        {
            if (_visualizationAlgorithm != VisualizationAlgorithm.ChargesAndSprings)
            {
                return;
            }

            _animationTimer.Stop();

            if (!IsAnimationSuspended)
            {
                var coordsDeltas = new Point[_vertices.Count];

                // Считаем действующие на вершины силы
                for (var i = 0; i < _vertices.Count; ++i)
                {
                    var vi = _vertices[i];
                    var fx = 0.0;
                    var fy = 0.0;
                    var deltaI = new Point(0.0, 0.0);
                    for (var j = 0; j < _vertices.Count; ++j)
                    {
                        if (j == i) continue;
                        var vj = _vertices[j];

                        var distanceIj = Distance(vi, vj);

                        var ex = (vi.X - vj.X)/distanceIj;
                        var ey = (vi.Y - vj.Y)/distanceIj;
                        fx += G*ex/Math.Pow(distanceIj, 2);
                        fy += G*ey/Math.Pow(distanceIj, 2);

                        if (this[vi, vj] != null || this[vj, vi] != null)
                        {
                            fx -= K*(distanceIj - L)*ex;
                            fy -= K*(distanceIj - L)*ey;
                        }
                        else
                        {
                            fx -= K*(distanceIj - L)*ex/2;
                            fy -= K*(distanceIj - L)*ey/2;
                        }

                        deltaI.X += fx*TICK_INTERVAL;
                        deltaI.Y += fy*TICK_INTERVAL;
                    }
                    coordsDeltas[i] = deltaI;
                }

                // Подравниваем, чтобы картинка оставалась в центре
                var newPositions = new Point[_vertices.Count];
                for (var i = 0; i < _vertices.Count; ++i)
                {
                    var vi = _vertices[i];
                    var deltaI = coordsDeltas[i];
                    newPositions[i].X = vi.X + deltaI.X;
                    newPositions[i].Y = vi.Y + deltaI.Y;
                }
                var minX = newPositions.Min(p => p.X);
                var minY = newPositions.Min(p => p.Y);
                var maxX = newPositions.Max(p => p.X);
                var maxY = newPositions.Max(p => p.Y);

                var graphCenter = new Point((maxX + minX)/2, (maxY + minY)/2);
                var layoutCenter = new Point(LayoutRoot.ActualWidth/2, LayoutRoot.ActualHeight/2);

                var deltaX = layoutCenter.X - graphCenter.X;
                var deltaY = layoutCenter.Y - graphCenter.Y;

                for (var i = 0; i < newPositions.Length; ++i)
                {
                    newPositions[i].X += deltaX;
                    newPositions[i].Y += deltaY;
                }

                // Запускаем анимацию
                for (var i = 0; i < _vertices.Count; ++i)
                {
                    var vi = _vertices[i];
                    if (vi == _capturedVertex)
                    {
                        continue;
                    }
                    var targetPositionI = newPositions[i];

                    var xiAnimation = SilverlightHelper
                        .GetStoryboard(vi, "X", targetPositionI.X, ANIMATION_INTERVAL, null);
                    var yiAnimation = SilverlightHelper
                        .GetStoryboard(vi, "Y", targetPositionI.Y, ANIMATION_INTERVAL, null);

                    xiAnimation.Begin();
                    yiAnimation.Begin();
                }
            }
            _animationTimer.Start();
        }

        /// <summary> Считает позиции всех вершин, используя модель с зарядами и пружинками </summary>
        private void CalculateVertexPositions()
        {
            switch (_visualizationAlgorithm)
            {
                case VisualizationAlgorithm.ChargesAndSprings:
                    SetStartLocations();
                    _animationTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(ANIMATION_INTERVAL) };
                    _animationTimer.Tick += (s, e) => MoveVertices();
                    _animationTimer.Start();
                    break;
                case VisualizationAlgorithm.RandomPositions:
                    SetStartLocations();
                    break;
                case VisualizationAlgorithm.Circle:
                    throw new NotImplementedException();
                default:
                    throw new NotSupportedException(
                        string.Format("Указан неизвестный алгоритм визуализации {0}", _visualizationAlgorithm));
            }
        }

        #endregion // Вычисление коориднат вершин


        #region Перемещение вершин

        /// <summary> Разрешено ли перемещение вершин мышкой? </summary>
        public static readonly DependencyProperty IsMouseVerticesMovingEnebledProperty = DependencyProperty.Register(
            "IsMouseVerticesMovingEnabled", 
            typeof(bool), 
            typeof(GraphVisualizer), 
            new PropertyMetadata(true));

        /// <summary> Разрешено ли перемещение вершин мышкой? </summary>
        public bool IsMouseVerticesMovingEnabled
        {
            get { return (bool)GetValue(IsMouseVerticesMovingEnebledProperty); }
            set { SetValue(IsMouseVerticesMovingEnebledProperty, value); }
        }

        private ThrottledMouseMoveEvent _moveEvent;
        private Vertex _capturedVertex;

        private void InitVerticesMoving()
        {
            _moveEvent = new ThrottledMouseMoveEvent(LayoutRoot);
        }

        private void ReleaseVertex(object sender, MouseButtonEventArgs e)
        {
            Contract.Assert(_capturedVertex != null);

            _capturedVertex = null;
            _moveEvent.MouseMove -= MouseMoveVertex;
            LayoutRoot.MouseLeftButtonUp -= ReleaseVertex;
            LayoutRoot.ReleaseMouseCapture();
        }

        private void CaptureVertex(object sender, MouseButtonEventArgs args)
        {
            Contract.Assert(!args.Handled);
            Contract.Assert(sender != null);
            Contract.Assert(sender is Vertex);
            
            args.Handled = true;


            if (!IsMouseVerticesMovingEnabled)
            {
                OnVertexClick((Vertex)sender);
            }
            else
            {
                _capturedVertex = (Vertex)sender;
                _moveEvent.MouseMove += MouseMoveVertex;
                LayoutRoot.MouseLeftButtonUp += ReleaseVertex;
                LayoutRoot.CaptureMouse();
            }
        }

        private void MouseMoveVertex(object sender, MouseEventArgs mouseEventArgs)
        {
            Contract.Assert(_capturedVertex != null);

            var position = mouseEventArgs.GetPosition(LayoutRoot);

            {
                var radius = _capturedVertex.Radius;
                if (position.X < radius)
                {
                    position.X = radius;
                }
                if (position.X > LayoutRoot.ActualWidth - radius)
                {
                    position.X = LayoutRoot.ActualWidth - radius;
                }
                if (position.Y < radius)
                {
                    position.Y = radius;
                }
                if (position.Y > LayoutRoot.ActualHeight - radius)
                {
                    position.Y = LayoutRoot.ActualHeight - radius;
                }
            }

            _capturedVertex.X = position.X;
            _capturedVertex.Y = position.Y;
        }

        #endregion // Перемещение вершин


        #region Constructors

        /// <summary> Конструктор </summary>
        public GraphVisualizer()
        {
            InitializeComponent();
            _vertices = new ObservableCollection<Vertex>();
            _edges = new ObservableCollection<Edge>();
            _vertices.CollectionChanged += VerticesChanged;
            _edges.CollectionChanged += EdgesChanged;
            InitVerticesMoving();
        }

        #endregion // Constructors


        #region Edges&Vertices

        private readonly ObservableCollection<Vertex> _vertices;
        private readonly ObservableCollection<Edge> _edges;

        private void EdgesChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (_suspendNotifications)
                return;

            if (args.Action == NotifyCollectionChangedAction.Reset)
                return;

            _suspendNotifications = true;

            if (args.NewItems != null && args.Action != NotifyCollectionChangedAction.Remove)
            {
                args.NewItems.Cast<Edge>().ForEach(e =>
                        {
                            var vertex1 = Graph.Vertices.Single(v => VerticesComparer.Comparer.Equals(v, e.Vertex1));
                            var vertex2 = Graph.Vertices.Single(v => VerticesComparer.Comparer.Equals(v, e.Vertex2));
                            var newEdge = Directed
                                              ? (IEdge)new DirectedEdge(vertex1, vertex2)
                                              : (IEdge)new UndirectedEdge(vertex1, vertex2);

                            Graph.AddEdge(newEdge);
                        });
            }
            if (args.OldItems != null && args.Action != NotifyCollectionChangedAction.Add)
            {
                args.OldItems.Cast<Edge>().ForEach(e =>
                    {
                        var edge = Graph.Edges.Single(p => EdgesComparer.Comparer.Equals(p, e));
                        Graph.RemoveEdge(edge);
                    });
            }

            _suspendNotifications = false;
        }

        private void VerticesChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (_suspendNotifications)
                return;

            if (args.Action == NotifyCollectionChangedAction.Reset)
                return;

            _suspendNotifications = true;

            if (args.NewItems != null && args.Action != NotifyCollectionChangedAction.Remove)
            {
                args.NewItems.Cast<Vertex>().ForEach(v => Graph.AddVertex(new Core.Vertex(v.Name)));
            }
            if (args.OldItems != null && args.Action != NotifyCollectionChangedAction.Add)
            {
                args.OldItems.Cast<Vertex>().ForEach(v =>
                    {
                        var vertex = Graph.Vertices.Single(p => VerticesComparer.Comparer.Equals(p, v));
                        Graph.RemoveVertex(vertex);
                    });
            }

            _suspendNotifications = false;
        }

        #endregion // Edges&Vertices


        #region Implementation of IGraph

        /// <summary> Граф ориентированный? </summary>
        public bool Directed { get; private set; }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        public bool AllowMultipleEdges { get; private set; }

        /// <summary> Числов рёбер </summary>
        public int EdgesCount
        {
            get { return _edges.Count; }
        }

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        public ReadOnlyCollection<IEdge> Edges
        {
            get { return new ReadOnlyCollection<IEdge>(_edges.Cast<IEdge>().ToList()); }
        }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public void AddEdge(IEdge edge)
        {
            var newEdge = new Edge
            {
                Vertex1 = _vertices.Single(v => VerticesComparer.Comparer.Equals(v, edge.Vertex1)),
                Vertex2 = _vertices.Single(v => VerticesComparer.Comparer.Equals(v, edge.Vertex2)),
                Directed = edge.Directed,
                Stroke = DefaultEdgeStroke,
                StrokeThickness = DefaultEdgeStrokeThickness
            };
            _edges.Add(newEdge);
            LayoutRoot.Children.Add(newEdge);
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        public void RemoveEdge(IEdge edge)
        {
            var toRemove = (Edge)edge;
            _edges.Remove(toRemove);
            LayoutRoot.Children.Remove(toRemove);
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        public IEdge this[IVertex v1, IVertex v2]
        {
            get
            {
                var toFind = new Edge { Vertex1 = v1, Vertex2 = v2, Directed = this.Directed };
                return _edges.FirstOrDefault(e => EdgesComparer.Comparer.Equals(e, toFind));
            }
        }

        /// <summary> Числов вершин </summary>
        public int VerticesCount
        {
            get { return _vertices.Count; }
        }

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        public ReadOnlyCollection<IVertex> Vertices
        {
            get { return new ReadOnlyCollection<IVertex>(_vertices.Cast<IVertex>().ToList()); }
        }

        /// <summary> Добавляет вершину vertex в граф </summary>
        /// <remarks> Для добавления вершины на конкретную позицию используйте AddVertex(Vertex) </remarks>
        public void AddVertex(IVertex vertex)
        {
            var newVertex = new Vertex
            {
                Name = vertex.Name,
                BorderThickness = new Thickness(DefaultVertexBorderThickness),
                BorderBrush = DefaultVertexBorderBrush,
                Background = DefaultVertexBackground,
                Style = DefaultVertexStyle,
                Radius = DefaultVertexRadius
            };
            newVertex.MouseLeftButtonDown += CaptureVertex;
            _vertices.Add(newVertex);
            LayoutRoot.Children.Add(newVertex);           
        }

        /// <summary> Добавляет вершину vertex в граф </summary>
        public void AddVertex(Vertex vertex)
        {
            // Когда добавляется вершина, с уже установленными координатами, нужно пнуть байдинги, чтобы они
            // сработали, когда мы добавим вершину на новый канвас. Сами они почему-то как-то криво работают.
            // Чтобы пнуть, просто меняем координаты на нулевые и после добавления возвращаем обратно.
            var x = vertex.X;
            var y = vertex.Y;
            vertex.X = 0;
            vertex.Y = 0;
            _vertices.Add(vertex);
            vertex.MouseLeftButtonDown += CaptureVertex;
            LayoutRoot.Children.Add(vertex);
            vertex.X = x;
            vertex.Y = y;
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        public void RemoveVertex(IVertex vertex)
        {
            _edges.Where(e => e.IsIncidentTo(vertex)).ForEach(RemoveEdge);
            var toRemove = (Vertex)vertex;
            _vertices.Remove(toRemove);
            LayoutRoot.Children.Remove(toRemove);
        }

        #endregion
    }
}
