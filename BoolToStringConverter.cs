using System;
using System.Globalization;
using System.Windows.Data;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// Converts a boolean to a double precision value.
    /// </summary>
    /// <remarks>
    /// The parameter should contain a string, or 2 strings separated by a semicolon (;).
    /// If the parameter contains 2 strings, the first string is used as the false value, and the second is used as the true value.
    /// If the parameter contains 1 string, it is used as the true value, and an empty string is used as the false value.
    /// </remarks>
    [ValueConversion(typeof(bool), typeof(string), ParameterType=typeof(string))]
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("Cannot convert from type " + value.GetType().Name, "value");
            string falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return (bool)value ? trueValue : falseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            var stringValue = (string)value;
            string falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return stringValue.Equals(trueValue, StringComparison.Ordinal);
        }

        private void ParseParameter(object parameter, out string falseValue, out string trueValue)
        {
            falseValue = string.Empty;
            trueValue = string.Empty;
            var stringValue = parameter as string;
            if (string.IsNullOrWhiteSpace(stringValue))
                return;
            var stringValues = stringValue.Split(';');
            if (stringValues.Length == 0)
                return;

            if (stringValues.Length == 1)
            {
                trueValue = stringValues[0];
            }
            else
            {
                falseValue = stringValues[0];
                trueValue = stringValues[1];
            }
        }
    }
}
