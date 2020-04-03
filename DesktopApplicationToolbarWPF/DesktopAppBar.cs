using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows;
using System.Windows.Forms;
using System.Data;
//using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Drawing;
using System.Runtime.Serialization;
namespace DesktopApplicationToolbarWPF
{
 
    [Serializable]
    class DesktopAppBar
    {
        public bool IsRegistered { get; set; } = false;
        public int uCallBack { get; protected set;}
        public IntPtr WindowHandle { get; set; }
        public ABEdge DockingEdge { get; set; } = ABEdge.ABE_TOP;
        public System.Drawing.Rectangle Dimensions { get; set; }

        //To Do:
        //https://www.codeproject.com/Articles/6741/AppBar-using-C

        public DesktopAppBar(Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            WindowHandle = helper.Handle;
            Dimensions = new Rectangle((int)window.Left, (int)window.Top, (int)window.ActualWidth, (int)window.ActualHeight);
        }

        public DesktopAppBar(IntPtr hwnd, Rectangle dimensions)
        {
            WindowHandle = hwnd;
            Dimensions = dimensions;
        }

        //RECT WINAPI structure
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        //APPBARDATA SHELLAPI structure
        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        //Enums
        public enum ABMsg : int
        {
            ABM_NEW = 0,
            ABM_REMOVE,
            ABM_QUERYPOS,
            ABM_SETPOS,
            ABM_GETSTATE,
            ABM_GETTASKBARPOS,
            ABM_ACTIVATE,
            ABM_GETAUTOHIDEBAR,
            ABM_SETAUTOHIDEBAR,
            ABM_WINDOWPOSCHANGED,
            ABM_SETSTATE
        }

        public enum ABNotify : int
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }

        public enum ABEdge : int
        {
            ABE_LEFT = 0,
            ABE_TOP,
            ABE_RIGHT,
            ABE_BOTTOM
        }

        //WIN32 and SHELL API functions import
        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
        public static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);
        [DllImport("USER32")]
        public static extern int GetSystemMetrics(int Index);
        [DllImport("User32.dll", ExactSpelling = true,
            CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool MoveWindow
            (IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage(string msg);

        private void RegisterBar()
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = WindowHandle;
            if (!IsRegistered)
            {
                uCallBack = RegisterWindowMessage("AppBarMessage");
                abd.uCallbackMessage = uCallBack;

                uint ret = SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd);
                IsRegistered = true;

                ABSetPos(DockingEdge);
            }
            else
            {
                SHAppBarMessage((int)ABMsg.ABM_REMOVE, ref abd);
                IsRegistered = false;
            }
        }
        
        private void ABSetPos(ABEdge edge)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = WindowHandle;
            abd.uEdge = (int)edge;

            if (abd.uEdge == (int)ABEdge.ABE_LEFT || abd.uEdge == (int)ABEdge.ABE_RIGHT)
            {
                abd.rc.top = 0;
                abd.rc.bottom = SystemInformation.PrimaryMonitorSize.Height;
                if (abd.uEdge == (int)ABEdge.ABE_LEFT)
                {
                    abd.rc.left = 0;
                    abd.rc.right = Dimensions.Width;
                }
                else
                {
                    abd.rc.right = SystemInformation.PrimaryMonitorSize.Width;
                    abd.rc.left = abd.rc.right - Dimensions.Width;
                }

            }
            else
            {
                abd.rc.left = 0;
                abd.rc.right = SystemInformation.PrimaryMonitorSize.Width;
                if (abd.uEdge == (int)ABEdge.ABE_TOP)
                {
                    abd.rc.top = 0;
                    abd.rc.bottom = Dimensions.Height;
                }
                else
                {
                    abd.rc.bottom = SystemInformation.PrimaryMonitorSize.Height;
                    abd.rc.top = abd.rc.bottom - Dimensions.Height;
                }
            }

            SHAppBarMessage((int)ABMsg.ABM_QUERYPOS, ref abd);

            switch (abd.uEdge)
            {
                case (int)ABEdge.ABE_LEFT:
                    abd.rc.right = abd.rc.left + Dimensions.Width;
                    break;
                case (int)ABEdge.ABE_RIGHT:
                    abd.rc.left = abd.rc.right - Dimensions.Width;
                    break;
                case (int)ABEdge.ABE_TOP:
                    abd.rc.bottom = abd.rc.top + Dimensions.Height;
                    break;
                case (int)ABEdge.ABE_BOTTOM:
                    abd.rc.top = abd.rc.bottom - Dimensions.Height;
                    break;
            }

            SHAppBarMessage((int)ABMsg.ABM_SETPOS, ref abd);
            MoveWindow(abd.hWnd, abd.rc.left, abd.rc.top,
                    abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top, true);
        }
    }
}
