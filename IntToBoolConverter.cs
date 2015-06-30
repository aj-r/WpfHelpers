using System;
using System.Globalization;
using System.Windows.Data;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// Converts an integer to a boolean value. 0 converts to false, all other numbers convert to true.
    /// </summary>
    [ValueConversion(typeof(int), typeof(bool))]
    public class IntToBoolConverter : IValueConverter
    {
        public IntToBoolConverter()
        {
            MinValue = 1;
            MaxValue = int.MaxValue;
        }

        /// <summary>
        /// Gets or sets the minimum integer value that will yield an output of true.
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum integer value that will yield an output of true.
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the integer value that will yield an output of true. If a range of values is allowed, this is equal to MinValue.
        /// </summary>
        public int Value
        {
            get { return MinValue; }
            set
            {
                MinValue = value;
                MaxValue = value;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var n = value as int?;
            if (n == null)
                throw new ArgumentException("Cannot convert from type " + value.GetType().Name, "value");
            return n >= MinValue && n <= MaxValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            return (bool)value ? Value : 0;
        }
    }
}
