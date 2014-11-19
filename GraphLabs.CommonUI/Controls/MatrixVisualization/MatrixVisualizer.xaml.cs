using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using GraphLabs.CommonUI.Controls.ViewModels.Matrix;

namespace GraphLabs.CommonUI.Controls.MatrixVisualization
{
    /// <summary> Матрица смежности </summary>
    public partial class MatrixVisualizer : UserControl
    {
        #region Внешность

        private static readonly Color DEFAULT_MATRIX_BACKGROUND_COLOR = Colors.LightGray;
        private static readonly Color DEFAULT_CELL_BORDER_COLOR = Colors.Black;
        private static readonly Color DEFAULT_HEADER_BACKGROUND_COLOR = Color.FromArgb(80, 100, 150, 150);

        /*public Brush DefaultMatrixBackground
        {
            get { return (Brush)GetValue(DefaultMatrixBackgroundProperty); }
            set { SetValue(DefaultMatrixBackgroundProperty, value); }
        }

        public static DependencyProperty DefaultMatrixBackgroundProperty = DependencyProperty.Register
        (
            "DefaultMatrixBackground",
            typeof(Brush),
            typeof(MatrixVisualizer),
            new PropertyMetadata(new SolidColorBrush(DEFAULT_MATRIX_BACKGROUND_COLOR), )
        );*/

        #endregion Внешность
    }
}
