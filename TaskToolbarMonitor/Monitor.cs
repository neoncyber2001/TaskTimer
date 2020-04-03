using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TaskToolbarMonitor
{
    public class Monitor
    {
        private int windowPtr;

        public static int CrashThresdold { get; set; } = 2;
        public int UpdateDelay { get; set; } = 1000;
        public IntPtr WindowPtr { get => (IntPtr)windowPtr; protected set => windowPtr = (int)value; }
        public bool HasExited { get => MonProc.HasExited; }
        public bool IsResponding { get => MonProc.Responding; }
        protected Process MonProc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="delay"></param>
        public Monitor(int pid, int delay = 1000)
        {
            Console.WriteLine(SimpleLogger.instance.LogPath);
            try
            {
                MonProc = Process.GetProcessById(pid);
                MonProc.Refresh();
                Debug.WriteLine(MonProc.MainWindowTitle);
                Debug.WriteLine(MonProc.Id);
                Thread.Sleep(1000);
                this.WindowPtr = MonProc.MainWindowHandle;
            }
            catch (ArgumentException ex)
            {
                SimpleLogger.instance.LogEvent(SimpleLogger.LogLevels.Debug, ex.Message.ToString());
                throw ex;
            }
            UpdateDelay = delay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            bool looping = true;
            int crashedCount = 0;
            while (looping)
            {
                if (this.MonProc.HasExited)
                {
                    Checkreg();
                    return;
                }
                if (!this.MonProc.Responding)
                {
                    crashedCount++;
                    if (crashedCount > Monitor.CrashThresdold)
                    {
                        SimpleLogger.instance.LogEvent(SimpleLogger.LogLevels.Warning, "Process " + MonProc.Id + " seems to have crashed... attempting to kill.");
                        bool registered = Checkreg();
                        try
                        {
                            MonProc.Kill();
                        }
                        catch (Exception ex)
                        {
                            SimpleLogger.instance.LogEvent(SimpleLogger.LogLevels.Error, $"Kill Fialed: {ex.Message}");
                            throw ex;
                        }
                        return;
                    }
                }
                Thread.Sleep(UpdateDelay);
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if the window was still registered as an app bar.</returns>
        public bool Checkreg()
        {
            //todo
            return true;
        }

    }
}
