using System;
using System.Globalization;
using System.Windows.Data;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// Calculates a <see cref="System.Double"/> value by linearly interpolating between two numeric values.
    /// </summary>
    /// <remarks>
    /// This value converter requires 3 input values. The first two inputs are the starting and ending values, 
    /// and the third input is the scale (0-1). A scale of less than 0 or greater than 1 results in a value
    /// outside of the first two input values.
    /// </remarks>
    public class LinearInterpolationConverter : OneWayMultiValueConverter
    {
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
