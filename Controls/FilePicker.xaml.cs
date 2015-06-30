using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Sharp.Utils.Wpf.ViewModel;

namespace Sharp.Utils.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for FilePicker.xaml
    /// </summary>
    public partial class FilePicker : UserControl
    {
        FilePickerViewModel viewModel;

        /// <summary>
        /// Creates a new <see cref="FilePicker"/> instance.
        /// </summary>
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

        /// <summary>
        /// Identifies the <see cref="Mode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(FilePickerMode), typeof(FilePicker),
            new FrameworkPropertyMetadata(FilePickerMode.File, ModeProperty_Changed));

        /// <summary>
        /// Gets or sets which type of object the current <see cref="FilePicker"/> instance represents.
        /// </summary>
        public FilePickerMode Mode
        {
            get { return (FilePickerMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private static void ModeProperty_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((FilePicker)obj).viewModel.Mode = (FilePickerMode)e.NewValue;
        }

        /// <summary>
        /// Identifies the <see cref="FileName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(FilePicker),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FileNameProperty_Changed));

        /// <summary>
        /// Gets or sets the name of the file or directory that the current <see cref="FilePicker"/> instance represents.
        /// </summary>
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

    /// <summary>
    /// Indicates the type of object for a <see cref="FilePicker"/> control.
    /// </summary>
    public enum FilePickerMode
    {
        /// <summary>
        /// Represents a file.
        /// </summary>
        File,
        /// <summary>
        /// Represents a folder that contains files.
        /// </summary>
        Folder
    }
}
