using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphLabs.Utils;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary> Контрол-стрелка (направленное ребро графа) </summary>
    public sealed class Arrow : Control
    {
        #region Координаты

        /// <summary> Координата X вершины-истока </summary>
        public static DependencyProperty X1Property =
            DependencyProperty.Register(
                "X1",
                typeof(double),
                typeof(Arrow),
                new PropertyMetadata(
                    (d, e) => ((Arrow)d).UpdateComponentsPosition()
                ));


        /// <summary> Координата Y вершины-истока </summary>
        public static DependencyProperty Y1Property =
            DependencyProperty.Register(
                "Y1",
                typeof(double),
                typeof(Arrow),
                new PropertyMetadata(
                    (d, e) => ((Arrow)d).UpdateComponentsPosition()
                ));

        /// <summary> Координата X конечная </summary>
        public static DependencyProperty X2Property =
            DependencyProperty.Register(
                "X2",
                typeof(double),
                typeof(Arrow),
                new PropertyMetadata(
                    (d, e) => ((Arrow)d).UpdateComponentsPosition()
                ));

        /// <summary> Координата Y конечная </summary>
        public static DependencyProperty Y2Property =
            DependencyProperty.Register(
                "Y2",
                typeof(double),
                typeof(Arrow),
                new PropertyMetadata(
                    (d, e) => ((Arrow)d).UpdateComponentsPosition()
                ));

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

        /// <summary> Координата X конечная </summary>
        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        /// <summary> Координата Y конечная </summary>
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
                typeof(Arrow),
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
                typeof(Arrow),
                new PropertyMetadata(DEFAULT_THICKNESS));

        /// <summary> Толщина </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        private Polygon _triangle;

        /// <summary> Применение шаблона </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _triangle = GetTemplateChild("PART_TRIANGLE") as Polygon;

            UpdateComponentsPosition();
        }

        #endregion // Внешний вид

        private IEnumerable<Point> CalculateArrowCoords()
        {
            double c1, d1, c2, d2, a, b;

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
                    a = X2;
                    b = Y2;
                    c1 = a - l * Math.Cos(alfa - beta);
                    d1 = b + l * Math.Sin(alfa - beta);
                    c2 = a - l * Math.Sin(Math.PI / 2 - alfa - beta);
                    d2 = b + l * Math.Cos(Math.PI / 2 - alfa - beta);
                }
                else if (X2 < X1 && Y2 < Y1)
                {
                    a = X2;
                    b = Y2;
                    c1 = a + l * Math.Sin(Math.PI / 2 - alfa - beta);
                    d1 = b + l * Math.Cos(Math.PI / 2 - alfa - beta);
                    c2 = a + l * Math.Cos(alfa - beta);
                    d2 = b + l * Math.Sin(alfa - beta);
                }
                else if (X2 < X1 && Y2 > Y1)
                {
                    a = X2;
                    b = Y2;
                    c1 = a + l * Math.Cos(alfa - beta);
                    d1 = b - l * Math.Sin(alfa - beta);
                    c2 = a + l * Math.Sin(Math.PI / 2 - alfa - beta);
                    d2 = b - l * Math.Cos(Math.PI / 2 - alfa - beta);
                }
                else if (X2 > X1 && Y1 < Y2)
                {
                    a = X2;
                    b = Y2;
                    c1 = a - l * Math.Sin(Math.PI / 2 - alfa - beta);
                    d1 = b - l * Math.Cos(Math.PI / 2 - alfa - beta);
                    c2 = a - l * Math.Cos(alfa - beta);
                    d2 = b - l * Math.Sin(alfa - beta);
                }
                else if (X2 < X1 && Y2 == Y1)
                {
                    a = X2;
                    b = Y1;
                    c1 = a + l * Math.Cos(beta);
                    c2 = a + l * Math.Cos(beta);
                    d1 = b + l * Math.Sin(beta);
                    d2 = b - l * Math.Sin(beta);
                }
                else if (X2 > X1 && Y2 == Y1)
                {
                    a = X2;
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
                    b = Y2;
                    c1 = a + l * Math.Sin(beta);
                    c2 = a - l * Math.Sin(beta);
                    d1 = b - l * Math.Cos(beta);
                    d2 = b - l * Math.Cos(beta);
                }
                else if (X2 == X1 && Y2 < Y1)
                {
                    a = X1;
                    b = Y2;
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

        private void UpdateComponentsPosition()
        {
            if (_triangle == null) return;
            _triangle.Points.Clear();
            CalculateArrowCoords().ForEach(p => _triangle.Points.Add(p));
        }

        /// <summary> Обновляет ZIndex стрелки таким образом, чтобы она не рисовалось поверх вершин </summary>
        private void UpdateZIndex()
        {
            Canvas.SetZIndex(this, -500);
        }

        /// <summary> Конструктор </summary>
        public Arrow()
        {
            DefaultStyleKey = typeof(Arrow);
            UpdateZIndex();
        }
    }
}
