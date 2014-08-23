using System;
using System.Linq;
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
        public static RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(ItemEventHandler), typeof(ButtonComboBox));

        /// <summary>
        /// Occurs when the button is clicked or a menu item is selected.
        /// </summary>
        public event ItemEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        private ButtonBase button;
        private UIElement itemsHost;
        private bool isItemClicked;
        private object clickedItem;

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
            itemsHost.PreviewMouseUp += itemsHost_PreviewMouseUp;
            base.OnApplyTemplate();
        }

        public static DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonComboBox),
            new PropertyMetadata(ClickMode.Release, ClickModeProperty_Changed));

        /// <summary>
        /// Gets or sets when the <see cref="WpfHelpers.Controls.ButtonComboBox.Click"/> event occurs.
        /// </summary>
        public ClickMode ClickMode
        {
            get { return (ClickMode)GetValue(ClickModeProperty); }
            set { SetValue(ClickModeProperty, value); }
        }

        private static void ClickModeProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bcb = d as ButtonComboBox;
            if (bcb != null && bcb.button != null)
                bcb.button.ClickMode = (ClickMode)e.NewValue;
        }

        public static DependencyProperty AllowSelectedItemChangeProperty = DependencyProperty.Register("AllowSelectedItemChange", typeof(bool), typeof(ButtonComboBox),
            new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets whether the user is allowed to change the selected item.
        /// </summary>
        public bool AllowSelectedItemChange
        {
            get { return (bool)GetValue(AllowSelectedItemChangeProperty); }
            set { SetValue(AllowSelectedItemChangeProperty, value); }
        }

        private bool ignoreSelectionChanged;

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (ignoreSelectionChanged)
                return;
            if (!AllowSelectedItemChange)
            {
                if (e.RemovedItems.Count > 0)
                {
                    ignoreSelectionChanged = true;
                    SelectedItem = e.RemovedItems[0];
                    ignoreSelectionChanged = false;
                }
                return;
            }
            base.OnSelectionChanged(e);
        }

        private void itemsHost_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource == null)
                return;
            var cbItem = originalSource.Ancestor<ComboBoxItem>();
            if (cbItem == null)
                return;
            clickedItem = (ItemsSource != null) ? cbItem.DataContext : cbItem;
            isItemClicked = true;
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            if (isItemClicked)
            {
                RaiseClick(clickedItem);
                isItemClicked = false;
            }
        }

        protected void RaiseClick(object item)
        {
            OnClick(new ItemEventArgs(item, ClickEvent));
        }

        protected virtual void OnClick(RoutedEventArgs e)
        {
            if (e.Handled)
                return;
            RaiseEvent(e);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            RaiseClick(SelectedItem);
        }
    }
}
