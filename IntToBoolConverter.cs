using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Converts an integer to a boolean value. 0 converts to false, all other numbers convert to true.
    /// </summary>
    [ValueConversion(typeof(int), typeof(bool))]
    public class IntToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Creates a new <see cref="IntToBoolConverter"/> instance.
        /// </summary>
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

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var n = value as int?;
            if (n == null)
                throw new ArgumentException("Cannot convert from type " + value.GetType().Name, "value");
            return n >= MinValue && n <= MaxValue;
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
            if (!(value is bool))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            return (bool)value ? Value : 0;
        }
    }
}
