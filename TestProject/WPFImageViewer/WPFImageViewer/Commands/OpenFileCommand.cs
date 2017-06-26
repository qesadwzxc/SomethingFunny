using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using WPFImageViewer.ViewModels;

namespace WPFImageViewer.Commands
{
    public class OpenFileCommand : ICommand
    {
        private MainViewModel _data;
        public OpenFileCommand(MainViewModel data)
        {
            _data = data;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "Image Files|*.jpg;*.png;*.bmp;*.gif" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                _data.ImagePath = dialog.FileName;
            }
        }
    }
}
