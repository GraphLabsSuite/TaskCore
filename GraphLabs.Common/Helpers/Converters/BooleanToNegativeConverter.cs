using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphLabs.Common.Helpers.Converters
{
    /// <summary> Конвертер из Boolean в !Boolean </summary>
    public class BooleanToNegativeConverter : IValueConverter
    {
        /// <summary> Из Boolean в !Boolean </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        /// <summary> Из Boolean в !Boolean </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
