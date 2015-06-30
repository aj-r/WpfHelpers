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
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
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
