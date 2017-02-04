#region

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIDE.Coloring.StringFunctions;
using TIDE.Coloring.Types;

#endregion

namespace TIDE.Forms
{
    public class TideTextBox : RichTextBox
    {
        //The following stuff is copied.

        private const int EmSetEventMask = 0x0400 + 69;
        private const int WmSetredraw = 0x0b;
        private static IntPtr _oldEventMask;

        private bool _isUpdating;

        /// <summary>
        ///     colors the text in the specified area
        /// </summary>
        /// <param name="area">The area in which the text shall get colored</param>
        /// <param name="color">The new color of the text in the area</param>
        /// <param name="back">Indicates wether the background and not the foreground color shall get changed</param>
        /// <param name="resetCursor">Indicates wether to reset the cursor position to the position it was before</param>
        public void color_FromTo(Range area, Color color, bool back = false,
            bool resetCursor = true)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => color_FromTo(area, color, back)));
                return;
            }

            var pos = SelectionStart;
            Select(area.Beginning, area.Ending - area.Beginning);
            if ((!back || SelectionBackColor != color) && (back || SelectionColor != color))
                if (!back)
                    SelectionColor = color;
                else
                    SelectionBackColor = color;
            if (resetCursor)
                Select(pos, 0);
            else if (area.Beginning > 0)
                Select(area.Beginning - 1, 0);
            else if (Text.Length > area.Beginning + 1)
                Select(area.Beginning + 1, 0);
            else
                Select(0, 0);
            SelectionColor = ForeColor;
            if (SelectionBackColor != BackColor)
                SelectionBackColor = BackColor;
        }

        /// <summary>
        ///     Highlights the specified line
        /// </summary>
        /// <param name="lineIndex">The lineIndex of the line that shall get highlighted</param>
        /// <param name="color">The new background color of the text</param>
        public async void HighlightLine(int lineIndex, Color color)
        {
            var text = (string) Invoke(new Func<string>(() => Text));
            var line = await GetLine(lineIndex, text);
            color_FromTo(line, color, true, false);
        }

        /// <summary>
        ///     Evaluates the line area of the specified lineIndex
        /// </summary>
        /// <param name="lineIndex">The line index of the line of which the area is wanted</param>
        /// <param name="text">The text of which the area will be</param>
        /// <returns></returns>
        private static async Task<Range> GetLine(int lineIndex, string text) => await Task.Run(() =>
        {
            var pos = 0;
            for (var i = 0; i < lineIndex; i++)
                pos += text.Split('\n')[i].Length + 1;
            return new Range(pos, pos + text.Split('\n')[lineIndex].Length);
        });

        /// <summary>
        ///     Colors the whole document
        /// </summary>
        /// <param name="asm">Indicates wether assembler code is colored</param>
        public void ColorAll(bool asm = false) => Task.Run(() =>
        {
            BeginUpdate();
            foreach (var c in GetCurrent.GetAllChars(this))
                Coloring.Coloring.CharActions(c, this);
            foreach (var word in GetCurrent.GetAllWords(this))
                Coloring.Coloring.WordActions(word, this, asm);
            EndUpdate();
        });

        /// <summary>
        ///     Colors the current line
        /// </summary>
        public void ColorCurrentLine()
        {
            BeginUpdate();
            foreach (var c in GetCurrent.GetCurrentLineChars(this))
                Coloring.Coloring.CharActions(c, this);
            foreach (var word in GetCurrent.GetCurrentLineWords(this))
                Coloring.Coloring.WordActions(word, this);
            EndUpdate();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        // ReSharper disable once IdentifierTypo
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public void BeginUpdate()
        {
            if (InvokeRequired)
            {
                Invoke((Action) BeginUpdate);
                return;
            }
            if (_isUpdating)
                return;
            _isUpdating = true;
            SendMessage(Handle, WmSetredraw, IntPtr.Zero, IntPtr.Zero);
            _oldEventMask = SendMessage(Handle, EmSetEventMask, IntPtr.Zero, IntPtr.Zero);
        }

        public void EndUpdate()
        {
            if (InvokeRequired)
            {
                Invoke((Action) EndUpdate);
                return;
            }
            if (!_isUpdating)
                return;
            _isUpdating = false;
            SendMessage(Handle, WmSetredraw, (IntPtr) 1, IntPtr.Zero);
            SendMessage(Handle, EmSetEventMask, IntPtr.Zero, _oldEventMask);
        }
    }
}