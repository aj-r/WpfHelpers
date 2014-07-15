using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfHelpers
{
    /// <summary>
    /// Converts a nullable value to a boolean: true if the value is not null; false if it is null.
    /// </summary>
    /// <remarks>
    /// The value can be a reference type or a System.Nullable&lt;T&gt;.
    /// </remarks>
    public class NullableToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only convert one-way.");
        }
    }
}
