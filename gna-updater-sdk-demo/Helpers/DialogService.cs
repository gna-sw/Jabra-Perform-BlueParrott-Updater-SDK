using GNAUpdaterSDK_Demo.Logger;
using Microsoft.VisualBasic;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GNAUpdaterSDK_Demo.Helpers
{
    public static class DialogService
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        private const int WM_CLOSE = 0x10;

        public static void CloseDialogs()
        {
            try
            {
                Interaction.AppActivate(System.Diagnostics.Process.GetCurrentProcess().Id);
                SendKeys.SendWait(" ");
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.ToString());
            }

            try
            {
                IntPtr h = FindWindow("#32770", "Open");
                SendMessage(h, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.ToString());
            }

            try
            {
                IntPtr h1 = FindWindow("#32770", "Save As");
                SendMessage(h1, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.ToString());
            }

            try
            {
                IntPtr h1 = FindWindowByCaption(IntPtr.Zero, "Load Save Setting");
                SendMessage(h1, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.ToString());
            }

            try
            {
                IntPtr h1 = FindWindowByCaption(IntPtr.Zero, "Information");
                SendMessage(h1, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.ToString());
            }
        }
    }
}
