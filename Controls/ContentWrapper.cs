using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace WpfHelpers.Controls
{
    /// <summary>
    /// A ContentControl wrapper that can contain any FrameworkElement. Useful for adding mouse events to controls that otherwise lack them.
    /// </summary>
    [ContentProperty("Content")]
    public class ContentWrapper : ContentControl
    {
        private FrameworkElement content;

        public FrameworkElement Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                if (content != null)
                    AddChild(content);
            }
        }
    }
}
