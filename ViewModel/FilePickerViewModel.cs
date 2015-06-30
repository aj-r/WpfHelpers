using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using Sharp.Utils.ObjectModel;
using Sharp.Utils.Wpf.Controls;

namespace Sharp.Utils.Wpf.ViewModel
{
    internal class FilePickerViewModel : NotifyPropertyChangedBase
    {
        private string fileName;

        public string FileName {
            get
            {
                return fileName;
            }
            set
            {
                if (fileName == value)
                    return;
                fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        private FilePickerMode mode;

        public FilePickerMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                if (mode == value)
                    return;
                mode = value;
                RaisePropertyChanged(() => Mode);
            }
        }

        public ICommand SelectFileCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Mode == FilePickerMode.File)
                    {
                        var dialog = new OpenFileDialog();
                        dialog.Multiselect = false;
                        dialog.FileName = FileName;
                        var result = dialog.ShowDialog();
                        if (result == true)
                        {
                            FileName = dialog.FileName;
                        }
                    }
                    else
                    {
                        // Folder mode
                        var dialog = new System.Windows.Forms.FolderBrowserDialog();
                        dialog.SelectedPath = FileName;
                        var result = dialog.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            FileName = dialog.SelectedPath;
                        }
                    }
                });
            }
        }
    }
}
