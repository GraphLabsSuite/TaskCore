using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GraphLabs.Components.Helpers.Converters
{
    /// <summary> Конвертер из Boolean в Visibility </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary> Из Boolean в Visibility </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        /// <summary> Из Visibility в Boolean </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Visibility)value)
            {
                case Visibility.Collapsed:
                    return false;
                case Visibility.Visible:
                    return true;
                default:
                    throw new NotSupportedException("Быть такого не может!");
            }
        }
    }
}
