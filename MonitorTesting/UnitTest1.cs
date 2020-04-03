using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskToolbarMonitor;

namespace MonitorTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Process NotepadProc = Process.Start("Notepad.exe");
            TaskToolbarMonitor.Monitor mn = new TaskToolbarMonitor.Monitor(NotepadProc.Id);
            Thread.Sleep(500);
            Assert.AreEqual(NotepadProc.HasExited, mn.HasExited);
            Assert.AreEqual(NotepadProc.MainWindowHandle, mn.WindowPtr);
            Thread.Sleep(500);
            NotepadProc.Kill();
            Thread.Sleep(1000);
            Assert.AreEqual(true, mn.HasExited);
            NotepadProc.Refresh();
            Assert.AreNotEqual(NotepadProc.MainWindowHandle, mn.WindowPtr);
        }
    }
}
