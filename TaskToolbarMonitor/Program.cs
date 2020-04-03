using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WpfAppBar;
using System.Threading;
namespace TaskToolbarMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0) {
                Monitor tbmon = new Monitor(Int32.Parse(args[0]));
                tbmon.Run();
            }
             


        }
    }
}
