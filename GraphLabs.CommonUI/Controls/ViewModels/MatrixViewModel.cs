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

namespace GraphLabs.CommonUI.Controls.ViewModels
{
    public class MatrixViewModel<T> : DependencyObject
    {
        private CellViewModel<T>[,] _matrix;

        public ReadOnlyCollection<ReadOnlyCollection<CellViewModel<T>>> Matrix;

        public uint ColsCount
        {
            get; private set; 
        }

        public uint RowsCount
        {
            get; private set; 
        }

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", 
            typeof (Brush), 
            typeof (MatrixViewModel<T>), 
            new PropertyMetadata(default(Brush))
        );
         
        public Brush Background
        {
            get { return (Brush) GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public void reSize(uint rows, uint cols)
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
        }

        public void clear()
        {
            _matrix = new CellViewModel<T>[ColsCount,RowsCount];
        }
    }
}
