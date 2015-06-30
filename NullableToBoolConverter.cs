using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Converts a nullable value to a boolean: true if the value is not null; false if it is null. The value can be a reference type or a System.Nullable&lt;T&gt;.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class NullableToBoolConverter : OneWayValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value. This is <value>true</value> if the value is NOT null, or <value>false</value> if it IS null.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
    }
}
