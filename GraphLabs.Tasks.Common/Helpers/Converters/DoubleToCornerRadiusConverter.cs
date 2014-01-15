using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Data;

namespace GraphLabs.Common.Helpers.Converters
{
    /// <summary> Конвертер из CornerRadius в double и обратно </summary>
    public class DoubleToCornerRadiusConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
        /// </summary>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var input = (CornerRadius)value;
            
            // ReSharper disable CompareOfFloatsByEqualityOperator
            Contract.Assert(input.BottomLeft != input.BottomRight ||
                input.BottomLeft != input.TopLeft ||
                input.BottomLeft != input.TopRight ||
                input.BottomRight != input.TopLeft ||
                input.BottomRight != input.TopRight ||
                input.TopLeft != input.TopRight);
            // ReSharper restore CompareOfFloatsByEqualityOperator

            return input.BottomLeft;
        }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var input = (double)value;
            if (double.IsNaN(input))
                input = 0.0;
            return new CornerRadius(input, input, input, input);
        }
    }
}
