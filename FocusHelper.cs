using System;
using System.Windows;
using System.Windows.Input;

namespace Sharp.Utils.Wpf
{
    public static class FocusHelper
    {
        /// <summary>
        /// Removes focus from a control when the enter key is pressed while the control has focus.
        /// </summary>
        public static void RemoveFocusOnEnterKeyPress(FrameworkElement control)
        {
            control.KeyDown += control_KeyDown;
        }


        private static void control_KeyDown(object sender, KeyEventArgs e)
        {
            var control = sender as FrameworkElement;
            switch (e.Key)
            {
                case Key.Enter:
                    // Move to a parent that can take focus
                    var parent = control.Parent as FrameworkElement;
                    while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable)
                        parent = (FrameworkElement)parent.Parent as FrameworkElement;
                    var input = parent as IInputElement;
                    if (input != null)
                    {
                        DependencyObject scope = FocusManager.GetFocusScope(control);
                        FocusManager.SetFocusedElement(scope, input);
                    }
                    break;
            }
        }
        /// <summary>
        /// Removes focus from a control.
        /// </summary>
        public static void RemoveFocus(FrameworkElement control)
        {
            // Move to a parent that can take focus
            var parent = control.Parent as FrameworkElement;
            while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable)
                parent = (FrameworkElement)parent.Parent as FrameworkElement;
            var input = parent as IInputElement;
            if (input != null)
            {
                DependencyObject scope = FocusManager.GetFocusScope(control);
                FocusManager.SetFocusedElement(scope, input);
            }
        }
    }
}
