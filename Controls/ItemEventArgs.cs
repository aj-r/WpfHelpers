using System;
using System.Windows;

namespace WpfHelpers.Controls
{
    public class ItemEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Creates a new <see cref="WpfHelpers.Controls.ItemEventArgs"/> instance.
        /// </summary>
        public ItemEventArgs()
        { }

        /// <summary>
        /// Creates a new <see cref="WpfHelpers.Controls.ItemEventArgs"/> instance.
        /// </summary>
        /// <param name="item">The item for which the event was raised.</param>
        public ItemEventArgs(object item)
        {
            this.item = item;
        }

        /// <summary>
        /// Creates a new <see cref="WpfHelpers.Controls.ItemEventArgs"/> instance.
        /// </summary>
        /// <param name="item">The item for which the event was raised.</param>
        /// <param name="routedEvent">The routed event identifier for this <see cref="WpfHelpers.Controls.ItemEventArgs"/> instance.</param>
        public ItemEventArgs(object item, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            this.item = item;
        }

        /// <summary>
        /// Creates a new <see cref="WpfHelpers.Controls.ItemEventArgs"/> instance.
        /// </summary>
        /// <param name="item">The item for which the event was raised.</param>
        /// <param name="routedEvent">The routed event identifier for this <see cref="WpfHelpers.Controls.ItemEventArgs"/> instance.</param>
        /// <param name="source">An alternate source that will be reported when this event is handled. This pre-populates the <see cref="System.Windows.RoutedEventArgs.Source"/> property.</param>
        public ItemEventArgs(object item, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            this.item = item;
        }

        private object item;

        /// <summary>
        /// Gets the item for which the event was raised.
        /// </summary>
        public object Item
        {
            get { return item; }
        }
    }

    public delegate void ItemEventHandler(object sender, ItemEventArgs e);
}
