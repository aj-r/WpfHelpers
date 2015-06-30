using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Converts a <see cref="System.TimeSpan"/> to a string displaying the most significant non-zero component of the timespan.
    /// </summary>
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : IValueConverter
    {
        private static readonly string[,] componentNames = {
            { null, null, null },
            { "day", "days", null },
            { "hour", "hours", "h" },
            { "minute", "minutes", "min" },
            { "second", "seconds", "s" },
            { "millisecond", "milliseconds", "ms" }
        };

        private static readonly Regex regex = new Regex(@"([0-9]+) *(.+)");

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
                if (!Enum.TryParse(componentName, out component) || component == DateTimeComponent.None)
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
                    return GetOutput((int)time.TotalDays, component);
                case DateTimeComponent.Hour:
                    return GetOutput((int)time.TotalHours, component);
                case DateTimeComponent.Minute:
                    return GetOutput((int)time.TotalMinutes, component);
                case DateTimeComponent.Second:
                    return GetOutput((int)time.TotalSeconds, component);
                default:
                    return GetOutput((int)time.TotalMilliseconds, component);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                throw new ArgumentException("Cannot convert back from type " + value.GetType().Name, "value");
            var str = (string)value;
            if (str == "never")
                return TimeSpan.MaxValue;
            var match = regex.Match(str);
            if (match != null)
            {
                string valueString = match.Groups[1].Value;
                string componentName = match.Groups[2].Value;
                double componentValue;
                if (componentName != null && double.TryParse(valueString, out componentValue))
                {
                    DateTimeComponent component = DateTimeComponent.None;
                    var length0 = componentNames.GetLength(0);
                    var length1 = componentNames.GetLength(1);
                    for (int i = 0; i < length0; i++)
                    {
                        for (int j = 0; j < length1; j++)
                        {
                            if (componentName.Equals(componentNames[i, j], StringComparison.InvariantCultureIgnoreCase))
                            {
                                component = (DateTimeComponent)i;
                                break;
                            }
                        }
                        if (component != DateTimeComponent.None)
                            break;
                    }
                    switch (component)
                    {
                        case DateTimeComponent.Day:
                            return TimeSpan.FromDays(componentValue);
                        case DateTimeComponent.Hour:
                            return TimeSpan.FromHours(componentValue);
                        case DateTimeComponent.Minute:
                            return TimeSpan.FromMinutes(componentValue);
                        case DateTimeComponent.Second:
                            return TimeSpan.FromSeconds(componentValue);
                        case DateTimeComponent.Millisecond:
                            return TimeSpan.FromMilliseconds(componentValue);
                    }
                }
            }
            return TimeSpan.Parse(str);
        }

        private string GetOutput(int value, DateTimeComponent component)
        {
            string componentName = null;
            if (Abbreviate)
                componentName = componentNames[(int)component, 2];
            if(componentName == null)
                componentName = componentNames[(int)component, value == 1 ? 0 : 1];
            return string.Concat(value, " ", componentName);
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
