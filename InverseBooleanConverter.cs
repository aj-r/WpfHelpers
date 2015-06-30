using System;
using System.Globalization;
using System.Windows.Data;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// Converts a boolean value to its inverse.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("value must be a boolean.", "value");
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("value must be a boolean.", "value");
            return !(bool)value;
        }
    }
}
