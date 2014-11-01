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
    [ValueConversion(typeof(object), typeof(bool))]
    public class NullableToBoolConverter : OneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
    }
}
