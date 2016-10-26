using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphLabs.CommonUI.Controls
{
    /// <summary> Матрица-переключательница </summary>
    public class SwitchMatrix : Matrix
    {
        private string[] _values;

        /// <summary> Значения в ячейках. Переключаются циклично </summary>
        public string[] Values
        {
            get { return _values; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (value.Distinct().Count() != value.Length)
                    throw new ArgumentException("Массив значений не должен содержать повторяющихся элементов.");
                _values = value;
            }
        }

        /// <summary> Матрица-переключательница </summary>
        public SwitchMatrix()
        {
            MatrixGrid.IsReadOnly = true;
            MatrixGrid.MouseLeftButtonUp += OnMouseLeftButtonUp;
            Values = new string[0];
        }

        private void OnMouseLeftButtonUp(object sender,
            MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // iteratively traverse the visual tree
            while ((dep != null) &&
                   !(dep is DataGridCell) &&
                   !(dep is DataGridColumnHeader))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            if (dep is DataGridCell)
            {
                DataGridCell cell = dep as DataGridCell;
                var textBlock = cell.Content as TextBlock;
                if (textBlock == null)
                    throw new Exception("Какая-то неправильная ячейка");

                if (Values.Length == 0)
                {
                    textBlock.Text = string.Empty;
                }

                var currentValue = textBlock.Text;
                var newValueIndex = Array.IndexOf(Values, currentValue) + 1;
                if (newValueIndex >= Values.Length)
                    newValueIndex = 0;

                textBlock.Text = Values[newValueIndex];
            }
        }
    }
}