using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GraphLabs.CommonUI.Controls.ViewModels.Matrix
{
    /// <summary> ViewModel матрицы </summary>
    /// <typeparam name="T"></typeparam>
    public class MatrixViewModel<T> : DependencyObject
    {
        private CellViewModel<T>[,] _matrix;

        /// <summary> Вьюмодели ячеек </summary>
        public ReadOnlyCollection<ReadOnlyCollection<CellViewModel<T>>> Matrix;

        /// <summary> Заголовочная колонка </summary>
        public readonly MatrixColumnViewModel<T> HeaderColumn;

        /// <summary> Заголовочная строка </summary>
        public readonly MatrixRowViewModel<T> HeaderRow;

        /// <summary> Число колонок </summary>
        public uint ColsCount
        {
            get; private set; 
        }

        /// <summary> Число строк </summary>
        public uint RowsCount
        {
            get; private set; 
        }

        /// <summary> Фон </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", 
            typeof (Brush), 
            typeof (MatrixViewModel<T>), 
            new PropertyMetadata(default(Brush))
        );
        
        /// <summary> Фон </summary>
        public Brush Background
        {
            get { return (Brush) GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary> Изменить размерность </summary>
        public void ReSize(uint rows, uint cols)
        {
            var newMatrix = new CellViewModel<T>[cols, rows];

            for (int i = 0; i < Math.Min(cols,ColsCount); i++)
            {
                for (int j = 0; j < Math.Min(rows,RowsCount); j++)
                {
                    newMatrix[i, j] = _matrix[i, j];
                } 
            }

            _matrix = newMatrix;

            ColsCount = cols;
            RowsCount = rows;

            HeaderColumn.ReSize(ColsCount);
            HeaderRow.ReSize(RowsCount);
        }

        /// <summary> Очистить </summary>
        public void Сlear()
        {
            _matrix = new CellViewModel<T>[ColsCount,RowsCount];
        }
    }
}
