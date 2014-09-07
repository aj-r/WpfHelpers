using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

// From http://stackoverflow.com/questions/339620/how-do-i-remove-minimize-and-maximize-from-a-resizable-window-in-wpf
namespace WpfHelpers
{
    public static class WindowExtensions
    {
        // from winuser.h
        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        /// <summary>
        /// Hides the minimize and maximize buttons.
        /// </summary>
        /// <param name="window">The window for which to hide the buttons.</param>
        public static void HideMinimizeAndMaximizeButtons(this Window window)
        {
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }

        /// <summary>
        /// Hides the close button for the window. Also removes the icon and system menu from the top left corner.
        /// </summary>
        /// <param name="window">The window for which to hide the button.</param>
        public static void HideCloseButton(this Window window)
        {
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_SYSMENU));
        }
    }
}
