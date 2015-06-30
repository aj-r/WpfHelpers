using System;
using System.Globalization;
using System.Linq;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// A multi-value converter that combines boolean values using or logic.
    /// </summary>
    public class OrConverter : OneWayMultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<bool>().Any(b => b);
        }
    }
}
