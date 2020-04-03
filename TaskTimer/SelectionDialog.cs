using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskTimer
{
    /// <summary>
    /// Interaction logic for Selection_Dialog.xaml
    /// </summary>
    public partial class SelectionDialog : Window
    {
        Timer closeTimer = new Timer();
        SelectionVM vm = new SelectionVM();
        public SelectionDialog()
        {
            vm.WindowText = WindowText;
            vm.WindowTitle = WindowTitle;
            vm.AcceptableInput = AcceptableInput;
            this.DataContext = vm;
            InitializeComponent();
        }

        public SelectionDialog(string title, string text, List<Key> input, double timeout_s )
        {
            vm.WindowText = text;
            vm.WindowTitle = title;
            vm.AcceptableInput = input;
            Timeout = timeout_s;
            this.DataContext = vm;
            InitializeComponent();
            closeTimer.Interval = timeout_s*1000;
            closeTimer.Elapsed += closeTimerEllapsed;
            closeTimer.AutoReset = false;
            closeTimer.Start();
        }

        private void closeTimerEllapsed(object sender, ElapsedEventArgs e)
        {
           //todo;
        }

        public List<Key> AcceptableInput
        {
            get { return (List<Key>)GetValue(AcceptableInputProperty); }
            set { SetValue(AcceptableInputProperty, value); vm.AcceptableInput = value; }
        }

        // Using a DependencyProperty as the backing store for AcceptableInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AcceptableInputProperty =
            DependencyProperty.Register("AcceptableInput", typeof(List<Key>), typeof(SelectionDialog), new PropertyMetadata(new List<Key>()));



        public String WindowTitle
        {
            get { return (String)GetValue(WindowTitleProperty); }
            set { SetValue(WindowTitleProperty, value); vm.WindowTitle=value; }
        }

        // Using a DependencyProperty as the backing store for WindowTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowTitleProperty =
            DependencyProperty.Register("WindowTitle", typeof(String), typeof(SelectionDialog), new PropertyMetadata("Title"));



        public String WindowText
        {
            get { return (String)GetValue(WindowTextProperty); }
            set { SetValue(WindowTextProperty, value); vm.WindowText = value; }
        }

        // Using a DependencyProperty as the backing store for WindowText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowTextProperty =
            DependencyProperty.Register("WindowText", typeof(String), typeof(SelectionDialog), new PropertyMetadata("Text"));



        public Double Timeout
        {
            get { return (Double)GetValue(TimeoutProperty); }
            set { SetValue(TimeoutProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Timeout.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeoutProperty =
            DependencyProperty.Register("Timeout", typeof(Double), typeof(SelectionDialog), new PropertyMetadata(30.0d));

        public object UserResults { get; protected set; } = null;

        private HashSet<Key> Pressed = new HashSet<Key>();
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (Pressed.Contains(e.Key))
            {
                Pressed.Remove(e.Key);
                if (this.AcceptableInput.Contains(e.Key))
                {
                    this.DialogResult = true;
                    UserResults = e.Key;
                 
                }
                this.Hide();
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Pressed.Add(e.Key);
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            closeTimer.Stop();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.Hide();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
