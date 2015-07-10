// Stolen from http://stackoverflow.com/questions/501886/how-should-the-viewmodel-close-the-form
using System;
using System.Windows;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Contains an attached property that can be used to set the DialogResult of a window.
    /// </summary>
    /// <remarks>
    /// This attached property is necessary when you need to set the DialogResult from the view model.
    /// Since the Window.DialogResult property is not a DependencyProperty, you cannot bind to it.
    /// The DialogCloser.DialogResult property IS a DependencyProperty, so you can bind a ViewModel property to it.
    /// </remarks>
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
