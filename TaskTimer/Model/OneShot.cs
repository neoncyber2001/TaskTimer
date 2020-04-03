using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TaskTimer.Model
{
    public class OneShot
    {
        //todo
        public static SolidColorBrush RandomColor()
        {
            SolidColorBrush c = new SolidColorBrush();
            return c;
        }
        public OneShot()
        {
            SecondsTotal = 0;
            SecondsRemain = 0;
            IsRunning = false;
            Color = new SolidColorBrush(Colors.Cyan);
        }
        public double SecondsTotal { get; set; } 
        public double SecondsRemain { get; set; }
        public double PctRemaining => (SecondsRemain/SecondsTotal)*100;
        public bool IsRunning { get; set; }
        public Brush Color { get; set; }
    }
}
