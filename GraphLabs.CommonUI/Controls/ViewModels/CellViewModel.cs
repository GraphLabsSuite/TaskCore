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
    public class CellViewModel<T> : DependencyObject
    {

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", 
            typeof (T), 
            typeof (CellViewModel<T>), 
            new PropertyMetadata(default(T))
        );

        public T Value
        {
            get { return (T) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty BackProperty = DependencyProperty.Register(
            "Background",
            typeof (Brush),
            typeof (CellViewModel<T>),
            new PropertyMetadata(default(Brush))
        );

        public Brush Background
        {
            get { return (Brush) GetValue(BackProperty); }
            set { SetValue(BackProperty, value); }
        }
    }
}
