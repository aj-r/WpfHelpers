﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace WpfHelpers.Controls
{
    [TemplatePart(Name = "PART_Button")]
    [TemplatePart(Name = "PART_DropDownButton")]
    [TemplatePart(Name = "PART_DropDownHost")]
    public class DropDownButton : ContentControl, ICommandSource
    {
        private bool closingDebounce;
        private DispatcherTimer debounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
        private IAddChild dropDownHost;

        public DropDownButton()
        {
            Binding binding = new Binding("DropDown.IsOpen");
            binding.Source = this;
            binding.Mode = BindingMode.TwoWay;
            SetBinding(IsDropDownOpenProperty, binding);

            debounceTimer.Tick += debounceTimer_Tick;
        }

        public override void OnApplyTemplate()
        {
            var button = GetTemplateChild("PART_Button") as ButtonBase;
            if (button != null)
                button.Click += button_Click;
            var dropDownButton = GetTemplateChild("PART_DropDownButton") as UIElement;
            if (dropDownButton != null)
                dropDownButton.PreviewMouseDown += toggle_PreviewMouseDown;
            dropDownHost = GetTemplateChild("PART_DropDownHost") as IAddChild;
            if (dropDownHost != null && DropDown != null && !(DropDown is ContextMenu))
                dropDownHost.AddChild(DropDown);
            base.OnApplyTemplate();
        }

        private void debounceTimer_Tick(object sender, EventArgs e)
        {
            closingDebounce = false;
            debounceTimer.Stop();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OnClick(new RoutedEventArgs(ClickEvent));
        }

        private void toggle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (closingDebounce)
                e.Handled = true;
        }

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

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(FrameworkElement), typeof(DropDownButton),
            new PropertyMetadata(null, DropDownProperty_Changed));

        /// <summary>
        /// Gets or sets the DropDown menu for the <see cref="WpfHelpers.Controls.DropDownButton"/>. Should be a ContextMenu or a Popup.
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
            var dropDownHostElement = ddb.dropDownHost as UIElement;
            if (cm != null)
            {
                cm.PlacementTarget = ddb;
                cm.Placement = PlacementMode.Bottom;
                if (dropDownHostElement != null)
                    dropDownHostElement.Visibility = Visibility.Hidden;
            }
            else if (ddb.dropDownHost != null)
            {
                ddb.dropDownHost.AddChild(e.NewValue);
                if (dropDownHostElement != null)
                    dropDownHostElement.Visibility = Visibility.Visible;
            }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DropDownButton),
            new UIPropertyMetadata(false, IsDropDownOpenProperty_Changed));

        /// <summary>
        /// Gets or sets the DropDown menu for the <see cref="WpfHelpers.Controls.DropDownButton"/>.
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
            if ((bool)e.NewValue)
            {
                if (!ddb.IsDropDownEnabled)
                    ddb.IsDropDownOpen = false;
                ddb.OnDropDownOpened(new RoutedEventArgs(DropDownOpenedEvent));
            }
            else
            {
                ddb.OnDropDownClosed(new RoutedEventArgs(DropDownClosedEvent));
            }
        }

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

        protected virtual void OnDropDownOpened(RoutedEventArgs e)
        {
            if (e.Handled || !IsDropDownEnabled)
                return;
            RaiseEvent(e);
        }

        public static RoutedEvent DropDownOpenedEvent = EventManager.RegisterRoutedEvent("DropDownOpened", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DropDownButton));
        
        /// <summary>
        /// Occurs when the drop-down menu is opened.
        /// </summary>
        public event RoutedEventHandler DropDownOpened
        {
            add { AddHandler(DropDownOpenedEvent, value); }
            remove { RemoveHandler(DropDownOpenedEvent, value); }
        }

        protected virtual void OnDropDownClosed(RoutedEventArgs e)
        {
            if (e.Handled)
                return;
            closingDebounce = true;
            debounceTimer.Start();
            RaiseEvent(e);
        }

        public static RoutedEvent DropDownClosedEvent = EventManager.RegisterRoutedEvent("DropDownClosed", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DropDownButton));

        /// <summary>
        /// Occurs when the drop-down menu is closed.
        /// </summary>
        public event RoutedEventHandler DropDownClosed
        {
            add { AddHandler(DropDownClosedEvent, value); }
            remove { RemoveHandler(DropDownClosedEvent, value); }
        }

        protected virtual void OnClick(RoutedEventArgs e)
        {
            if (e.Handled)
                return;
            IsDropDownOpen = false;
            RaiseEvent(e);
        }

        public static RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownButton));

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

    }
}