using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfHelpers
{
    /// <summary>
    /// Converts an integer to a boolean value. 0 converts to false, all other numbers convert to true.
    /// </summary>
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
                throw new ArgumentException("Cannot convert from type " + value.GetType().Name, "value");
            return (int)value != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            return (bool)value ? 1 : 0;
        }
    }
}
