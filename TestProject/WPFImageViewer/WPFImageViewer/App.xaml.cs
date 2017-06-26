using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFImageViewer.ViewModels;

namespace WPFImageViewer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            MainWindow window = new MainWindow();
            window.DataContext = new MainViewModel();
            window.Show();
        }
    }
}
