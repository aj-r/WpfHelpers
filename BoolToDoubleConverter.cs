using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfHelpers
{
    /// <summary>
    /// Converts a boolean to a double precision value.
    /// </summary>
    /// <remarks>
    /// False converts to 0.0; true converts to the ConverterParameter value, or 1.0 if no ConverterParameter is specified.
    /// </remarks>
    public class BoolToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("Cannot convert from type " + value.GetType().Name, "value");
            double falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return (bool)value ? trueValue : falseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            var opacity = (double)value;
            double falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return opacity == trueValue;
        }

        private void ParseParameter(object parameter, out double falseValue, out double trueValue)
        {
            falseValue = 0.0;
            trueValue = 1.0;
            var stringValue = parameter as string;
            if (string.IsNullOrWhiteSpace(stringValue))
                return;
            var stringValues = stringValue.Split(';');
            if (stringValues.Length == 0)
                return;

            if (stringValues.Length == 1)
            {
                trueValue = double.Parse(stringValues[0]);
            }
            else
            {
                falseValue = double.Parse(stringValues[0]);
                trueValue = double.Parse(stringValues[1]);
            }
        }
    }
}
