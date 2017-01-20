using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TCompiler.ExternalStuff
{
    public static class NativeMethods
    {

        private const int EmSetEventMask = 0x0400 + 69;
        private const int WmSetredraw = 0x0b;
        private static IntPtr _oldEventMask;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        // ReSharper disable once IdentifierTypo
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public static void BeginUpdate(RichTextBox tb)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action<RichTextBox>) BeginUpdate, tb);
                return;
            }
            SendMessage(tb.Handle, WmSetredraw, IntPtr.Zero, IntPtr.Zero);
            _oldEventMask = SendMessage(tb.Handle, EmSetEventMask, IntPtr.Zero, IntPtr.Zero);
        }

        public static void EndUpdate(RichTextBox tb)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action<RichTextBox>) EndUpdate, tb);
                return;
            }
            SendMessage(tb.Handle, WmSetredraw, (IntPtr) 1, IntPtr.Zero);
            SendMessage(tb.Handle, EmSetEventMask, IntPtr.Zero, _oldEventMask);
        }
    }
}