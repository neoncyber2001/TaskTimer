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

        public static readonly RoutedUICommand Close = new RoutedUICommand("Close", "Close", typeof(CustomCommands));
        public static readonly RoutedUICommand ToggleDock = new RoutedUICommand("Toggle Dock", "ToggleDock", typeof(CustomCommands));
        public static readonly RoutedUICommand OneshotTimer = new RoutedUICommand("One Shot Timer", "OneshotTimer", typeof(CustomCommands));
        public static readonly RoutedUICommand ToggleTimer = new RoutedUICommand("Toggle Timer", "ToggleTimer", typeof(CustomCommands));
        public static readonly RoutedUICommand StartRestartTimer = new RoutedUICommand(@"Start / Restart Timer", "StartRestartTimer", typeof(CustomCommands));
        public static readonly RoutedUICommand StopTimer = new RoutedUICommand("Stop Timer", "StopTimer", typeof(CustomCommands));
    }
}
