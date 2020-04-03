using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TaskTimer;

namespace TaskTimer.Model
{
    /**
     * Class to hold chronomiter data.
     */
    [Serializable]
    public class Chrono
    {
        [NonSerialized]
        private DispatcherTimer myTimer;
        [NonSerialized]
        private double time;
        [NonSerialized]
        private AlertLevels alertLevel = AlertLevels.NoAlarm;

        /**
* Constructor
*/
        public Chrono()
        {
            loadInit();
        }

        public void loadInit()
        {
            MyTimer = new DispatcherTimer();
            MyTimer.Tick += Chrono_TimerTick;
            MyTimer.Interval = TimeSpan.FromSeconds(1);
        }

        /**
         * Constructor
         * <param name="redt">Default time for red alert</param>
         * <param name="yellowt">Default time for Yellow alert</param>
         */
        public Chrono(double yellowt, double redt)
        {
            MyTimer = new DispatcherTimer();
            this.TimerTick += Chrono_TimerTick;
            this.YellowAlert = yellowt;
            this.RedAlert = redt;
        }

        virtual protected void Chrono_TimerTick(object sender, EventArgs e)
        {
            this.Time++;
            if (this.Time <= YellowAlert && this.AlertLevel != AlertLevels.NoAlarm)
            {
                this.AlertLevel = AlertLevels.NoAlarm;
            }
            else if (this.Time > YellowAlert && this.Time <= RedAlert && this.AlertLevel != AlertLevels.YellowAlret)
            {
                this.AlertLevel = AlertLevels.YellowAlret;
            }
            else if (this.Time > RedAlert && this.AlertLevel != AlertLevels.RedAlert)
            {
                this.AlertLevel = AlertLevels.RedAlert;
            }
        }

        public void Start()
        {
            if (MyTimer.IsEnabled)
            {
                MyTimer.Stop();
            }
            Time = 0d;
            MyTimer.Interval = TimeSpan.FromMilliseconds(1000);
            MyTimer.Start();
        }
        public void Stop()
        {
            MyTimer.Stop();
            Time = 0d;
        }

        public DispatcherTimer MyTimer { get => myTimer; set => myTimer = value; }
        public event EventHandler TimerTick
        {
            add { MyTimer.Tick += value; }
            remove { MyTimer.Tick -= value; }
        }
        public double Time { get => time; set => time = value; }
        public double YellowAlert { get; set; }
        public double RedAlert { get; set; }

        public double YellowAlert_M { get { return YellowAlert / 60; } set { YellowAlert = value * 60; } }
        public double RedAlert_M { get { return RedAlert / 60; } set { RedAlert = value * 60; } }
        public string Label { get; set; } = "Chronometer";
        public TimeSpan redTs
        {
            get
            {
                return TimeSpan.FromSeconds(RedAlert);
            }
            set
            {
                RedAlert = value.Seconds;
            }
        }
        public TimeSpan yrllowTs
        {
            get
            {
                return TimeSpan.FromSeconds(YellowAlert);
            }
            set
            {
                RedAlert = value.Seconds;
            }
        }
        public string DisplayTime
        {
            get
            {
                return TimeSpan.FromSeconds(Time).ToString();
            }
        }

        public bool IsRunning { get => MyTimer.IsEnabled; }

        public AlertLevels AlertLevel { get => alertLevel; set => alertLevel = value; }
    }
}