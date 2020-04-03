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
using System.Windows.Media;
using System.Windows.Threading;
using TaskTimer.Model;
using System.Reflection;
using WpfAppBar;
using System.Windows.Documents;
using System.Timers;
using NHotkey.Wpf;
//using Colorspace;
using System.Media;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TaskTimer
{
    class MainVM : INotifyPropertyChanged
    {


        private double ColorProgress=0;
        private Random rnd = new Random((int)DateTime.Now.Second);

        //Action<int> MyAction;
        protected MainWindow mainWin { get; set; }

        public MainVM(MainWindow _main)
        {
            if (TaskTimer.Properties.Settings.Default.LoadCrono)
            {
                LoadInit();
            }
            else {
                BasicInit(); 
            }
            this.mainWin = _main;
            
        }

        private void LoadInit()
        {
            IFormatter formatter = new BinaryFormatter();
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + TaskTimer.Properties.Settings.Default.Datapath + TaskTimer.Properties.Settings.Default.DataFile;
            Debug.WriteLine("Attempting to load from " + Path);
            Stream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.None);
            _ChronoList = (ObservableCollection<Chrono>) formatter.Deserialize(stream);
            stream.Close();
            _ChronoList.ToList().ForEach((ch) => ch.loadInit());
        }
        private void BasicInit()
        {
            AddChronoList(new Chrono(5 * 60, 10 * 60));
            AddChronoList(new Chrono(5 * 60, 10 * 60));
            AddChronoList(new Chrono(5 * 60, 10 * 60));
            AddChronoList(new Chrono(5 * 60, 10 * 60));

            ostimer.Tick += Ostimer_Tick;
            ostimer.Interval = TimeSpan.FromSeconds(1);
            ostimer.Start();
        }

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

        public void RemoveChronoList(Chrono value)
        {
            _ChronoList.Remove(value);
            NotifyPropertyChanged("ChronoList");
        }


        private ObservableCollection<OneShot> _OneShotList = new ObservableCollection<OneShot>();
        public ReadOnlyObservableCollection<OneShot> OneShotList
        {
            get => new ReadOnlyObservableCollection<OneShot>(_OneShotList);
        }
        public void Add_OneShotList(OneShot value)
        {
            _OneShotList.Add(value);
            NotifyPropertyChanged("OneShotList");
        }
        public void Remove_OneShotList(OneShot value)
        {
            _OneShotList.Remove(value);
            NotifyPropertyChanged("OneShotList");
        }



        private void NotifyTrigger(object sender, EventArgs e)
        {
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





        
        private void Ostimer_Tick(object sender, EventArgs e)
        {
            bool update = false;
            OneShotList.ToList().ForEach((os) => {
                if (os.IsRunning)
                {
                    os.SecondsRemain--;
                    Debug.WriteLine(os.SecondsRemain);
                    Debug.WriteLine(os.SecondsTotal);
                    Debug.WriteLine(os.PctRemaining);
                    if (os.SecondsRemain == 0) {
                        SystemSounds.Beep.Play();
                        os.IsRunning = false;
                        Remove_OneShotList(os);
                    }
                    update = true;
                }
            });
            if (update)
            {
                NotifyPropertyChanged("OneShotList");
            }
        }

        public List<CommandBinding> GetBindings()
        {
            List<CommandBinding> cbl = new List<CommandBinding>();
            cbl.Add(new CommandBinding(CustomCommands.Close, CloseApp_Executed));
            cbl.Add(new CommandBinding(CustomCommands.StartRestartTimer, StartRestart_Executed));
            cbl.Add(new CommandBinding(CustomCommands.StopTimer, Stop_Executed, Stop_CanExecute));
            cbl.Add(new CommandBinding(CustomCommands.ToggleDock, ToggleDock_Executed));
            cbl.Add(new CommandBinding(CustomCommands.ToggleTimer, ToggleTimer_Executed));
            cbl.Add(new CommandBinding(CustomCommands.StartTimerDlg, TimerDlgExecuted));
            cbl.Add(new CommandBinding(CustomCommands.CountdownTimerDlg, CountdownDlgExecuted));
            cbl.Add(new CommandBinding(CustomCommands.Configure, ConfigureExecuted));
            return cbl;
        }

        private void ConfigureExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _ChronoList.CollectionChanged += _ChronoList_CollectionChanged;
            Settings settingsDlg = new Settings(_ChronoList);
            settingsDlg.savecomplete += OnSaved;
            settingsDlg.ShowDialog();
            settingsDlg.savecomplete -= OnSaved;

        }

        private void OnSaved(Object sender, bool e)
        {
            NotifyPropertyChanged("ChronoList");

        }

        private void _ChronoList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TimerDlgExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionDialog dlg = new SelectionDialog("Start/Stop Chrono", "Please select the Chronomiter to start/stop (1-4).", new List<Key>(), 10.0d);
            dlg.AcceptableInput.Add(Key.D1);
            dlg.AcceptableInput.Add(Key.D2);
            dlg.AcceptableInput.Add(Key.D3);
            dlg.AcceptableInput.Add(Key.D4);
            dlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dlg.Topmost = true;
            bool? dlgres = dlg.ShowDialog();
            if (dlgres != null || dlgres == true)
            {
                //Todo
                if(dlg.DialogResult==true)Debug.WriteLine(dlg.UserResults.ToString());
                switch (dlg.UserResults)
                {
                    case (Key.D1):
                        this.Start_RestartTimer(0);
                        break;
                    case (Key.D2):
                        this.Start_RestartTimer(1);
                        break;
                    case (Key.D3):
                        this.Start_RestartTimer(2);
                        break;
                    case (Key.D4):
                        this.Start_RestartTimer(3);
                        break;
                    default:
                        break;
                }
            }
        }

        private void CountdownDlgExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionDialog dlg = new SelectionDialog("Start Countdown", "Please select the duration 1min to 10min. (Use the 0 key for 10 mins)", new List<Key>(), 10.0d);
            dlg.AcceptableInput.AddRange(SelectionVM.CommonKeysNumbersRow0to9);

            bool? dlgres = dlg.ShowDialog();
            if (dlgres == true)
            {
                //Todo
                if (dlg.DialogResult == true) Debug.WriteLine(dlg.UserResults.ToString());
                switch (dlg.UserResults)
                {
                    case (Key.D1):
                        this.StartOST(1);
                        break;
                    case (Key.D2):
                        this.StartOST(2);
                        break;
                    case (Key.D3):
                        this.StartOST(3);
                        break;
                    case (Key.D4):
                        this.StartOST(4);
                        break;
                    case (Key.D5):
                        this.StartOST(5);
                        break;
                    case (Key.D6):
                        this.StartOST(6);
                        break;
                    case (Key.D7):
                        this.StartOST(7);
                        break;
                    case (Key.D8):
                        this.StartOST(8);
                        break;
                    case (Key.D9):
                        this.StartOST(9);
                        break;
                    case (Key.D0):
                        this.StartOST(10);
                        break;

                    default:
                        break;
                }
            }
        }
        private void Ost_canExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                e.CanExecute = true;
            }
            else
            {
                //TODO:
                e.CanExecute = true;
            }
        }
        private void KillOST(object sender, ExecutedRoutedEventArgs e)
        {
            //other OSHOST
            OneShot Pressed = e.Parameter as OneShot; 
            foreach( OneShot os in OneShotList){
                if (os.Equals(Pressed))
                {
                    //Expected result achieved.
                    Remove_OneShotList(os);                   
                }
                else
                {
                    //expected results did not match
                }
            }
        }
        private void StartOST(double mins)
        {
            if (this.OneShotList.Count < 3)
            {
                Color colorB = ColorHelpers.HSVtoColor((ColorProgress%10) * 36, 255, 255);
                ColorProgress++;
                /*
                LinearGradientBrush lgb = new LinearGradientBrush();
                lgb.StartPoint = new Point(0, 1);
                lgb.EndPoint = new Point(0, -1);
                lgb.GradientStops.Add(new GradientStop(colorA, 0));
                lgb.GradientStops.Add(new GradientStop(colorB, .5));
                lgb.GradientStops.Add(new GradientStop(colorA, 1));
                */
                OneShot os = new OneShot()
                {
                    SecondsTotal = mins * 60                  
                };
                os.Color = new SolidColorBrush(colorB);
                os.IsRunning = true;
                os.SecondsRemain = os.SecondsTotal;
                Add_OneShotList(os);
            }
        }
        private void Ost_executed(object sender, ExecutedRoutedEventArgs e)
        {
            //TODO:

            if (OneShotList.Count <= 3)
            {
                StartOST((double)int.Parse((string)e.Parameter));
            }
        }

        /// <summary>
        /// Starts or restarts a timer.
        /// </summary>
        /// <param name="timerno"></param>
        private void Start_RestartTimer(int timerno){ 
            //Todo:
                
            ChronoList[timerno].Start();
        }
        /// <summary>
        /// Stops a timer.
        /// </summary>
        /// <param name="timerno"></param>
        private void Stop_Timer(int timerno)
        {
            //Todo:

            ChronoList[timerno].Start();
        }

        private void StartRestart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Todo:
            if (e.Parameter.GetType() == typeof(string)) {
                int indx = int.Parse((string)e.Parameter);
                Start_RestartTimer(indx);
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

            if (!ParrentWindow.isDocked)
            {
                ParrentWindow.Dock();
            }
            else
            {
                ParrentWindow.UnDock();
            }
        }

        private MainWindow _ParrentWindow;
        public MainWindow ParrentWindow
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
            get => (OneShotRemain / OneShotTime) * 100;
        }

        protected DispatcherTimer ostimer = new DispatcherTimer();
        protected Double OneShotTime = 3 * 60;
        protected Double OneShotRemain = 0;


        private void CloseApp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mainWin.UnDock();
            mainWin.Close() ;
        }
       





    }
}
