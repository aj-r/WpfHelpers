using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Converts an object reference to a boolean value: true if the object references are equal; false if they are not.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool), ParameterType = typeof(object))]
    public class ObjectToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ReferenceEquals(value, parameter);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            return (bool)value ? parameter : null;
        }
    }
}
