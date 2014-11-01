using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfHelpers
{
    /// <summary>
    /// Converts a <see cref="System.TimeSpan"/> to a string displaying the most significant non-zero component of the timespan.
    /// </summary>
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan))
                throw new ArgumentException("Cannot convert  from type " + value.GetType().Name, "value");
            var time = (TimeSpan)value;
            if (time == TimeSpan.MaxValue)
                return "never";
            if (time.Milliseconds > 0)
                return GetOutput((int)time.TotalMilliseconds, "millisecond", "milliseconds");
            if (time.Seconds > 0)
                return GetOutput((int)time.TotalSeconds, "second", "seconds");
            if (time.Minutes > 0)
                return GetOutput((int)time.TotalMinutes, "minute", "minutes");
            if (time.Hours > 0)
                return GetOutput((int)time.TotalHours, "hour", "hours");
            return GetOutput((int)time.TotalDays, "day", "days");
        }

        private static string GetOutput(int value, string singular, string plural)
        {
            return value + " " + (value == 1 ? singular : plural);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            var str = (string)value;
            if (str == "never")
                return TimeSpan.MaxValue;
            // TODO: this is not tested. does it work?
            var time = TimeSpan.Parse(str);
            return time;
        }
    }
}
