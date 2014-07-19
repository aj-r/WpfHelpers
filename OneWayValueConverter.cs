using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfHelpers
{
    public abstract class OneWayValueConverter : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only convert one-way.");
        }
    }
}
