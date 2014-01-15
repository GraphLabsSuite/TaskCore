using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GraphLabs.Common.Helpers.Converters
{
    /// <summary> Конвертер из штрафа в видимость </summary>
    public class PenaltyToVisibilityConverter : IValueConverter
    {
        /// <summary> Из штрафа в видимость </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(targetType == typeof(Visibility));
            var penalty = (int)value;

            return penalty != 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary> Из видимости в штраф - нельзя </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
