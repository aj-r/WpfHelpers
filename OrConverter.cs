using System;
using System.Globalization;
using System.Linq;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// A multi-value converter that combines boolean values using or logic.
    /// </summary>
    public class OrConverter : OneWayMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding
        /// engine calls this method when it propagates the values from source bindings
        /// to the binding target.
        /// </summary>
        /// <param name="values">The values produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<bool>().Any(b => b);
        }
    }
}
