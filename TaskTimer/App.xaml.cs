using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
namespace TaskTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            MainVM dc = new MainVM();
            dc.GetBindings().ForEach(cb => window.CommandBindings.Add(cb));
            window.DataContext = dc;
            MainWindow.Show();
        }
    }
}
