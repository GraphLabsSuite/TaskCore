using System;
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
    /// <summary> ViewModel ячейки </summary>
    /// <typeparam name="T"></typeparam>
    public class CellViewModel<T> : DependencyObject
    {
        /// <summary> Значение </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", 
            typeof (T), 
            typeof (CellViewModel<T>), 
            new PropertyMetadata(default(T))
        );

        /// <summary> Значение </summary>
        public T Value
        {
            get { return (T) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary> Фон </summary>
        public static readonly DependencyProperty BackProperty = DependencyProperty.Register(
            "Background",
            typeof (Brush),
            typeof (CellViewModel<T>),
            new PropertyMetadata(default(Brush))
        );

        /// <summary> Фон </summary>
        public Brush Background
        {
            get { return (Brush) GetValue(BackProperty); }
            set { SetValue(BackProperty, value); }
        }
    }
}
