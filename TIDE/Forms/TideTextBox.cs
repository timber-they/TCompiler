#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIDE.Coloring.StringFunctions;
using TIDE.Coloring.Types;
using TIDE.Forms.Tools;

#endregion

namespace TIDE.Forms
{
    public class TideTextBox : RichTextBox
    {
        //The following stuff is copied.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        // ReSharper disable once IdentifierTypo
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        private const int EmSetEventMask = 0x0400 + 69;
        private const int WmSetredraw = 0x0b;
        private const int WmUser = 0x400;
        private const int EmHideselection = WmUser + 63;
        private static IntPtr _oldEventMask;

        private int _updatingCounter;

        //Not copied anymore from here on

        public string CurrentLine()
            => Lines.Length == 0 ? "" : Lines[GetLineFromCharIndex(GetFirstCharIndexOfCurrentLine())];

        private bool _isUpdating;

        /// <summary>
        ///     colors the text in the specified area
        /// </summary>
        /// <param name="area">The area in which the text shall get colored</param>
        /// <param name="color">The new color of the text in the area</param>
        /// <param name="back">Indicates wether the background and not the foreground color shall get changed</param>
        /// <param name="resetCursor">Indicates wether to reset the cursor position to the position it was before</param>
        public void Color_FromTo(Range area, Color color, bool back = false,
            bool resetCursor = true)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Color_FromTo(area, color, back)));
                return;
            }

            HideCursor();
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
            ShowCursor();
        }

        /// <summary>
        ///     Hides the cursor
        /// </summary>
        private void HideCursor() => SendMessage(Handle, EmHideselection, 1, 0);

        /// <summary>
        ///     Shows the cursor, after it was hidden
        /// </summary>
        private void ShowCursor() => SendMessage(Handle, EmHideselection, 0, 0);

        /// <summary>
        ///     Highlights the specified line
        /// </summary>
        /// <param name="lineIndex">The lineIndex of the line that shall get highlighted</param>
        /// <param name="color">The new background color of the text</param>
        public async void HighlightLine(int lineIndex, Color color)
        {
            var text = (string) Invoke(new Func<string>(() => Text));
            var line = await GetLine(lineIndex, text);
            Color_FromTo(line, color, true, false);
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

        /// <summary>
        ///     Start updating the UI of the textBox - don't forget to call <see cref="EndUpdate"/>
        /// </summary>
        public void BeginUpdate()
        {
            if (InvokeRequired)
            {
                Invoke((Action) BeginUpdate);
                return;
            }
            _updatingCounter++;
            Console.WriteLine("Increased update counter ({0})", _updatingCounter);
            if (_isUpdating || _updatingCounter > 1)
                return;
            Console.WriteLine("    Began to update");
            _isUpdating = true;
            SendMessage(Handle, WmSetredraw, IntPtr.Zero, IntPtr.Zero);
            _oldEventMask = SendMessage(Handle, EmSetEventMask, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        ///     End updating the UI of the textBox
        /// </summary>
        public void EndUpdate()
        {
            if (InvokeRequired)
            {
                Invoke((Action) EndUpdate);
                return;
            }
            _updatingCounter--;
            Console.WriteLine($"Decreased update counter ({_updatingCounter})");
            if (!_isUpdating || _updatingCounter > 0)
                return;
            Console.WriteLine("    Ended to update");
            _isUpdating = false;
            SendMessage(Handle, WmSetredraw, (IntPtr) 1, IntPtr.Zero);
            SendMessage(Handle, EmSetEventMask, IntPtr.Zero, _oldEventMask);
        }

        public List<int> GetSelectedLines() => Enumerable.Range(SelectionStart, SelectionLength)
            .Select(GetLineFromCharIndex)
            .Distinct()
            .ToList();

        /// <summary>
        ///     Set the value of the DoubleBuffered property
        /// </summary>
        /// <param name="doubleBuffered">The new value</param>
        public void SetDoublebuffered(bool doubleBuffered) => DoubleBuffered = doubleBuffered;

        /// <summary>
        ///     Formats the whole text of the textBox
        /// </summary>
        public void Format()
        {
            var currentLine = GetLineFromCharIndex(SelectionStart);
            var trimmedCharIndexOfLine = SelectionStart - Lines[currentLine]
                                             .TakeWhile(c => c == ' ')
                                             .Count() -
                                         GetFirstCharIndexFromLine(currentLine);
            BeginUpdate();
            Text = Formatting.FormatText(Text);
            SelectionStart = GetFirstCharIndexFromLine(currentLine) + trimmedCharIndexOfLine +
                             Lines[currentLine].TakeWhile(c => c == ' ').Count();
            ColorAll();
            EndUpdate();
        }

        /// <summary>
        ///     Formats the specified lines of the TextBox
        /// </summary>
        /// <param name="lines">The lines to format</param>
        public void Format(List<int> lines)
        {
            var currentLine = GetLineFromCharIndex(SelectionStart);
            var trimmedCharIndexOfLine = SelectionStart - Lines[currentLine]
                                             .TakeWhile(c => c == ' ')
                                             .Count() -
                                         GetFirstCharIndexFromLine(currentLine);
            BeginUpdate();
            Text = Formatting.FormatLines(Text, lines);
            SelectionStart = GetFirstCharIndexFromLine(currentLine) + trimmedCharIndexOfLine +
                             Lines[currentLine].TakeWhile(c => c == ' ').Count();
            ColorAll();
            EndUpdate();
        }
    }
}