using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SharpUtils.Wpf.ViewModel;

namespace SharpUtils.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for FilePicker.xaml
    /// </summary>
    public partial class FilePicker : UserControl
    {
        FilePickerViewModel viewModel;

        public FilePicker()
        {
            InitializeComponent();
            viewModel = new FilePickerViewModel();
            grid.DataContext = viewModel;
            viewModel.PropertyChanged += viewModel_PropertyChanged;
        }

        private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == FileNameProperty.Name)
            {
                FileName = viewModel.FileName;
            }
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(FilePickerMode), typeof(FilePicker),
            new FrameworkPropertyMetadata(FilePickerMode.File, ModeProperty_Changed));

        public FilePickerMode Mode
        {
            get { return (FilePickerMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private static void ModeProperty_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((FilePicker)obj).viewModel.Mode = (FilePickerMode)e.NewValue;
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(FilePicker),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FileNameProperty_Changed));

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        private static void FileNameProperty_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((FilePicker)obj).viewModel.FileName = e.NewValue as string;
        }
    }

    public enum FilePickerMode
    {
        File,
        Folder
    }
}
