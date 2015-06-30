using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Converts a <see cref="System.TimeSpan"/> to a string displaying the most significant non-zero component of the timespan.
    /// This converter is only designed to work with English cultures.
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

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
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

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
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

        /// <summary>
        /// The <see cref="DateTimeComponent"/> of the TimeSpan to display. If this is not set, then the most significant non-zero component of the TimeSpan will be displayed.
        /// </summary>
        public DateTimeComponent Component { get; set; }

        /// <summary>
        /// Gets or sets whether the abbreviated version of the TimeSpan component should be shown (e.g. "h" instead of "hours").
        /// </summary>
        public bool Abbreviate { get; set; }
    }

    /// <summary>
    /// Represents a component of a date or time.
    /// </summary>
    public enum DateTimeComponent
    {
        /// <summary>
        /// Indicates that no DateTimeComponent is specified.
        /// </summary>
        None,
        /// <summary>
        /// The day component of a time span.
        /// </summary>
        Day,
        /// <summary>
        /// The hour component of a time span.
        /// </summary>
        Hour,
        /// <summary>
        /// The minute component of a time span.
        /// </summary>
        Minute,
        /// <summary>
        /// The second component of a time span.
        /// </summary>
        Second,
        /// <summary>
        /// The millisecond component of a time span.
        /// </summary>
        Millisecond
    }
}
