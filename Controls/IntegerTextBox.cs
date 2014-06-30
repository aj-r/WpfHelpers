using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfHelpers.Controls
{
    /// <summary>
    /// A text box that only allows integer input.
    /// </summary>
    /// <remarks>
    /// Copied from http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
    /// </remarks>
    public class IntegerTextBox : RestrictedValueTextBox
    {
        public IntegerTextBox()
        {
            TextAlignment = TextAlignment.Right;
        }

        protected override bool CheckFormat(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;
            int val;
            return int.TryParse(text, out val);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            Value = string.IsNullOrEmpty(Text) ? 0 : int.Parse(Text);
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(IntegerTextBox),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueProperty_Changed));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueProperty_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((IntegerTextBox)obj).Text = e.NewValue.ToString();
        }

    }
}
