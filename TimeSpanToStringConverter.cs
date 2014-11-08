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
                throw new ArgumentException("value must be a TimeSpan", "value");
            var time = (TimeSpan)value;
            if (time == TimeSpan.MaxValue)
                return "never";

            DateTimeComponent component;
            if (Component != DateTimeComponent.None)
            {
                component = Component;
            }
            else
            {
                string componentName = parameter as string;
                if (!Enum.TryParse<DateTimeComponent>(componentName, out component))
                {
                    if (time.TotalDays >= 1.0)
                        component = DateTimeComponent.Day;
                    else if (time.TotalHours >= 1.0)
                        component = DateTimeComponent.Hour;
                    else if (time.TotalMinutes >= 1.0)
                        component = DateTimeComponent.Minute;
                    else if (time.TotalSeconds >= 1.0)
                        component = DateTimeComponent.Second;
                    else
                        component = DateTimeComponent.Millisecond;
                }
            }
            switch (component)
            {
                case DateTimeComponent.Day:
                    return GetOutput((int)time.TotalDays, "day", "days", "day");
                case DateTimeComponent.Hour:
                    return GetOutput((int)time.TotalHours, "hour", "hours", "hr.");
                case DateTimeComponent.Minute:
                    return GetOutput((int)time.TotalMinutes, "minute", "minutes", "min.");
                case DateTimeComponent.Second:
                    return GetOutput((int)time.TotalSeconds, "second", "seconds", "sec.");
                default:
                    return GetOutput((int)time.TotalMilliseconds, "millisecond", "milliseconds", "ms");
            }
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

        private string GetOutput(int value, string singular, string plural, string abbreviated)
        {
            return value + " " + (Abbreviate ? abbreviated : (value == 1 ? singular : plural));
        }

        public DateTimeComponent Component { get; set; }
        public bool Abbreviate { get; set; }
    }

    public enum DateTimeComponent
    {
        None,
        Day,
        Hour,
        Minute,
        Second,
        Millisecond
    }
}
