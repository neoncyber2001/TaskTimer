using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TaskTimer.Model;
using WpfAppBar;
namespace TaskTimer
{
    class MainVM:INotifyPropertyChanged
    {

        Action<int> MyAction;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        private ObservableCollection<Chrono> _ChronoList = new ObservableCollection<Chrono>();
        public ReadOnlyObservableCollection<Chrono> ChronoList
        {
            get => new ReadOnlyObservableCollection<Chrono>(_ChronoList);
        }
        public void AddChronoList(Chrono value)
        {
            if (_ChronoList.Count < 9)
            {
                value.TimerTick += NotifyTrigger;
                _ChronoList.Add(value);
                NotifyPropertyChanged("ChronoList");
            }
        }

        private void NotifyTrigger(object sender, EventArgs e)
        {
            NotifyPropertyChanged("ChronoList");
        }

        public void RemoveChronoList(Chrono value)
        {
            _ChronoList.Remove(value);
            NotifyPropertyChanged("ChronoList");
        }

        
        private bool _IsDocked;
        public bool IsDocked
        {
            get => _IsDocked;
            set
            {
                _IsDocked = value;
                NotifyPropertyChanged();
            }
        }

        
        public MainVM()
        {
            AddChronoList(new Chrono(5*60,10*60));
            AddChronoList(new Chrono(5*60, 10*60));
            ostimer.Interval = TimeSpan.FromMilliseconds(1000);
            ostimer.Tick += Ostimer_Tick;
        }

        private void Ostimer_Tick(object sender, EventArgs e)
        {
            if(OneShotRemain > 0)
            {
                OneShotRemain--;
            }

        }

        public List<CommandBinding> GetBindings()
        {
            List<CommandBinding> cbl = new List<CommandBinding>();
            cbl.Add(new CommandBinding(CustomCommands.StartRestartTimer, StartRestart_Executed));
            cbl.Add(new CommandBinding(CustomCommands.StartRestartTimer, Stop_Executed, Stop_CanExecute));
            cbl.Add(new CommandBinding(CustomCommands.ToggleDock,ToggleDock_Executed));
            cbl.Add(new CommandBinding(CustomCommands.ToggleTimer, ToggleTimer_Executed));
            return cbl;
        }

        private void StartRestart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Todo:
            if(e.Parameter.GetType() == typeof(string)) {
                int indx = int.Parse((string)e.Parameter);
                ChronoList[indx].Start();
            }
            else
            {
                Debug.WriteLine(e.Parameter.GetType().ToString());
            }
            
        }


        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Todo:
            Chrono chr = e.Parameter as Chrono;
            if (chr.IsRunning)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Chrono chr = e.Parameter as Chrono;
            chr.Stop();
        }

        private void ToggleDock_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            if (!IsDocked)
            {
                ParrentWindow.WindowStyle = WindowStyle.None;
                AppBarFunctions.SetAppBar(ParrentWindow, ABEdge.Top);
                IsDocked = true;
            }
            else
            {
                AppBarFunctions.SetAppBar(ParrentWindow, ABEdge.None);
                ParrentWindow.WindowStyle = WindowStyle.ToolWindow;
                IsDocked = false;
            }
        }

        private Window _ParrentWindow;
        public Window ParrentWindow
        {
            get => _ParrentWindow;
            set
            {
                _ParrentWindow = value;
                NotifyPropertyChanged();
            }
        }


        private void ToggleTimer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Todo:
            Chrono chr = e.Parameter as Chrono;
            if (chr.IsRunning)
            {
                chr.Stop();
            }
            else
            {
                chr.Start();
            }
        }

        public double OneShotProgress
        {
            get => (OneShotRemain/OneShotTime)*100;
        }

        protected DispatcherTimer ostimer = new DispatcherTimer();
        protected Double OneShotTime = 3*60;
        protected Double OneShotRemain = 0;

        public void SetOneShotTime(double t)
        {
            OneShotTime = t;
            OneShotRemain = t;
        }

    }
}
