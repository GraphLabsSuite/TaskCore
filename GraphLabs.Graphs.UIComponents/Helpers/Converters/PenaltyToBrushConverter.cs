using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GraphLabs.Components.Helpers.Converters
{
    /// <summary> Конвертер из штрафа в цвет </summary>
    public class PenaltyToBrushConverter : IValueConverter
    {
        /// <summary> Из штрафа в цвет </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(targetType == typeof(Brush));
            var penalty = (int)value;

            if (penalty >= 0)
            {
                return new SolidColorBrush(Color.FromArgb(255, 70, 130, 180));
            }
            else
            {
                return new SolidColorBrush(Colors.Red);
            }
        }

        /// <summary> Из цвета в штраф - нельзя </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
