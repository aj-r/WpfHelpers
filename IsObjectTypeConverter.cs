using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfHelpers
{
    /// <summary>
    /// A converter that converts to a boolean value that indicates whether the input object is an instance of a certain type
    /// </summary>
    public class IsObjectTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter", "ConverterParameter is required.");
            if (!(parameter is Type))
                throw new ArgumentException("parameter must be a type.", "parameter");
            var expectedType = (Type)parameter;
            if (value == null)
                return false;
            var type = value.GetType();
            return expectedType.IsAssignableFrom(type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("IsObjectTypeConverter can only convert one-way.");
        }
    }
}
