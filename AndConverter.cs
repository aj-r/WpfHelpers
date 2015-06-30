using System;
using System.Globalization;
using System.Linq;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// A multi-value converter that combines boolean values using and logic.
    /// </summary>
    public class AndConverter : OneWayMultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<bool>().All(b => b);
        }
    }
}
