using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Calculates a <see cref="System.Double"/> value by linearly interpolating between two numeric values.
    /// This value converter requires 3 input values. The first two inputs are the starting and ending values, 
    /// and the third input is the scale (0-1). A scale of less than 0 or greater than 1 results in a value
    /// outside of the first two input values.
    /// </summary>
    public class LinearInterpolationConverter : OneWayMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding
        /// engine calls this method when it propagates the values from source bindings
        /// to the binding target.
        /// </summary>
        /// <param name="values">
        /// The values produced by the binding source. The first two inputs are the starting and ending values, 
        /// and the third input is the scale (0-1). A scale of less than 0 or greater than 1 results in a value
        /// outside of the first two input values.
        /// </param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3)
                throw new ArgumentException("LinearInterpolationConverter must have 3 input values.", "values");
            var value1 = GetNumericValue(values[0]);
            var value2 = GetNumericValue(values[1]);
            var scale = GetNumericValue(values[2]);
            var output = value1 + (value2 - value1) * scale;
            return output;
        }

        private static double GetNumericValue(object obj)
        {
            if (obj is double)
                return (double)obj;
            if (obj is int)
                return (double)(int)obj;
            if (obj is long)
                return (double)(long)obj;
            if (obj is short)
                return (double)(short)obj;
            if (obj is float)
                return (double)(float)obj;
            if (obj is byte)
                return (double)(byte)obj;
            if (obj is string)
                return double.Parse((string)obj);
            if (obj == null)
                throw new ArgumentNullException("obj");
            throw new NotSupportedException(string.Concat("The type '", obj.GetType(), "' is not a supported numeric type."));
        }
    }
}
