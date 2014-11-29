using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GraphLabs.CommonUI.Helpers;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary> Контрол-вершина </summary>
    public sealed class Vertex : Control, IVertex
    {
        #region Size

        private const double DEFAULT_VERTEX_RADIUS = Double.NaN;

        /// <summary> Радиус вершины </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                "Radius", 
                typeof(double), 
                typeof(Vertex), 
                new PropertyMetadata(DEFAULT_VERTEX_RADIUS, RadiusChanged));

        /// <summary> Callback на изменение радиуса </summary>
        /// <remarks> 
        /// При изменении радиуса меняет положение вершины таким образом,
        /// чтобы её центр остался на прежнем месте.
        /// </remarks>
        private static void RadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vertex = (Vertex)d;
            var oldLeft = (double)d.GetValue(Canvas.LeftProperty);
            var oldTop = (double)d.GetValue(Canvas.TopProperty);
            var delta = (double)e.NewValue - (double)e.OldValue;
            delta = !double.IsNaN(delta) ? delta : 0.0;

            vertex.SetValue(Canvas.LeftProperty, oldLeft - delta);
            vertex.SetValue(Canvas.TopProperty, oldTop - delta);
        }

        /// <summary> Радиус вершины </summary>
        public double Radius
        {
            get
            {
                return (double)GetValue(RadiusProperty);
            }
            set
            {
                SetValue(RadiusProperty, value);
            }
        }

        /// <summary> Длина </summary>
        [Obsolete("Используйте Radius")]
        public new double Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }


        /// <summary> Ширина </summary>
        [Obsolete("Используйте Radius")]
        public new double Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        private UIElement _label;
        private UIElement _comments;

        /// <summary> Возвращает предпочитаемый радиус вершины </summary>
        public double GetDesiredRadius()
        {
            const double DESIRED_DELTA = 5.0;
            var nameSize = SilverlightHelper.GetTextSize(Name);
            var desiredRadius = Math.Max(nameSize.Width, nameSize.Height)/2 + DESIRED_DELTA;

            return desiredRadius;
        }

        #endregion // Size


        #region Coords

        /// <summary> Коэффициент масштабирования </summary>
        public static readonly DependencyProperty ScaleFactorProperty =
            DependencyProperty.Register(
                "ScaleFactor",
                typeof(double),
                typeof(Vertex),
                new PropertyMetadata((double)1, UpdatePosition));

        /// <summary> Подпись над графом </summary>
         public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Vertex), null);

        
        /// <summary> текст </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
       } 

        private static void UpdatePosition(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var v = (Vertex)d;
            v.X = v.X / (double)e.NewValue;
            v.Y = v.Y / (double)e.NewValue;
        }

        /// <summary> Коэффициент масштабирования </summary>
        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        /// <summary> Координата X центра вершины </summary>
        public static DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Vertex), null);

        /// <summary> Координата Y центра вершины </summary>
        public static DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Vertex), null);

        /// <summary> Координата X центра вершины </summary>
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        /// <summary> Координата Y центра вершины </summary>
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        /// <summary> Модельная координата X центра вершины </summary>
        public static readonly DependencyProperty ModelXProperty =
            DependencyProperty.Register("ModelX", typeof(double), typeof(Vertex), null);

        /// <summary> Модельная координата X центра вершины </summary>
        public double ModelX
        {
            get { return (double)GetValue(ModelXProperty); }
            set { SetValue(ModelXProperty, value); }
        }

        /// <summary> Модельная координата Y центра вершины </summary>
        public static readonly DependencyProperty ModelYProperty =
            DependencyProperty.Register("ModelY", typeof(double), typeof(Vertex), null);
        
        /// <summary> Модельная координата Y центра вершины </summary>
        public double ModelY
        {
            get { return (double)GetValue(ModelYProperty); }
            set { SetValue(ModelYProperty, value); }
        }

        #endregion // Coords


        #region Constructors

        /// <summary> Устанавливает привязки </summary>
        private void InitBindings()
        {
            var radiusToWidthHeightConverter = new RadiusToWidthHeightConverter();
            var radiusPath = new PropertyPath("Radius");

            SetBinding(WidthProperty, new Binding
            {
                Path = radiusPath,
                Source = this,
                Mode = BindingMode.TwoWay,
                Converter = radiusToWidthHeightConverter
            });
            SetBinding(HeightProperty, new Binding
            {
                Path = radiusPath,
                Source = this,
                Mode = BindingMode.TwoWay,
                Converter = radiusToWidthHeightConverter
            });

            var coordsConverter = new CoordsConverter(() => Radius);
            SetBinding(Canvas.LeftProperty, new Binding
            {
                Path = new PropertyPath("X"),
                Source = this,
                Mode = BindingMode.TwoWay,
                Converter = coordsConverter,
            });
            SetBinding(Canvas.TopProperty, new Binding
            {
                Path = new PropertyPath("Y"),
                Source = this,
                Mode = BindingMode.TwoWay,
                Converter = coordsConverter,
            });

            var modelCoordsConverter = new ModelCoordsConverter(() => ScaleFactor);
            SetBinding(XProperty, new Binding
            {
                Path = new PropertyPath("ModelX"),
                Source = this,
                Mode = BindingMode.TwoWay,
                Converter = modelCoordsConverter,
            });
            SetBinding(YProperty, new Binding
            {
                Path = new PropertyPath("ModelY"),
                Source = this,
                Mode = BindingMode.TwoWay,
                Converter = modelCoordsConverter,
            });
        }

        /// <summary> Конструктор по-умолчанию </summary>
        public Vertex()
        {
            DefaultStyleKey = typeof(Vertex);
            InitBindings();
        }

        #endregion // Constructors


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary> When overridden in a derived class, is invoked whenever application code or internal processes 
        /// (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. 
        /// In simplest terms, this means the method is called just before a UI element displays in an application. 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _label = GetTemplateChild("PART_LABEL") as UIElement;
            _comments = GetTemplateChild("COMMENTS") as UIElement;
        }


        #region Converters

        /// <summary> Конвертер из радиуса в длину/ширину </summary>
        public class RadiusToWidthHeightConverter : IValueConverter
        {
            /// <summary> Из радиуса в длину/ширину </summary>
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (double)value * 2;
            }

            /// <summary> Из длины/ширины в радиус </summary>
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (double)value / 2;
            }
        }

        /// <summary> Из координат центра вершины в координаты её верхнего левого угла </summary>
        public class CoordsConverter : IValueConverter
        {
            private readonly Func<double> _radiusAccessor;

            /// <summary> Ctor. </summary>
            /// <param name="radiusAccessor"> Функция, возвращающая радиус вершины </param>
            public CoordsConverter(Func<double> radiusAccessor)
            {
                _radiusAccessor = radiusAccessor;
            }

            /// <summary> Из центра в угол </summary>
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var result = (double)value - _radiusAccessor();
                return !double.IsNaN(result) ? result : 0.0;
            }

            /// <summary> Из угла в центр </summary>
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var result = (double)value + _radiusAccessor();
                return !double.IsNaN(result) ? result : 0.0;
            }
        }

        /// <summary> Из модельных координат в реальные </summary>
        public class ModelCoordsConverter : IValueConverter
        {
            private readonly Func<double> _scaleAccessor;

            /// <summary> Ctor. </summary>
            /// <param name="scaleAccessor"> Функция, возвращающая радиус вершины </param>
            public ModelCoordsConverter(Func<double> scaleAccessor)
            {
                _scaleAccessor = scaleAccessor;
            }

            /// <summary> Из модельных в реальные </summary>
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var result = (double)value * _scaleAccessor();
                return !double.IsNaN(result) ? result : 0.0;
            }

            /// <summary> Из реальных в модельные </summary>
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var result = (double)value / _scaleAccessor();
                return !double.IsNaN(result) ? result : 0.0;
            }
        }

        #endregion

        /// <summary> Переименовать </summary>
        public Vertex Rename(string newName)
        {
            return (Vertex)((IVertex)this).Rename(newName);
        }

        /// <summary> Переименовать вершину </summary>
        IVertex IVertex.Rename(string newName)
        {
            Name = newName;
            return this;
        }

        /// <summary> GetHashCode </summary>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        /// <remarks> Не в этой жизни </remarks>
        public object Clone()
        {
            throw new NotSupportedException();
        }

        #region Сравнение

        /// <summary> Сравнение вершин </summary>
        public bool Equals(IVertex other)
        {
            return Graphs.ValueEqualityComparer.VerticesEquals(this, other);
        }

        /// <summary> Сравниваем </summary>
        public override bool Equals(object obj)
        {
            var v = obj as IVertex;
            return Equals(v);
        }


        #endregion
    }
}
