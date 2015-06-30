using System;
using System.Windows;
using System.Windows.Controls;

namespace Sharp.Utils.Wpf.Controls
{
    /// <summary>
    /// A text box that only allows double precision floating point input.
    /// </summary>
    /// <remarks>
    /// Copied from http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
    /// </remarks>
    public class NumericTextBox : RestrictedValueTextBox
    {
        /// <summary>
        /// Creates a new <see cref="NumericTextBox"/> instance.
        /// </summary>
        public NumericTextBox()
        {
            TextAlignment = TextAlignment.Right;
        }

        /// <summary>
        /// Checks the format of the text and determines whether it is valid.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <returns><value>true</value> if the input text is a valid number; otherwise <value>false</value>.</returns>
        protected override bool CheckFormat(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;
            double val;
            return double.TryParse(text, out val);
        }

        /// <summary>
        /// Occurs when the text in the text box changes.
        /// </summary>
        /// <param name="e">The arguments for the event.</param>
        /// <remarks>
        /// This event is only raised if the current text in the text box is in a valid format.
        /// </remarks>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            Value = string.IsNullOrEmpty(Text) ? 0 : double.Parse(Text);
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> property.
        /// </summary>
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(NumericTextBox),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueProperty_Changed));

        /// <summary>
        /// The numeric value in the text box.
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueProperty_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((IntegerTextBox)obj).Text = e.NewValue.ToString();
        }

    }
}
