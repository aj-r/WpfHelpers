using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// A multi-value converter that combines floating-point values using multiplication.
    /// </summary>
    public class MultiplyConverter : OneWayMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding
        /// engine calls this method when it propagates the values from source bindings
        /// to the binding target.
        /// </summary>
        /// <param name="values">The values produced by the binding source. These must be of type System.Double.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 1.0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] is double)
                    result *= (double)values[i];
            }
            return result;
        }
    }
}
