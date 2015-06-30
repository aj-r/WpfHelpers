using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// A markup extension that gives access to Enum values and descriptions in XAML.
    /// </summary>
    public class EnumerationExtension : MarkupExtension
    {
        private readonly Type _enumType;

        /// <summary>
        /// Creates a new <see cref="EnumerationExtension"/> instance.
        /// </summary>
        /// <param name="enumType">The type of enum for which to get values.</param>
        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

            if (enumType.IsEnum == false)
                throw new ArgumentException("Type must be an Enum.", "enumType");

            _enumType = enumType;
        }

        /// <summary>
        /// Gets the type of enum for which to get values.
        /// </summary>
        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                    return;

            }
        }

        /// <summary>
        /// Provides a list containing an <see cref="EnumerationMember"/> containing the value and description for each item in the enum. 
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>A list of EnumerationMembers</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            return (
              from object enumValue in enumValues
              select new EnumerationMember
              {
                  Value = enumValue,
                  Description = GetDescription(enumValue)
              }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = EnumType
              .GetField(enumValue.ToString())
              .GetCustomAttributes(typeof(DescriptionAttribute), false)
              .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute != null
              ? descriptionAttribute.Description
              : enumValue.ToString();
        }

        /// <summary>
        /// Represents the value and description for an item in the enum.
        /// </summary>
        public class EnumerationMember
        {
            /// <summary>
            /// Gets or sets the description of the enum item. This is generally determined from the DescriptionAttribute on the enum member, if present.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the value of the enum item.
            /// </summary>
            public object Value { get; set; }
        }
    }
}
