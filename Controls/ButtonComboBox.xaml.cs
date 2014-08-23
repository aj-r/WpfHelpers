using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WpfHelpers.Controls
{
    /// <summary>
    /// Interaction logic for ButtonComboBox.xaml
    /// </summary>
    [TemplatePart(Name = "PART_Button")]
    [TemplatePart(Name = "PART_ItemsHost")]
    public partial class ButtonComboBox : ComboBox
    {
        /// <summary>
        /// Occurs when the button is clicked or a menu item is selected.
        /// </summary>
        public event RoutedEventHandler Click;
        public static RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ButtonComboBox));

        private ButtonBase button;
        private UIElement itemsHost;
        private bool itemClicked;

        public ButtonComboBox()
        {
            DefaultStyleKey = typeof(ButtonComboBox);
        }

        public override void OnApplyTemplate()
        {
            button = GetTemplateChild("PART_Button") as ButtonBase;
            button.ClickMode = ClickMode;
            button.Click += button_Click;
            itemsHost = GetTemplateChild("PART_ItemsHost") as UIElement;
            itemsHost.MouseUp += itemsHost_MouseUp;
            base.OnApplyTemplate();
        }

        public static DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonComboBox),
            new PropertyMetadata(ClickMode.Release, ClickModeProperty_Changed));

        public ClickMode ClickMode
        {
            get { return (ClickMode)GetValue(ClickModeProperty); }
            set { SetValue(ClickModeProperty, value); }
        }

        private static void ClickModeProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bcb = (ButtonComboBox)d;
            if (bcb.button != null)
                bcb.button.ClickMode = (ClickMode)e.NewValue;
        }

        private void itemsHost_MouseUp(object sender, MouseButtonEventArgs e)
        {
            itemClicked = true;
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            if (itemClicked)
            {
                RaiseClick();
                itemClicked = false;
            }
        }

        protected void RaiseClick()
        {
            OnClick(new RoutedEventArgs(ClickEvent));
        }

        protected virtual void OnClick(RoutedEventArgs e)
        {
            if (e.Handled)
                return;
            RaiseEvent(e);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            RaiseClick();
        }
    }
}
