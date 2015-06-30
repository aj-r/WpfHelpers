// Stolen from http://stackoverflow.com/questions/501886/how-should-the-viewmodel-close-the-form
using System;
using System.Windows;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Contains an attached property that can be used to set the DialogResult of a window.
    /// </summary>
    public static class DialogCloser
    {
        /// <summary>
        /// An attached property that sets the DialogResult of a window.
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
            "DialogResult",
            typeof(bool?),
            typeof(DialogCloser),
            new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }

        /// <summary>
        /// Sets the DialogResult of a window.
        /// </summary>
        /// <param name="target">The target window.</param>
        /// <param name="value">The DialogResult.</param>
        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }
    }
}
