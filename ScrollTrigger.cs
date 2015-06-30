using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Invokes actions when the scroll position changes.
    /// </summary>
    public class ScrollTrigger : TriggerBase<FrameworkElement>
    {
        /// <summary>
        /// Called after the trigger is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var scrollViewer in GetScrollViewers())
                scrollViewer.ScrollChanged += scrollViewer_ScrollChanged;
        }

        private void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            const double changeTolerance = 1e-6;
            if (Math.Abs(e.VerticalChange) < changeTolerance && Math.Abs(e.HorizontalChange) < changeTolerance)
                return;
            InvokeActions(e.OriginalSource);
        }

        private IEnumerable<ScrollViewer> GetScrollViewers()
        {
            for (DependencyObject element = AssociatedObject; element != null; element = VisualTreeHelper.GetParent(element))
                if (element is ScrollViewer)
                    yield return element as ScrollViewer;
        }
    }
}
