using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Converts a boolean to a double precision value.
    /// </summary>
    /// <remarks>
    /// The parameter should be a string representing a number, or 2 numbers separated by a semicolon (;).
    /// If the parameter contains 2 numbers, the first number is used as the false value, and the second is used as the true value.
    /// If the parameter contains 1 number, it is used as the true value, and an 0 is used as the false value.
    /// If the parameter is not specified, then 1 is used as the true value, and an 0 is used as the false value.
    /// </remarks>
    [ValueConversion(typeof(bool), typeof(double), ParameterType = typeof(string))]
    public class BoolToDoubleConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="values">The values produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("Cannot convert from type " + value.GetType().Name, "value");
            double falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return (bool)value ? trueValue : falseValue;
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
            if (!(value is double))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            var doubleValue = (double)value;
            double falseValue, trueValue;
            ParseParameter(parameter, out falseValue, out trueValue);
            return doubleValue == trueValue;
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
