using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GraphLabs.Common.Utils;
using GraphLabs.CommonUI.Helpers;
using GraphLabs.Graphs.Helpers;
using GraphLabs.Utils;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary> Контрол для визуализации графов </summary>
    public partial class GraphVisualizer : UserControl, IGraph<Vertex, Edge>
    {
        private static DependencyProperty RegisterDependencyProperty<T>(
            Expression<Func<GraphVisualizer, T>> accessor,
            PropertyMetadata metadata = null)
        {
            var propertyName = ExpressionUtility.NameForMember(accessor);
            return DependencyProperty.Register(propertyName, typeof(T), typeof(GraphVisualizer), metadata ?? new PropertyMetadata(null));
        }

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
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                SetValue(DefaultVertexBackgroundProperty, value);
            }
        }

        /// <summary> Кисть для границы вершины по-умолчанию </summary>
        public Brush DefaultVertexBorderBrush
        {
            get { return (Brush)GetValue(DefaultVertexBorderBrushProperty); }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                SetValue(DefaultVertexBorderBrushProperty, value);
            }
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
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                SetValue(DefaultEdgeStrokeProperty, value);
            }
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
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                SetValue(DefaultVertexStyleProperty, value);
            }
        }

        /// <summary> Возможность наложения вершин по умолчанию </summary>
        public bool AllowVerticesOverlap
        {
            get { return (bool)GetValue(AllowVerticesOverlapProperty); }
            set { SetValue(AllowVerticesOverlapProperty, value); }
        }

        /// <summary> Радиус вершины по-умолчанию </summary>
        public static readonly DependencyProperty DefaultVertexRadiusProperty = RegisterDependencyProperty(
            gv => gv.DefaultVertexRadius,
            new PropertyMetadata(DEFAULT_VERTEX_RADIUS, DefaultVertexRadiusChanged));

        /// <summary> Кисть для фона вершины по-умолчанию </summary>
        public static readonly DependencyProperty DefaultVertexBackgroundProperty = RegisterDependencyProperty(
            gv => gv.DefaultVertexBackground,
            new PropertyMetadata(new SolidColorBrush(DEFAULT_VERTEX_BACKGROUND_COLOR), DefaultVertexBackgroundChanged));

        /// <summary> Кисть для фона вершины по-умолчанию </summary>
        public static readonly DependencyProperty DefaultVertexBorderBrushProperty = RegisterDependencyProperty(
            gv => gv.DefaultVertexBorderBrush,
            new PropertyMetadata(new SolidColorBrush(DEFAULT_VERTEX_BORDER_COLOR), DefaultVertexBorderBrushChanged));

        /// <summary> Радиус вершины по-умолчанию </summary>
        public static readonly DependencyProperty DefaultVertexBorderThicknessProperty = RegisterDependencyProperty(
            gv => gv.DefaultVertexBorderThickness,
            new PropertyMetadata(DEFAULT_VERTEX_BORDER_THICKNESS, DefaultVertexBorderThicknessChanged));

        /// <summary> Кисть для ребра по-умолчанию </summary>
        public static readonly DependencyProperty DefaultEdgeStrokeProperty = RegisterDependencyProperty(
            gv => gv.DefaultEdgeStroke,
            new PropertyMetadata(new SolidColorBrush(DEFAULT_EDGE_STROKE_COLOR), DefaultEdgeStrokeChanged));

        /// <summary> Толщина ребра по-умолчанию </summary>
        public static readonly DependencyProperty DefaultEdgeStrokeThicknessProperty = RegisterDependencyProperty(
            gv => gv.DefaultEdgeStrokeThickness,
            new PropertyMetadata(DEFAULT_EDGE_STROKE_THICKNESS, DefaultEdgeStrokeThicknessChanged));

        /// <summary> Темплейт вершины по-умолчанию </summary>
        public static readonly DependencyProperty DefaultVertexStyleProperty = RegisterDependencyProperty(
            gv => gv.DefaultVertexStyle,
            new PropertyMetadata(null, DefaultVertexStyleChanged));

        /// <summary> Позволить наложение вершин </summary>
        public static readonly DependencyProperty AllowVerticesOverlapProperty = RegisterDependencyProperty(
            gv => gv.AllowVerticesOverlap,
            new PropertyMetadata(default(bool)));

        private static void DefaultVertexRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (double)args.NewValue;
            
            visualizer._vertices.ForEach(v => v.Radius = newValue);
        }

        private static void DefaultVertexBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (Brush)args.NewValue;

            visualizer._vertices.ForEach(v => v.Background = newValue);
        }

        private static void DefaultEdgeStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (Brush)args.NewValue;

            visualizer._edges.ForEach(e => e.Stroke = newValue);
        }

        private static void DefaultEdgeStrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (double)args.NewValue;

            visualizer._edges.ForEach(e => e.StrokeThickness = newValue);
        }

        private static void DefaultVertexBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (Brush)args.NewValue;

            visualizer._vertices.ForEach(v => v.BorderBrush = newValue);
        }

        private static void DefaultVertexBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (double)args.NewValue;

            visualizer._vertices.ForEach(v => v.BorderThickness = new Thickness(newValue));
        }

        private static void DefaultVertexStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var visualizer = (GraphVisualizer)d;
            var newValue = (Style)args.NewValue;

            visualizer._vertices.ForEach(v => v.Style = newValue ?? v.Resources[typeof(Vertex)] as Style);
        }

        #endregion // Внешность


        #region Отображаемый граф

        /// <summary> Отображаемый граф </summary>
        public static readonly DependencyProperty GraphProperty = 
            RegisterDependencyProperty(gv => gv.Graph, new PropertyMetadata(GraphChanged));

        /// <summary> Отображаемый граф </summary>
        public IObservableGraph Graph
        {
            get { return (IObservableGraph)GetValue(GraphProperty); }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                SetValue(GraphProperty, value);
            }
        }

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

            visualizer.Refresh();
        }

        private void DisplayedGraphChanged(object sender, GraphChangedEventArgs args)
        {
            if (_suspendNotifications)
                return;

            var graph = Graph;
                                                                                             
            Contract.Assert(graph != null);
                                                                                             
            _suspendNotifications = true;

            args.OldEdges?.ForEach(e =>                                                   
            {                                                                        
                var toRemove = _edges.Single(e.Equals);                              
                RemoveEdge(toRemove);                                                
            });

            args.OldVertices?.ForEach(e =>                                                
            {                                                                            
                var toRemove = _vertices.Single(e.Equals);
                RemoveVertex(toRemove);
            });

            args.NewVertices?.ForEach(AddVertex);
            args.NewEdges?.ForEach(AddEdge);

            _suspendNotifications = false;
        }

        #endregion // Отображаемый граф


        #region Методы и свойства визуализатора

        /// <summary> Анимация приостановлена? </summary>
        public static readonly DependencyProperty IsAnimationSuspendedProperty = RegisterDependencyProperty(
            gv => gv.IsAnimationSuspended, new PropertyMetadata(false));

        /// <summary> Анимация приостановлена? </summary>
        public bool IsAnimationSuspended
        {
            get { return (bool)GetValue(IsAnimationSuspendedProperty); }
            set { SetValue(IsAnimationSuspendedProperty, value); }
        }
        
        private bool _suspendNotifications = false;

        /// <summary> Алгоритм, используемый для визуализации </summary>
        public IVisualizationAlgorithm VisualizationAlgorithm
        {
            get { return _ivisualizationAlgorithm; }
            set
            {
                if (value == _ivisualizationAlgorithm)
                {
                    return;
                }

                value.Visualizer = this;
                _ivisualizationAlgorithm = value;
                Refresh();
            }
        }
        private IVisualizationAlgorithm _ivisualizationAlgorithm;

        /// <summary> Полностью обновляет состояние визуализатора, перечитывая отображаемый граф </summary>
        public void Refresh()
        {
            _vertices.Clear();
            _edges.Clear();
            LayoutRoot.Children.Clear();
            _suspendNotifications = true;

            if (Graph == null) return;

            var newGraph = Graph;
            foreach (var vertex in newGraph.Vertices)
            {
                AddVertex(vertex);
            }
            foreach (var edge in newGraph.Edges)
            {
                AddEdge(edge);
            }
            _ivisualizationAlgorithm.Visualize();
            _suspendNotifications = false;
        }

        /// <summary> Клик по вершине </summary>
        public event EventHandler<VertexClickEventArgs> VertexClick;

        private void OnVertexClick(Vertex vertex)
        {
            VertexClick?.Invoke(this, new VertexClickEventArgs(vertex));
        }

        /// <summary> Клик по ребру </summary>
        public event EventHandler<EdgeClickEventArgs> EdgeClick;

        private void OnEdgeClick(Edge edge)
        {
            EdgeClick?.Invoke(this, new EdgeClickEventArgs(edge));
        }

        #endregion // Методы и свойства визуализатора


        #region Вычисление коориднат вершин

        private DispatcherTimer _animationTimer;
        private const double MODEL_SIZE = 1000;

        private double GetScaleFactor()
        {
            var maxD = _vertices.Max(v => v.Radius) * 2;

            var width = _vertices.Max(v => v.ModelX) - _vertices.Min(v => v.ModelX) + maxD;
            var height = _vertices.Max(v => v.ModelY) - _vertices.Min(v => v.ModelY) + maxD;

            var scaleFactor = Math.Min(LayoutRoot.ActualWidth / width, LayoutRoot.ActualHeight / height);

            return scaleFactor;
        }

        /// <summary> Рассотяние между v1 и v2 </summary>
        private static double Distance(Vertex v1, Vertex v2)
        {
            return Math.Sqrt(Math.Pow(v1.ModelX - v2.ModelX, 2) + Math.Pow(v1.ModelY - v2.ModelY, 2));
        }

        /// <summary> Оперделяет, пересекаются ли вершины </summary>
        public static bool AreIntersecting(Vertex v1, Vertex v2)
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

                        var ex = (vi.ModelX - vj.ModelX)/distanceIj;
                        var ey = (vi.ModelY - vj.ModelY)/distanceIj;
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

                var newPositions = new Point[_vertices.Count];
                for (var i = 0; i < _vertices.Count; ++i)
                {
                    var vi = _vertices[i];
                    var deltaI = coordsDeltas[i];
                    newPositions[i].X = vi.ModelX + deltaI.X;
                    newPositions[i].Y = vi.ModelY + deltaI.Y;
                }

                var scaleFactor = GetScaleFactor();

                // Подравниваем, чтобы картинка оставалась в центре
                var minX = newPositions.Min(p => p.X) * scaleFactor;
                var minY = newPositions.Min(p => p.Y) * scaleFactor;
                var maxX = newPositions.Max(p => p.X) * scaleFactor;
                var maxY = newPositions.Max(p => p.Y) * scaleFactor;

                var graphCenter = new Point((maxX + minX)/2, (maxY + minY)/2);
                var layoutCenter = new Point(LayoutRoot.ActualWidth/2, LayoutRoot.ActualHeight/2);

                var deltaX = layoutCenter.X - graphCenter.X;
                var deltaY = layoutCenter.Y - graphCenter.Y;

                for (var i = 0; i < newPositions.Length; ++i)
                {
                    newPositions[i].X += deltaX / scaleFactor;
                    newPositions[i].Y += deltaY / scaleFactor;
                }

                // Запускаем анимацию
                for (var i = 0; i < _vertices.Count; ++i)
                {
                    var vi = _vertices[i];
                    if (vi.Equals(CapturedVertex))
                    {
                        continue;
                    }
                    var targetPositionI = newPositions[i];

                    var xiAnimation = SilverlightHelper
                        .GetStoryboard(vi, nameof(Vertex.ModelX), targetPositionI.X, ANIMATION_INTERVAL, null);
                    var yiAnimation = SilverlightHelper
                        .GetStoryboard(vi, nameof(Vertex.ModelY), targetPositionI.Y, ANIMATION_INTERVAL, null);
                    var scaleAnimation = SilverlightHelper
                        .GetStoryboard(vi, nameof(Vertex.ScaleFactor), scaleFactor, ANIMATION_INTERVAL, null);

                    xiAnimation.Begin();
                    yiAnimation.Begin();
                    scaleAnimation.Begin();
                }
            }
            _animationTimer.Start();
        }

        


        #endregion // Вычисление коориднат вершин


        #region Добавление рёбер

        /// <summary> Добавление рёбер </summary>
        public static readonly DependencyProperty IsEdgesAddingEnabledProperty =
            RegisterDependencyProperty(gv => gv.IsEdgesAddingEnabled, new PropertyMetadata(false));

        /// <summary> Включить добавление рёбер </summary>
        public bool IsEdgesAddingEnabled
        {
            get { return (bool) GetValue(IsEdgesAddingEnabledProperty); }
            set { SetValue(IsEdgesAddingEnabledProperty, value); }
        }

        private Vertex _source;
        private Arrow _arrow;

        private void CreateEdge(object sender, MouseButtonEventArgs args)
        {
            Contract.Assert(!args.Handled);
            Contract.Assert(_arrow == null);
            Contract.Assert(sender != null);
            Contract.Assert(sender is Vertex);

            if (!IsEdgesAddingEnabled) return;
            _source = (Vertex)sender;
            _arrow = new Arrow
            {
                X1 = _source.X,
                Y1 = _source.Y,
                X2 = _source.X,
                Y2 = _source.Y,
                Stroke = DefaultEdgeStroke,
                StrokeThickness = DefaultEdgeStrokeThickness
            };
            LayoutRoot.Children.Add(_arrow);
            LayoutRoot.MouseMove += DirectEdge;
            foreach (var v in LayoutRoot.Children)
            {
                v.MouseLeftButtonUp += ReleaseEdge;
            }
            MouseLeftButtonUp += ReleaseEdge;
        }

        private void DirectEdge(object sender, MouseEventArgs args)
        {
            Contract.Assert(_source != null);

            var mousePos = args.GetPosition(LayoutRoot);
            _arrow.X2 = mousePos.X;
            _arrow.Y2 = mousePos.Y;
        }

        private void ReleaseEdge(object sender, MouseButtonEventArgs args)
        {
            Contract.Assert(!args.Handled);
            Contract.Assert(sender != null);
            Contract.Assert(_source != null);

            var vertex = sender as Vertex;
            if (vertex != null)
            {
                var sourceName = _source.Name;
                var sinkName = vertex.Name;

                Graphs.Vertex source;
                Graphs.Vertex sink;
                IEdge newEdge;
                if (Graph is DirectedWeightedGraph)
                {
                    var g = (DirectedWeightedGraph)Graph;
                    source = g.Vertices.Single(v => v.Name == sourceName);
                    sink = g.Vertices.Single(v => v.Name == sinkName);

                    newEdge = new DirectedWeightedEdge(source, sink, 0); //todo нужно сделать вызов диалога задания веса
                }
                else if (Graph is DirectedGraph)
                {
                    var g = (DirectedGraph)Graph;
                    source = g.Vertices.Single(v => v.Name == sourceName);
                    sink = g.Vertices.Single(v => v.Name == sinkName);

                    newEdge = new DirectedEdge(source, sink);
                }
                else if (GetValue(GraphProperty) is UndirectedGraph)
                {
                    var g = (UndirectedGraph)Graph;
                    source = g.Vertices.Single(v => v.Name == sourceName);
                    sink = g.Vertices.Single(v => v.Name == sinkName);

                    newEdge = new UndirectedEdge(source, sink);
                }
                else
                {
                    throw new NotSupportedException("Неподдерживаемый тип графа");
                }

                var ed = Graph[source, sink];
                if (ed == null)
                    Graph.AddEdge(newEdge);
                else
                    Graph.RemoveEdge(ed);
            }

            LayoutRoot.Children.Remove(_arrow);
            _arrow = null;
            LayoutRoot.MouseMove -= DirectEdge;
            foreach (var v in LayoutRoot.Children) v.MouseLeftButtonUp -= ReleaseEdge;
            LayoutRoot.ReleaseMouseCapture();
        }

        #endregion //Добавление рёбер


        #region Перемещение вершин

        /// <summary> Разрешено ли перемещение вершин мышкой? </summary>
        public static readonly DependencyProperty IsMouseVerticesMovingEnabledProperty = DependencyProperty.Register(
            nameof(IsMouseVerticesMovingEnabled), 
            typeof(bool), 
            typeof(GraphVisualizer), 
            new PropertyMetadata(true));

        /// <summary> Разрешено ли перемещение вершин мышкой? </summary>
        public bool IsMouseVerticesMovingEnabled
        {
            get { return (bool) GetValue(IsMouseVerticesMovingEnabledProperty); }
            set { SetValue(IsMouseVerticesMovingEnabledProperty, value); }
        }

        private ThrottledMouseMoveEvent _moveEvent;

        /// <summary> Захваченная перемещаемая вершина </summary>
        protected Vertex CapturedVertex { get; private set; }
        private readonly object _capturedVertexLock = new object();


        private void InitVerticesMoving()
        {
            _moveEvent = new ThrottledMouseMoveEvent(LayoutRoot);
        }

        /// <summary> "Отпустить" перемещаемую вершину </summary>
        protected virtual void ReleaseVertex(object sender, MouseButtonEventArgs e)
        {
            lock (_capturedVertexLock)
            {
                if (CapturedVertex == null)
                {
                    return;
                }

                CapturedVertex = null;
                _moveEvent.MouseMove -= MouseMoveVertex;
                LayoutRoot.MouseLeftButtonUp -= ReleaseVertex;
                LayoutRoot.ReleaseMouseCapture();
            }
        }

        /// <summary> "Захватить" перемещаемую вершину </summary>
        private void CaptureVertex(object sender, MouseButtonEventArgs args)
        {
            Contract.Assert(!args.Handled);
            Contract.Assert(sender != null);
            Contract.Assert(sender is Vertex);

            if (IsMouseVerticesMovingEnabled)
            {
                lock (_capturedVertexLock)
                {
                    if (CapturedVertex != null)
                    {
                        return;
                    }

                    CapturedVertex = (Vertex)sender;
                    _moveEvent.MouseMove += MouseMoveVertex;
                    LayoutRoot.MouseLeftButtonUp += ReleaseVertex;
                    LayoutRoot.CaptureMouse();
                }
            }
            else
            {
                OnVertexClick((Vertex)sender);
            }
        }

        /// <summary> Перемещение вершины </summary>
        protected virtual void MouseMoveVertex(object sender, MouseEventArgs mouseEventArgs)
        {
            Contract.Assert(CapturedVertex != null);

            var position = mouseEventArgs.GetPosition(LayoutRoot);

            var radius = CapturedVertex.Radius;

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

            if (!AllowVerticesOverlap)
            {
                foreach (Vertex vertex in Vertices.Except(Enumerable.Repeat(CapturedVertex, 1)))
                {
                    var сurVerRadius = vertex.Radius;
                    var hyp = Math.Sqrt((position.X - vertex.X) * (position.X - vertex.X) +
                              (position.Y - vertex.Y) * (position.Y - vertex.Y));
                    if (!(hyp < radius + сurVerRadius)) continue;
                    var sin = hyp > 0 ? (position.X - vertex.X) / hyp : 0;
                    var cos = hyp > 0 ? (position.Y - vertex.Y) / hyp : 0;
                    position.X = vertex.X + (radius + сurVerRadius) * sin;
                    position.Y = vertex.Y + (radius + сurVerRadius) * cos;
                }
            }
            CapturedVertex.X = position.X;
            CapturedVertex.Y = position.Y;
        }

        #endregion // Перемещение вершин


        #region Constructors

        /// <summary> Конструктор </summary>
        public GraphVisualizer()
        {
            _animationTimer = null;
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
                            var vertex1 = Graph.Vertices.Single(e.Vertex1.Equals);
                            var vertex2 = Graph.Vertices.Single(e.Vertex2.Equals);
                            var newEdge = Directed
                                              ? (IEdge)new DirectedEdge((Graphs.Vertex)vertex1, (Graphs.Vertex)vertex2)
                                              : (IEdge)new UndirectedEdge((Graphs.Vertex)vertex1, (Graphs.Vertex)vertex2);

                            Graph.AddEdge(newEdge);
                        });
            }
            if (args.OldItems != null && args.Action != NotifyCollectionChangedAction.Add)
            {
                args.OldItems.Cast<Edge>().ForEach(e =>
                    {
                        var edge = Graph.Edges.Single(e.Equals);
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
                args.NewItems.Cast<Vertex>().ForEach(v => Graph.AddVertex(new Graphs.Vertex(v.Name)));
            }
            if (args.OldItems != null && args.Action != NotifyCollectionChangedAction.Add)
            {
                args.OldItems.Cast<Vertex>().ForEach(v =>
                    {
                        var vertex = Graph.Vertices.Single(v.Equals);
                        Graph.RemoveVertex(vertex);
                    });
            }

            _suspendNotifications = false;
        }

        #endregion // Edges&Vertices


        #region Implementation of IGraph

        /// <summary> Граф ориентированный? </summary>
        public bool Directed => Graph.Directed;

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        public bool AllowMultipleEdges => Graph.AllowMultipleEdges;

        /// <summary> Граф взвешенный? </summary>
        public bool Weighted => Graph.Weighted;

        /// <summary> Числов рёбер </summary>
        public int EdgesCount => _edges.Count;

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        ReadOnlyCollection<IEdge> IGraph.Edges => new ReadOnlyCollection<IEdge>(_edges.Cast<IEdge>().ToArray());

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        public ReadOnlyCollection<Edge> Edges => new ReadOnlyCollection<Edge>(_edges);

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public void AddEdge(IEdge edge)
        {
            var newEdge = new Edge
            {
                Vertex1 = _vertices.Single(edge.Vertex1.Equals),
                Vertex2 = _vertices.Single(edge.Vertex2.Equals),
                Directed = edge.Directed,
                Stroke = DefaultEdgeStroke,
                StrokeThickness = DefaultEdgeStrokeThickness,
                IsWeighted = edge.IsWeighted()
            };
            if (edge.Weight.HasValue)
            {
                newEdge.Weight = edge.Weight.Value;
            }
            _edges.Add(newEdge);
            LayoutRoot.Children.Add(newEdge);
            newEdge.MouseLeftButtonDown += (sender, args) => OnEdgeClick((Edge)sender);
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        public void RemoveEdge(IEdge edge)
        {
            RemoveEdge((Edge)edge);
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        IEdge IGraph.this[IVertex v1, IVertex v2] => this[(Vertex)v1, (Vertex)v2];

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public void AddEdge(Edge edge)
        {
            AddEdge((IEdge)edge);
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        public void RemoveEdge(Edge edge)
        {
            var toRemove = (Edge)edge;
            _edges.Remove(toRemove);
            LayoutRoot.Children.Remove(toRemove);
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        public Edge this[Vertex v1, Vertex v2]
        {
            get
            {
                var toFind = new Edge { Vertex1 = v1, Vertex2 = v2, Directed = this.Directed };
                return _edges.FirstOrDefault(toFind.Equals);
            }
        }

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        public ReadOnlyCollection<Vertex> Vertices => new ReadOnlyCollection<Vertex>(_vertices);

        /// <summary> Числов вершин </summary>
        public int VerticesCount => _vertices.Count;

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        ReadOnlyCollection<IVertex> IGraph.Vertices => new ReadOnlyCollection<IVertex>(_vertices.Cast<IVertex>().ToList());

        /// <summary> Добавляет вершину vertex в граф </summary>
        /// <remarks> Для добавления вершины на конкретную позицию используйте AddVertex(Vertex) </remarks>
        public void AddVertex(IVertex vertex)
        {
            var newVertex = new Vertex(vertex)
            {
                BorderThickness = new Thickness(DefaultVertexBorderThickness),
                BorderBrush = DefaultVertexBorderBrush,
                Background = DefaultVertexBackground,
                Style = DefaultVertexStyle,
                Radius = DefaultVertexRadius,
            };

            newVertex.MouseLeftButtonDown += CaptureVertex;
            newVertex.MouseLeftButtonDown += CreateEdge;
            _vertices.Add(newVertex);
            LayoutRoot.Children.Add(newVertex);
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        public void RemoveVertex(IVertex vertex)
        {
            RemoveVertex((Vertex)vertex);
        }

        /// <summary> Добавляет вершину vertex в граф </summary>
        public void AddVertex(Vertex vertex)
        {
            // Когда добавляется вершина, с уже установленными координатами, нужно пнуть байдинги, чтобы они
            // сработали, когда мы добавим вершину на новый канвас. Сами они почему-то как-то криво работают.
            // Чтобы пнуть, просто меняем координаты на нулевые и после добавления возвращаем обратно.
            var x = vertex.ModelX;
            var y = vertex.ModelY;
            vertex.ModelX = 0;
            vertex.ModelY = 0;
            _vertices.Add(vertex);
            vertex.MouseLeftButtonDown += CaptureVertex;
            vertex.MouseLeftButtonDown += CreateEdge;
            LayoutRoot.Children.Add(vertex);
            vertex.ModelX = x;
            vertex.ModelY = y;
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        public void RemoveVertex(Vertex vertex)
        {
            _edges.Where(e => e.IsIncidentTo(vertex)).ForEach(RemoveEdge);
            var toRemove = (Vertex)vertex;
            _vertices.Remove(toRemove);
            LayoutRoot.Children.Remove(toRemove);
        }

        #endregion
    }
}
