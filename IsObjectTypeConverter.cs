using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// A converter that converts to a boolean value that indicates whether the input object is an instance of a certain type
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool), ParameterType = typeof(Type))]
    public class IsObjectTypeConverter : OneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
    }
}
