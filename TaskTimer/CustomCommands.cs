using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskTimer
{
    public static class CustomCommands
    {

        ///ToDo: Custom Command SetDuration
        public static readonly RoutedUICommand SetDuration = new RoutedUICommand("SetDuration", "SetDuration", typeof(CustomCommands));
        public static readonly RoutedUICommand Close = new RoutedUICommand("Close", "Close", typeof(CustomCommands));
        public static readonly RoutedUICommand ToggleDock = new RoutedUICommand("Toggle Dock", "ToggleDock", typeof(CustomCommands));
        public static readonly RoutedUICommand OneshotTimer = new RoutedUICommand("One Shot Timer", "OneshotTimer", typeof(CustomCommands));
        public static readonly RoutedUICommand ToggleTimer = new RoutedUICommand("Toggle Timer", "ToggleTimer", typeof(CustomCommands));
        public static readonly RoutedUICommand StartRestartTimer = new RoutedUICommand(@"Start / Restart Timer", "StartRestartTimer", typeof(CustomCommands));
        public static readonly RoutedUICommand StopTimer = new RoutedUICommand("Stop Timer", "StopTimer", typeof(CustomCommands));
        public static readonly RoutedUICommand Configure = new RoutedUICommand("Configure", "Configure", typeof(CustomCommands));

        ///ToDo: Custom Command StartTimerDlg
        public static readonly RoutedUICommand StartTimerDlg = new RoutedUICommand("StartTimerDlg", "StartTimerDlg", typeof(CustomCommands));
        public static readonly RoutedUICommand StopTimerDlg = new RoutedUICommand("StartTimerDlg", "StartTimerDlg", typeof(CustomCommands));
        public static readonly RoutedUICommand CountdownTimerDlg = new RoutedUICommand("CountdownTimerDlg", "CountdownTimerDlg", typeof(CustomCommands));

    }
}
