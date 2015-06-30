using System;
using System.Globalization;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// An abstract base class for value converters that only convert one way (from source to binding).
    /// </summary>
    public abstract class OneWayValueConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="values">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a binding value back to its source value. This implementation always throws a NotSupportedException.
        /// </summary>
        /// <exception cref="NotSupportedException" />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only convert one-way.");
        }
    }

    /// <summary>
    /// An abstract base class for multi-value converters that only convert one way (from source to binding).
    /// </summary>
    public abstract class OneWayMultiValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding
        /// engine calls this method when it propagates the values from source bindings
        /// to the binding target.
        /// </summary>
        /// <param name="values">The values produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a binding value back to its source value. This implementation always throws a NotSupportedException.
        /// </summary>
        /// <exception cref="NotSupportedException" />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only convert one-way.");
        }
    }
}
