using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MetaTextBox
{
    public class MetaTextBox : Control
    {
        public override string Text { get; set; }
        protected override bool DoubleBuffered { get; set; } = true;
        public override Cursor Cursor { get; set; } = Cursors.IBeam;

        private static int UpScaleFactor = 1;
        private int startingLine;

        private int? _characterWidth;
        private int _cursorX;
        private int _cursorY;

        public int CursorPosition
        {
            set
            {
                var coordinates = GetCursorCoordinates(value);
                if (!coordinates.HasValue)
                    return;
                _cursorX = coordinates.Value.X;
                _cursorY = coordinates.Value.Y;
            }//TODO: get
        }

        private int GetCharacterWidth() => _characterWidth ?? (int) CreateGraphics().MeasureString(" ", Font).Width;

        public List<string> Lines { get; private set; }

        #region MainFunctions

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            var key = e.KeyChar;
            e.Handled = true;

            switch (key)
            {
                //case (char) 8: //Backspace
                //    if (Text.Length > 0)
                //        Text = Text.Substring(0, Text.Length - 1);
                //    break;
                default:
                    if (!char.IsLetter(key))
                        e.Handled = false;
                    else
                        Text += key;
                    break;
            }

            if (!e.Handled)
                base.OnKeyPress(e);

            Lines = GetLines(CreateGraphics());
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //if (DesignMode)
            //    return;
            var newFrontend = GetNewFrontend();
            e.Graphics.DrawImage(newFrontend, e.ClipRectangle,
                new Rectangle(new Point(0, 0),
                    new Size(e.ClipRectangle.Width * UpScaleFactor, e.ClipRectangle.Height * UpScaleFactor)),
                GraphicsUnit.Pixel);
        }

        protected override void OnResize(EventArgs e)
        {
            if (Text != null)
                Lines = GetLines(CreateGraphics());
            Refresh();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            startingLine -= e.Delta / FontHeight;
            startingLine = startingLine < 0 ? 0 : startingLine > Lines.Count - 1 ? Lines.Count - 1 : startingLine;
            Refresh();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            CreateCaret(Handle, IntPtr.Zero, 1, FontHeight * UpScaleFactor - 2);
            SetCursorPosition(0, 0);
            ShowCaret(Handle);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            DestroyCaret();
            base.OnLostFocus(e);
        }

        #endregion

        #region HelperFunctions

        private void SetCursorPosition(int x, int y)
        {
            _cursorX = x;
            _cursorY = y;
            RefreshCaretPosition();
        }

        private void RefreshCaretPosition() => SetCaretPosition(_cursorX, _cursorY - startingLine);

        private void SetCaretPosition(int x, int y) => SetCaretPos(1 + x * GetCharacterWidth(), 2 + y * FontHeight);

        private Bitmap GetNewFrontend()
        {
            var bitmap = new Bitmap(Size.Width * UpScaleFactor, Size.Height * UpScaleFactor);
            var drawFont = new Font(Font.FontFamily, Font.Size * UpScaleFactor);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                var currentPoint = new Point(Location.X, Location.Y);

                graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(new Point(0, 0), bitmap.Size));
                Lines = Lines ?? GetLines(graphics);
                var lines = new List<string>(Lines);
                lines = startingLine < lines.Count ? lines.Skip(startingLine).ToList() : lines;
                foreach (var line in lines)
                {
                    if (currentPoint.Y - Location.Y > Size.Height * UpScaleFactor)
                        break;
                    graphics.DrawString(line, drawFont, new SolidBrush(ForeColor), currentPoint);
                    currentPoint.Y += drawFont.Height;
                }
            }

            return bitmap;
        }

        private List<string> GetLines(Graphics graphics)
        {
            var sizeX = Size.Width;
            var fin = new List<string>();
            if (Text == null)
                return fin;
            var text = Text;
            while (true)
            {
                var current = new StringBuilder();
                for (var wordsAdded = 0;; wordsAdded++)
                {
                    if (text.Length == 0)
                    {
                        fin.Add(current.ToString());
                        return fin;
                    }
                    if (text[0] == '\n')
                    {
                        text = text.Substring(1);
                        fin.Add(current.ToString());
                        break;
                    }
                    var currentWord = text.Split(' ').First();
                    var containsLineBreak = currentWord.Contains('\n');
                    if (containsLineBreak)
                        currentWord = currentWord.Split('\n').Select(s => s.Trim('\r')).First();
                    if (graphics.MeasureString(current + currentWord, Font).Width >= sizeX)
                    {
                        if (wordsAdded != 0)
                            fin.Add(current.ToString());
                        else if (graphics.MeasureString(current + text[0].ToString(), Font).Width >= sizeX)
                            return fin;
                        else
                        {
                            current.Append(text[0]);
                            text = text.Substring(1);
                            continue;
                        }
                        break;
                    }
                    current.Append(currentWord + " ");
                    text = currentWord.Length < text.Length ? text.Substring(currentWord.Length + 1) : "";
                    if (containsLineBreak)
                        break;
                }
            }
        }

        private Point? GetCursorCoordinates(int cursorPosition)
        {
            var leftCursorPosition = cursorPosition;
            for (var i = 0; i < Lines.Count; i++)
            {
                leftCursorPosition -= Lines[i].Length + 1;
                if (leftCursorPosition <= 0)
                    return new Point(leftCursorPosition + Lines[i].Length + 1, i);
            }
            return null;
        }

        #endregion

        #region ImportedMethods

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CreateCaret(IntPtr hWnd, IntPtr hBmp, int w, int h);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetCaretPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowCaret(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyCaret();

        #endregion
    }
}