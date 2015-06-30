using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Sharp.Utils.Wpf.Controls
{
    /// <summary>
    /// A button that opens a drop-down panel when clicked. Similar to a combo box, but has no selected item.
    /// </summary>
    [TemplatePart(Name = "PART_Button")]
    [TemplatePart(Name = "PART_DropDownHost")]
    public class DropDownButton : ContentControl, ICommandSource
    {
        private bool closingDebounce;
        private DispatcherTimer debounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
        private UIElement dropDownHost;

        /// <summary>
        /// Creates a new <see cref="DropDownButton"/> instance.
        /// </summary>
        public DropDownButton()
        {
            debounceTimer.Tick += debounceTimer_Tick;
        }

        #region Event Handlers

        /// <summary>
        /// Invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            var button = GetTemplateChild("PART_Button") as ButtonBase;
            if (button != null)
                button.Click += button_Click;
            dropDownHost = GetTemplateChild("PART_DropDownHost") as UIElement;
            if (dropDownHost != null && DropDown != null && !(DropDown is ContextMenu))
            {
                // Don't let mouse events for the drop-down bubble up to parent controls.
                dropDownHost.MouseDown += EatMouseEvent;
                dropDownHost.MouseUp += EatMouseEvent;
                dropDownHost.MouseEnter += EatMouseEvent;
                dropDownHost.MouseLeave += EatMouseEvent;
                dropDownHost.MouseMove += EatMouseEvent;
                var dropDownHostContainer = dropDownHost as IAddChild;
                if (dropDownHostContainer != null)
                    dropDownHostContainer.AddChild(DropDown);
            }

            var window = this.Ancestor<Window>();
            if (window != null)
                window.PreviewMouseDown += window_PreviewMouseDown;

            base.OnApplyTemplate();
        }

        private void EatMouseEvent(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void EatMouseEvent(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsDropDownOpen)
                return;
            if (((DependencyObject)e.OriginalSource).HasAncestor(this))
                return;
            IsDropDownOpen = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OnClick(new RoutedEventArgs(ClickEvent));
        }

        private void debounceTimer_Tick(object sender, EventArgs e)
        {
            closingDebounce = false;
            debounceTimer.Stop();
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="ClickMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(DropDownButton),
            new PropertyMetadata(ClickMode.Release));

        /// <summary>
        /// Gets or sets the command to invoke when the button is pressed.
        /// </summary>
        public ClickMode ClickMode
        {
            get { return (ClickMode)GetValue(ClickModeProperty); }
            set { SetValue(ClickModeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(DropDownButton),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command to invoke when the button is pressed.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DropDownButton),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the parameter to pass to the command.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(DropDownButton),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the element on which to raise the specified command.
        /// </summary>
        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DropDown"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(FrameworkElement), typeof(DropDownButton),
            new PropertyMetadata(null, DropDownProperty_Changed));

        /// <summary>
        /// Gets or sets the DropDown menu for the <see cref="DropDownButton"/>. Should be a ContextMenu or a Popup.
        /// </summary>
        public FrameworkElement DropDown
        {
            get { return (FrameworkElement)GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }

        private static void DropDownProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ddb = (DropDownButton)d;
            var cm = e.NewValue as ContextMenu;
            if (cm != null)
            {
                cm.PlacementTarget = ddb;
                cm.Placement = PlacementMode.Bottom;
                if (ddb.dropDownHost != null)
                    ddb.dropDownHost.Visibility = Visibility.Hidden;
            }
            else if (ddb.dropDownHost != null)
            {
                var dropDownHostContainer = ddb.dropDownHost as IAddChild;
                if (dropDownHostContainer != null)
                    dropDownHostContainer.AddChild(ddb.DropDown);
                ddb.dropDownHost.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DropDownButton),
            new UIPropertyMetadata(false, IsDropDownOpenProperty_Changed));

        /// <summary>
        /// Gets or sets the DropDown menu for the <see cref="DropDownButton"/>.
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set
            {
                if (value && !IsDropDownEnabled)
                    throw new InvalidOperationException("Cannot open drop down when it is disabled.");
                SetValue(IsDropDownOpenProperty, value);
            }
        }

        private static void IsDropDownOpenProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ddb = d as DropDownButton;
            if (ddb.closingDebounce || !ddb.IsDropDownEnabled)
            {
                ddb.IsDropDownOpen = false;
                return;
            }
            if ((bool)e.NewValue)
                ddb.OnDropDownOpened(new RoutedEventArgs(DropDownOpenedEvent));
            else
                ddb.OnDropDownClosed(new RoutedEventArgs(DropDownClosedEvent));
        }

        /// <summary>
        /// Identifies the <see cref="IsDropDownEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDropDownEnabledProperty = DependencyProperty.Register("IsDropDownEnabled", typeof(bool), typeof(DropDownButton),
            new UIPropertyMetadata(true));

        /// <summary>
        /// Gets or sets whether the DropDown menu is enabled.
        /// </summary>
        public bool IsDropDownEnabled
        {
            get { return (bool)GetValue(IsDropDownEnabledProperty); }
            set { SetValue(IsDropDownEnabledProperty, value); }
        }

        #endregion

        #region Routed Events

        /// <summary>
        /// Invoked when the user opens the drop-down.
        /// </summary>
        /// <param name="e">Arguments for the event.</param>
        protected virtual void OnDropDownOpened(RoutedEventArgs e)
        {
            if (e.Handled || !IsDropDownEnabled)
                return;
            RaiseEvent(e);
        }

        /// <summary>
        /// Identifies the <see cref="DropDownOpened"/> routed event.
        /// </summary>
        public static RoutedEvent DropDownOpenedEvent = EventManager.RegisterRoutedEvent("DropDownOpened", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DropDownButton));
        
        /// <summary>
        /// Occurs when the drop-down menu is opened.
        /// </summary>
        public event RoutedEventHandler DropDownOpened
        {
            add { AddHandler(DropDownOpenedEvent, value); }
            remove { RemoveHandler(DropDownOpenedEvent, value); }
        }

        /// <summary>
        /// Invoked when the user closes the drop-down.
        /// </summary>
        /// <param name="e">Arguments for the event.</param>
        protected virtual void OnDropDownClosed(RoutedEventArgs e)
        {
            if (e.Handled)
                return;
            closingDebounce = true;
            debounceTimer.Start();
            RaiseEvent(e);
        }

        /// <summary>
        /// Identifies the <see cref="DropDownClosed"/> routed event.
        /// </summary>
        public static RoutedEvent DropDownClosedEvent = EventManager.RegisterRoutedEvent("DropDownClosed", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DropDownButton));

        /// <summary>
        /// Occurs when the drop-down menu is closed.
        /// </summary>
        public event RoutedEventHandler DropDownClosed
        {
            add { AddHandler(DropDownClosedEvent, value); }
            remove { RemoveHandler(DropDownClosedEvent, value); }
        }

        /// <summary>
        /// Invoked when the user clicks on the drop-down button.
        /// </summary>
        /// <param name="e">Arguments for the event.</param>
        protected virtual void OnClick(RoutedEventArgs e)
        {
            if (e.Handled)
                return;
            RaiseEvent(e);
        }

        /// <summary>
        /// Identifies the <see cref="Click"/> routed event.
        /// </summary>
        public static RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownButton));

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        #endregion

    }
}
