using System;
using System.Globalization;
using System.Text;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Converts an enum value to a string.
    /// </summary>
    public class EnumToStringConverter : OneWayValueConverter
    {
        /// <summary>
        /// Gets or sets the string used to separate words in the enum value name.
        /// </summary>
        /// <remarks>
        /// Word boundaries are determined by looking for an uppercase letter followed by a lowercase letter (however the first character never has a word boundary before it).
        /// </remarks>
        public string WordSeparator { get; set; }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueName = Enum.GetName(value.GetType(), value);
            if (string.IsNullOrEmpty(WordSeparator) || valueName == null || valueName.Length <= 1)
                return valueName;
            var sb = new StringBuilder(valueName.Length * 2);
            sb.Append(valueName[0]);
            for (int i = 1; i < valueName.Length; i++)
            {
                if (char.IsUpper(valueName, i) && char.IsLower(valueName, i + 1))
                    sb.Append(WordSeparator);
                sb.Append(valueName[i]);
            }
            return sb.ToString();
        }
    }
}
