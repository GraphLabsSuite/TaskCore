﻿using System;
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
    /// <summary> ViewModel колонки матрицы </summary>
    /// <typeparam name="T"></typeparam>
    public class MatrixColumnViewModel<T> : DependencyObject
    {
        private CellViewModel<T>[] _column;

        //public ReadOnlyCollection<T> Column;


        /// <summary> Фон колонки </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background",
            typeof(Brush),
            typeof(MatrixRowViewModel<T>),
            new PropertyMetadata(default(Brush))
        );

        /// <summary> Фон колонки </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary> Изменить размер </summary>
        public void ReSize(uint countOfRows)
        {
            var newcolumn = new CellViewModel<T>[countOfRows]; 
            for (int i = 0; i < Math.Min((_column.Length), countOfRows); i++)
            {
                newcolumn[i] = _column[i];
            }
            _column = newcolumn;

        }
    }
}
