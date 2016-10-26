using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using GraphLabs.CommonUI.Controls.ViewModels;

namespace GraphLabs.CommonUI.Controls
{
    /// <summary> Матрица-переключательница </summary>
    public class SwitchMatrix<T> : Matrix
    {
        private T[] _values;

        /// <summary> Значения в ячейках. Переключаются циклично </summary>
        public T[] Values
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
            Values = new T[0];
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

                var rowVm = cell.DataContext as MatrixRowViewModel<T>;
                if (rowVm == null)
                    throw new Exception("Не найден дата-контекст ячейки типа MatrixRowViewModel<T>");

                //аццкая жесть - сильверлайт такой сильверлайт =(
                var colIdx = int.Parse(
                    textBlock.GetBindingExpression(TextBlock.TextProperty)
                        .ParentBinding.Path.Path
                        .Replace("[", "")
                        .Replace("]", ""));
                
                if (Values.Length == 0)
                {
                    rowVm[colIdx] = default(T);
                }

                var currentValue = rowVm[colIdx];
                var newValueIndex = Array.IndexOf(Values, currentValue) + 1;
                if (newValueIndex >= Values.Length)
                    newValueIndex = 0;

                rowVm[colIdx] = Values[newValueIndex];
                textBlock.Text = Values[newValueIndex].ToString();
            }
        }
    }
}