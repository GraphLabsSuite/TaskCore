using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GraphLabs.Common.Helpers.Converters
{
    /// <summary> Конвертер из оценки в цвет </summary>
    public class ScoreToBrushConverter : IValueConverter
    {
        private const int GREEN_RANGE_START = 80;
        private const int YELLOW_RANGE_START = 60;


        /// <summary> Из оценки в цвет </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assert(targetType == typeof(Brush));
            var score = (int)value;

            if (score > GREEN_RANGE_START)
            {
                return new SolidColorBrush(Colors.Green);
            }
            else if (score > YELLOW_RANGE_START)
            {
                return new SolidColorBrush(Colors.Orange);
            }
            else
            {
                return new SolidColorBrush(Colors.Red);
            }
        }

        /// <summary> Из цвета в оценку - нельзя </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
