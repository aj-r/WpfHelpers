using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Converts a numeric value to and from a string.
    /// </summary>
    public class NumberToStringConverter : IValueConverter
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
            return value.ToString();
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
            if (!(value is string))
                throw new ArgumentException("value must be a string.", "value");
            var str = (string)value;
            if (targetType == typeof(int))
            {
                int result;
                return int.TryParse(str, out result) ? result : 0;
            }
            if (targetType == typeof(long))
            {
                long result;
                return long.TryParse(str, out result) ? result : 0L;
            }
            if (targetType == typeof(short))
            {
                short result;
                return short.TryParse(str, out result) ? result : (short)0;
            }
            if (targetType == typeof(byte))
            {
                byte result;
                return byte.TryParse(str, out result) ? result : (byte)0;
            }
            if (targetType == typeof(double))
            {
                double result;
                return double.TryParse(str, out result) ? result : 0.0;
            }
            if (targetType == typeof(float))
            {
                float result;
                return float.TryParse(str, out result) ? result : 0.0f;
            }
            throw new ArgumentException("Cannot convert back to type " + targetType.Name, "targetType");
        }
    }
}
