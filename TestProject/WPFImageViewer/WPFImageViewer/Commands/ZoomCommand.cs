using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFImageViewer.ViewModels;

namespace WPFImageViewer.Commands
{
    public enum ZoomType
    {
        ZoomIn = 0,
        ZoomOut = 1,
        Normal = 2
    }
    public class ZoomCommand : ICommand
    {
        private MainViewModel _data;
        public ZoomCommand(MainViewModel data)
        {
            _data = data;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return _data.ImagePath != null;
        }
        public void Execute(object parameter)
        {
            ZoomType type = (ZoomType)Enum.Parse(typeof(ZoomType), (string)parameter, true);
            switch (type)
            {
                case ZoomType.Normal:
                    _data.Zoom = 1;
                    break;
                case ZoomType.ZoomIn:
                    _data.Zoom *= 1.2;
                    break;
                case ZoomType.ZoomOut:
                    _data.Zoom /= 1.2;
                    break;
            }
        }
    }
}
