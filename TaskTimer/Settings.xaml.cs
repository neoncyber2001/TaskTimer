using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;

using System.Collections.ObjectModel;
using TaskTimer.Model;

namespace TaskTimer
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private SettingsVM vm;
        public Settings(ObservableCollection<Chrono> lst)
        {
            InitializeComponent();
            vm = new SettingsVM(lst);
            DataContext = vm;
            vm.CronosSaved += OnSaved;
        }
        public EventHandler<Boolean> savecomplete { get; set; }
                
        private void OnSaved(object sender, bool e)
        {
            savecomplete?.Invoke(this, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveCronos();
            this.Hide();

        }
    }
}
