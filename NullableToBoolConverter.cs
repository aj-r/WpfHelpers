using System;
using System.Globalization;

namespace WpfHelpers
{
    /// <summary>
    /// Converts a nullable value to a boolean: true if the value is not null; false if it is null.
    /// </summary>
    /// <remarks>
    /// The value can be a reference type or a System.Nullable&lt;T&gt;.
    /// </remarks>
    public class NullableToBoolConverter : OneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
    }
}
