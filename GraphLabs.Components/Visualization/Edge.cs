using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphLabs.Core;
using GraphLabs.Core.Helpers;

namespace GraphLabs.Components.Visualization
{
    /// <summary> Контрол-ребро </summary>
    public sealed class Edge : Control, IEdge<Vertex>
    {
        #region Вершины

        /// <summary> Вершина 1 (исток) </summary>
        public static DependencyProperty Vertex1Property = DependencyProperty.Register(
            "Vertex1",
            typeof(Vertex),
            typeof(Edge),
            new PropertyMetadata(Vertex1Changed));

        /// <summary> Вершина 2 (сток) </summary>
        public static DependencyProperty Vertex2Property = DependencyProperty.Register(
            "Vertex2",
            typeof(Vertex),
            typeof(Edge),
            new PropertyMetadata(Vertex2Changed));

        /// <summary> Вершина 1 (исток) </summary>
        public Vertex Vertex1
        {
            get
            {
                return (Vertex)GetValue(Vertex1Property);
            }
            set
            {
                SetValue(Vertex1Property, value);
            }
        }

        /// <summary> Индекс вершины 2 (вершины-стока) </summary>
        IVertex IEdge.Vertex2 
        {
            get { return Vertex2; }
        }

        /// <summary> Индекс вершины 1 (вершины-истока) </summary>
        IVertex IEdge.Vertex1 
        {
            get { return Vertex1; }
        }

        /// <summary> Вершина 2 (сток) </summary>
        public Vertex Vertex2
        {
            get
            {
                return (Vertex)GetValue(Vertex2Property);
            }
            set
            {
                SetValue(Vertex2Property, value);
            }
        }

        /// <summary> Callback на изменение вершины 1 </summary>
        private static void Vertex1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var edge = (Edge)d;
            edge.SetBinding(X1Property, new Binding
            {
                Path = new PropertyPath("X"),
                Source = e.NewValue
            });
            edge.SetBinding(Y1Property, new Binding
            {
                Path = new PropertyPath("Y"),
                Source = e.NewValue
            });

            edge.UpdateZIndex();
            edge.UpdateComponentsPosition();
        }

        /// <summary> Callback на изменение вершины 2 </summary>
        private static void Vertex2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var edge = (Edge)d;
            edge.SetBinding(X2Property, new Binding
            {
                Path = new PropertyPath("X"),
                Source = e.NewValue
            });
            edge.SetBinding(Y2Property, new Binding
            {
                Path = new PropertyPath("Y"),
                Source = e.NewValue
            });

            edge.UpdateZIndex();
            edge.UpdateComponentsPosition();
        }

        #endregion // Вершины


        #region Всякие свойства

        /// <summary> Ребро взвешенное? </summary>
        public static readonly DependencyProperty IsWeightedProperty =
            DependencyProperty.Register("IsWeighted", typeof(bool), typeof(Edge), new PropertyMetadata(default(bool)));

        /// <summary> Ребро взвешенное? </summary>
        public bool IsWeighted
        {
            get { return (bool)GetValue(IsWeightedProperty); }
            set { SetValue(IsWeightedProperty, value); }
        }

        /// <summary> Вес </summary>
        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(int), typeof(Edge), new PropertyMetadata(default(int)));

        /// <summary> Вес </summary>
        public int Weight
        {
            get { return (int)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        /// <summary> Ребро ориентированное? </summary>
        public static DependencyProperty DirectedProperty = DependencyProperty.Register(
            "Directed",
            typeof(bool),
            typeof(Edge),
            new PropertyMetadata(false, DirectedChanged));

        /// <summary> Callback на изменение свойства направленности </summary>
        private static void DirectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Edge)d).UpdateComponentsPosition();
        }

        /// <summary> Ребро ориентированное? </summary>
        public bool Directed
        {
            get
            {
                return (bool)GetValue(DirectedProperty);
            }
            set
            {
                SetValue(DirectedProperty, value);
            }
        }

        #endregion // Ориентированность


        #region Координаты

        /// <summary> Координата X вершины-истока </summary>
        public static DependencyProperty X1Property = 
            DependencyProperty.Register(
                "X1", 
                typeof(double), 
                typeof(Edge), 
                new PropertyMetadata(
                    (d, e) => ((Edge)d).UpdateComponentsPosition()
                ));


        /// <summary> Координата Y вершины-истока </summary>
        public static DependencyProperty Y1Property = 
            DependencyProperty.Register(
                "Y1", 
                typeof(double), 
                typeof(Edge), 
                new PropertyMetadata(
                    (d, e) => ((Edge)d).UpdateComponentsPosition()
                ));

        /// <summary> Координата X вершины-стока </summary>
        public static DependencyProperty X2Property = 
            DependencyProperty.Register(
                "X2", 
                typeof(double), 
                typeof(Edge), 
                new PropertyMetadata(
                    (d, e) => ((Edge)d).UpdateComponentsPosition()
                ));

        /// <summary> Координата Y вершины-стока </summary>
        public static DependencyProperty Y2Property = 
            DependencyProperty.Register(
                "Y2", 
                typeof(double), 
                typeof(Edge), 
                new PropertyMetadata(
                    (d, e) => ((Edge)d).UpdateComponentsPosition()
                ));

        /// <summary> X-кооридната метки веса </summary>
        public static readonly DependencyProperty WeightXProperty =
            DependencyProperty.Register("WeightX", typeof(double), typeof(Edge), new PropertyMetadata(default(double)));

        /// <summary> Y-кооридната метки веса </summary>
        public static readonly DependencyProperty WeightYProperty =
            DependencyProperty.Register("WeightY", typeof(double), typeof(Edge), new PropertyMetadata(default(double)));

        /// <summary> X-кооридната метки веса </summary>
        public double WeightX
        {
            get { return (double)GetValue(WeightXProperty); }
            set { SetValue(WeightXProperty, value); }
        }

        /// <summary> Y-кооридната метки веса </summary>
        public double WeightY
        {
            get { return (double)GetValue(WeightYProperty); }
            set { SetValue(WeightYProperty, value); }
        }

        /// <summary> Координата X вершины-истока </summary>    
        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        /// <summary> Координата Y вершины-истока </summary>
        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        /// <summary> Координата X вершины-стока </summary>
        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        /// <summary> Координата Y вершины-стока </summary>
        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }


        #endregion // Координаты


        #region Внешний вид

        // ReSharper disable InconsistentNaming
        private static readonly Brush DEFAULT_COLOR_BRUSH = new SolidColorBrush(Colors.Black);
        private const double DEFAULT_THICKNESS = 1.0;
        // ReSharper restore InconsistentNaming

        /// <summary> Цвет </summary>
        public static DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                "Stroke", 
                typeof(Brush), 
                typeof(Edge), 
                new PropertyMetadata(DEFAULT_COLOR_BRUSH));

        /// <summary> Цвет </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary> Толщина </summary>
        public static DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", 
                typeof(double), 
                typeof(Edge),
                new PropertyMetadata(DEFAULT_THICKNESS));

        /// <summary> Толщина </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        private Polygon _triangle;

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes 
        /// (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. 
        /// In simplest terms, this means the method is called just before a UI element displays in an application. 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _triangle = GetTemplateChild("PART_TRIANGLE") as Polygon;

            UpdateComponentsPosition();
        }

        [Obsolete("У ребра бэкграунда не должно быть.")]
        private new Brush Background
        {
            get { return base.Background; }
            set { base.Background = value; }
        }

        #endregion // Внешний вид


        #region Вспомагательные методы

        /// <summary> Обновляет ZIndex ребра таким образом, чтобы оно не рисовалось поверх вершин </summary>
        private void UpdateZIndex()
        {
            var vertex1ZIndex = Vertex1 != null
                                    ? Canvas.GetZIndex((UIElement)Vertex1)
                                    : 0;
            var vertex2ZIndex = Vertex2 != null
                                    ? Canvas.GetZIndex((UIElement)Vertex2)
                                    : 0;
            Canvas.SetZIndex(this, Math.Min(vertex1ZIndex, vertex2ZIndex) - 1);
        }

        /// <summary> Считает координаты углов триугольника-стрелки и возвращает результат в виде коллекции </summary>
        private IEnumerable<Point> CalculateArrowCoords()
        {
            var v = (Vertex)Vertex2;

            double c1, d1 , c2, d2, a, b;

            // "Острота" стрелки
            const double beta = Math.PI / 10;

            // Длина стрелки
            const double l = 10;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if ((X2 - X1) != 0)
            {
                var alfa = Math.Atan(Math.Abs(Y2 - Y1) / Math.Abs(X2 - X1));
                if (X2 > X1 && Y1 > Y2)
                {
                    a = X2 - v.Radius * Math.Cos(alfa);
                    b = Y2 + v.Radius * Math.Sin(alfa);
                    c1 = a - l * Math.Cos(alfa - beta);
                    d1 = b + l * Math.Sin(alfa - beta);
                    c2 = a - l * Math.Sin(Math.PI / 2 - alfa - beta);
                    d2 = b + l * Math.Cos(Math.PI / 2 - alfa - beta);
                }
                else if (X2 < X1 && Y2 < Y1)
                {
                    a = X2 + v.Radius * Math.Cos(alfa);
                    b = Y2 + v.Radius * Math.Sin(alfa);
                    c1 = a + l * Math.Sin(Math.PI / 2 - alfa - beta);
                    d1 = b + l * Math.Cos(Math.PI / 2 - alfa - beta);
                    c2 = a + l * Math.Cos(alfa - beta);
                    d2 = b + l * Math.Sin(alfa - beta);
                }
                else if (X2 < X1 && Y2 > Y1)
                {
                    a = X2 + v.Radius * Math.Cos(alfa);
                    b = Y2 - v.Radius * Math.Sin(alfa);
                    c1 = a + l * Math.Cos(alfa - beta);
                    d1 = b - l * Math.Sin(alfa - beta);
                    c2 = a + l * Math.Sin(Math.PI / 2 - alfa - beta);
                    d2 = b - l * Math.Cos(Math.PI / 2 - alfa - beta);
                }
                else if (X2 > X1 && Y1 < Y2)
                {
                    a = X2 - v.Radius * Math.Cos(alfa);
                    b = Y2 - v.Radius * Math.Sin(alfa);
                    c1 = a - l * Math.Sin(Math.PI / 2 - alfa - beta);
                    d1 = b - l * Math.Cos(Math.PI / 2 - alfa - beta);
                    c2 = a - l * Math.Cos(alfa - beta);
                    d2 = b - l * Math.Sin(alfa - beta);
                }
                else if (X2 < X1 && Y2 == Y1)
                {
                    a = X2 + v.Radius;
                    b = Y1;
                    c1 = a + l * Math.Cos(beta);
                    c2 = a + l * Math.Cos(beta);
                    d1 = b + l * Math.Sin(beta);
                    d2 = b - l * Math.Sin(beta);
                }
                else if (X2 > X1 && Y2 == Y1)
                {
                    a = X2 - v.Radius;
                    b = Y1;
                    c1 = a - l * Math.Cos(beta);
                    c2 = a - l * Math.Cos(beta);
                    d1 = b - l * Math.Sin(beta);
                    d2 = b + l * Math.Sin(beta);
                }
                else
                {
                    a = b = c1 = c2 = d1 = d2 = X1;
                }
            }
            else
            {
                if (X2 == X1 && Y2 > Y1)
                {
                    a = X1;
                    b = Y2 - v.Radius;
                    c1 = a + l * Math.Sin(beta);
                    c2 = a - l * Math.Sin(beta);
                    d1 = b - l * Math.Cos(beta);
                    d2 = b - l * Math.Cos(beta);
                }
                else if (X2 == X1 && Y2 < Y1)
                {
                    a = X1;
                    b = Y2 + v.Radius;
                    c1 = a - l * Math.Sin(beta);
                    c2 = a + l * Math.Sin(beta);
                    d1 = b + l * Math.Cos(beta);
                    d2 = b + l * Math.Cos(beta);
                }
                else
                {
                    a = b = c1 = c2 = d1 = d2 = X1;
                }
            }
            return new[]
                       {
                           new Point(a, b),
                           new Point(c1, d1),
                           new Point(c2, d2),
                       };
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary> Обновляет положение треугольника-стрелки </summary>
        private void UpdateComponentsPosition()
        {
            if (!Directed && !IsWeighted || _triangle == null || Vertex1 == null || Vertex2 == null)
            {
                return;
            }

            if (Directed)
            {
                _triangle.Points.Clear();
                CalculateArrowCoords().ForEach(p => _triangle.Points.Add(p));
            }

            if (IsWeighted)
            {
                WeightX = (X1 + X2)/2;
                WeightY = (Y1 + Y2)/2;
            }
        }

        #endregion // Вспомагательные методы


        /// <summary> Конструктор </summary>
        public Edge()
        {
            DefaultStyleKey = typeof(Edge);
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            return string.Format(Directed ? "{0}->{1}" : "{0}--{1}", Vertex1, Vertex2);

        }

        /// <summary> Сравнение рёбер </summary>
        public bool Equals(IEdge other)
        {
            return other.Vertex1.Equals(Vertex1) && other.Vertex2.Equals(Vertex2) && other.Directed == Directed;
        }

        /// <summary> Сравниваем </summary>
        public override bool Equals(object obj)
        {
            var e = obj as IEdge;
            return e != null && Equals(e);
        }

        /// <summary> GetHashCode </summary>
        public override int GetHashCode()
        {
            return Vertex1.GetHashCode() ^ Vertex2.GetHashCode() ^ Directed.GetHashCode();
        }

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        /// <remarks> Не в этот раз, детка </remarks>
        public object Clone()
        {
            throw new NotSupportedException();
        }
    }
}
