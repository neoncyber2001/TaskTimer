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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Diagnostics;
using System.Windows.Threading;
using NHotkey.Wpf; 
using WpfAppBar;

namespace TaskTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            HotkeyManager.Current.AddOrReplace("StartTimer", System.Windows.Input.Key.F1, ModifierKeys.Control | System.Windows.Input.ModifierKeys.Alt, OnTimerDlg);
            HotkeyManager.Current.AddOrReplace("StopTimer", System.Windows.Input.Key.F2, ModifierKeys.Control | System.Windows.Input.ModifierKeys.Alt, OnTimerDlg);
            HotkeyManager.Current.AddOrReplace("StartCountdown", System.Windows.Input.Key.F3, ModifierKeys.Control | System.Windows.Input.ModifierKeys.Alt, OnCountdnDlg );

            InitializeComponent();
            
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ((MainVM)DataContext).ChronoList.ToList().ForEach((cr) => cr.Stop());
            UnDock();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainVM)DataContext).ParrentWindow=this;
            if (TaskTimer.Properties.Settings.Default.StartDocked)
            {
                Dock();
            }
        }
        public bool isDocked { get; protected set; }
        public void Dock()
        {
            this.WindowStyle = WindowStyle.None;
            AppBarFunctions.SetAppBar(this, ABEdge.Top);
            this.DragBarThumb.Visibility = Visibility.Collapsed;
            DragBarThumb.IsEnabled = false;
            isDocked = true;
        }
        public void UnDock()
        {
            AppBarFunctions.SetAppBar(this, ABEdge.None);
            DragBarThumb.IsEnabled = true;
            isDocked = false;
            this.DragBarThumb.Visibility = Visibility.Visible;
            this.WindowStyle = WindowStyle.None;
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            this.Left += e.HorizontalChange;
            this.Top += e.VerticalChange;

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            
        }

        private void OnTimerDlg(object Sender, NHotkey.HotkeyEventArgs e)
        {
            CustomCommands.StartTimerDlg.Execute(this, this);
        }
        private void OnCountdnDlg(object Sender, NHotkey.HotkeyEventArgs e)
        {
            CustomCommands.CountdownTimerDlg.Execute(this, this);
        }
    }
}
