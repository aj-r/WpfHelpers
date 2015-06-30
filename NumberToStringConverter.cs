using System;
using System.Globalization;
using System.Windows.Data;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// Converts a numeric value to and from a string.
    /// </summary>
    public class NumberToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

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
