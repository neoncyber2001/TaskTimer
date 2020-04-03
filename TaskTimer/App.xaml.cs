using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using NHotkey.Wpf;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;

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
            MainVM dc = new MainVM(window);
            string pth = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + TaskTimer.Properties.Settings.Default.Datapath;
            Debug.WriteLine(pth);
            if(!Directory.Exists(pth))
            {
                Directory.CreateDirectory(pth);
            }

            if(File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + TaskTimer.Properties.Settings.Default.Datapath + TaskTimer.Properties.Settings.Default.DataFile))
            {
                TaskTimer.Properties.Settings.Default.LoadCrono = true;
                TaskTimer.Properties.Settings.Default.Save();
            }
            dc.GetBindings().ForEach(cb => window.CommandBindings.Add(cb));
            window.DataContext = dc;
            MainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            CustomCommands.StopTimer.Execute(this, null);
            TaskTimer.Properties.Settings.Default.Save();
        }
    }
}
