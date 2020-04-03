using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Reflection;
namespace TaskToolbarMonitor
{

    public sealed class SimpleLogger
    {
        private static readonly SimpleLogger _instance = new SimpleLogger(true);

        public static SimpleLogger instance
        {
            get
            {
                return _instance;
            }
        }

        public enum LogLevels {
            None,
            Error,
            Warning,
            Notice,
            Info,
            Debug,
            Verbose
        }

        public enum MsgTypes {
            Application,
            System,
            Environment,
            Runtime,
            IO,
            Network,
            Server
        };

        public LogLevels LoggingLevel { get; set; }
        public bool IsEchoing { get; set; }
        public String LogFileName { get; set; }
        public String LogFilePath { get; set; }
        public bool IsLoggingOn { get; set; }
        public String LogPath { get => LogFilePath + LogFileName; }
        private SimpleLogger(bool initEntry)
        {
            LoggingLevel = LogLevels.Notice;
            IsEchoing = false;
            LogFileName = Path.GetTempFileName();
            LogFilePath = Path.GetTempPath();
            if (initEntry)
            {
                String now = DateTime.Now.ToString();
                String sev = getLevelName(LogLevels.None);
                String typ = "System";
                string[] ConStr = { $"[{now}] - SEVERITY:[{sev}] - TYPE[{typ}] - MESSEGE[BEGIN EVENT LOGGING AT ERROR LVEL {getLevelName(LoggingLevel)}]" };
                ConStr.ToList().ForEach((str) => Console.WriteLine(str));
                File.WriteAllLines(LogPath, ConStr);
            }
        }

        public void LogEvent(LogLevels sev, String EventMessege)
        {
            this.LogEvent(sev, "Application", EventMessege);
        }

        public void LogEvent(LogLevels sev, String Type, String EventMessege)
        {
            String ConstructedMessege = "[" + DateTime.Now.ToString() + "] - SEVERITY:[" + getLevelName(sev) + "] - CATEGORY ["+ Type +"] - Messege [" + EventMessege + "]";
            string[] msgs = { ConstructedMessege };
            msgs.ToList().ForEach((str) => Console.WriteLine(str));
            File.AppendAllLines(LogPath, msgs);
        }

        public string getLevelName(LogLevels level) {
            string nOut;
            switch (level){
                case LogLevels.None:
                    nOut = "None";
                    break;
                case LogLevels.Error:
                    nOut = "Error";
                    break;
                case LogLevels.Warning:
                    nOut = "Warning";
                    break;
                case LogLevels.Notice:
                    nOut = "Notice";
                    break;
                case LogLevels.Info:
                    nOut = "Info";
                    break;
                case LogLevels.Debug:
                    nOut = "Debug";
                    break;
                default:
                    nOut = "Verbose";
                    break;
            }
            return nOut;
        }
    }
}
