using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskTimer.Model;
using TaskTimer.Properties;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

namespace TaskTimer
{
    [Serializable]
    public class SettingsVM : INotifyPropertyChanged
    {
        public SettingsVM()
        {
            Add_cronoList(new Chrono(5 * 60, 10 * 60));
            Add_cronoList(new Chrono(5 * 60, 10 * 60));
            Add_cronoList(new Chrono(5 * 60, 10 * 60));
            Add_cronoList(new Chrono(5 * 60, 10 * 60));
        }
        public SettingsVM(ObservableCollection<Chrono> lst)
        {
            _cronoList = lst;
            NotifyPropertyChanged("cronoList");
        }
        object lo = new object();
        private ObservableCollection<Chrono> _cronoList = new ObservableCollection<Chrono>();
        public ReadOnlyObservableCollection<Chrono> cronoList
        {
            get => new ReadOnlyObservableCollection<Chrono>(_cronoList);
        }
        public void Add_cronoList(Chrono value)
        {
            lock (lo)
            {
                _cronoList.Add(value);
                NotifyPropertyChanged("cronoList");
            }
        }
        public void Remove_cronoList(Chrono value)
        {
            lock (lo)
            {
                _cronoList.Remove(value);
                NotifyPropertyChanged("cronoList");
            }
        }

        private void NotifyPropertyChanged([CallerMemberName]String PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;



        public Boolean StartDocked
        {
            get => Properties.Settings.Default.StartDocked;
            set
            {
                Properties.Settings.Default.StartDocked = value;
                NotifyPropertyChanged();
            }
        }



        public Boolean AutoHide
        {
            get => Properties.Settings.Default.AutoHide;
            set
            {
                Properties.Settings.Default.AutoHide = value;
                NotifyPropertyChanged();
            }
        }



        public int KeySequenceTimeout
        {
            get => Properties.Settings.Default.KeySeqTimeout;
            set
            {
                Properties.Settings.Default.KeySeqTimeout = value;
                NotifyPropertyChanged();
            }
        }

        
        public int MaxHoldTimers
        {
            get => Properties.Settings.Default.MaxCountdown;
            set
            {
                Properties.Settings.Default.MaxCountdown = value;
                NotifyPropertyChanged();
            }
        }

        public void SaveCronos()
        {
            //Task tsk = new Task(() =>
            //{
                lock (lo)
                {
                    
                    Debug.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Properties.Settings.Default.Datapath + Properties.Settings.Default.DataFile);
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Properties.Settings.Default.Datapath + Properties.Settings.Default.DataFile, FileMode.Create);
                    formatter.Serialize(stream, _cronoList);
                    stream.Close();
                    CronosSaved?.Invoke(this, true);
                }
            //});
            //await tsk;
        }

        public EventHandler<bool> CronosSaved { get; set; }
    }

}
