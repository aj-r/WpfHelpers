using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfHelpers
{
    /// <summary>
    /// Converts an object reference to a boolean value: true if the object references are equal; false if they are not.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool), ParameterType = typeof(object))]
    public class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            return (bool)value ? parameter : null;
        }
    }
}
