using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetaTextBox
{
    public class MetaTextBox : Control
    {
        /// <summary>
        ///     Always ends with \n
        /// </summary>
        public override string Text
        {
            get => _text;
            set
            {
                _text = value.Replace("\r", "");
                _text = _text.LastOrDefault() == '\n' ? _text : _text + '\n';
            }
        }

        protected override bool DoubleBuffered { get; set; } = true;
        public override Cursor Cursor { get; set; } = Cursors.IBeam;

        private int _startingLine;

        private int? _characterWidth;
        private int _cursorX;
        private int _cursorY;

        private Bitmap _backgroundRenderedFrontEnd;
        private string _text;

        public int TabSize { get; set; } = 4;

        private bool _refreshingLines;
        private List<string> _lines;

        public int CursorIndex
        {
            set
            {
                var coordinates = GetCursorCoordinates(value);
                if (!coordinates.HasValue)
                    return;
                //Debug.WriteLine($"X: {coordinates.Value.X}; Y: {coordinates.Value.Y}");
                _cursorX = coordinates.Value.X;
                _cursorY = coordinates.Value.Y;
                RefreshCaretPosition();
            }
            get => GetCursorIndex(_cursorX, _cursorY);
        }

        private int GetCharacterWidth()
        {
            if (_characterWidth == null)
                _characterWidth = GetStringWidth("_");
            return _characterWidth.Value;
        }

        public List<string> Lines
        {
            get
            {
                while (_refreshingLines)
                {
                }
                return _lines;
            }
            private set => _lines = value;
        }

        #region MainFunctions

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (PerformInput(e.KeyCode, e))
                e.Handled = true;
            else
                base.OnKeyDown(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                case Keys.Tab:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            if (_backgroundRenderedFrontEnd != null)
                e.Graphics.DrawImage(_backgroundRenderedFrontEnd, e.ClipRectangle,
                    new Rectangle(new Point(0, 0),
                        new Size(e.ClipRectangle.Width,
                            e.ClipRectangle.Height)),
                    GraphicsUnit.Pixel);
        }

        protected override void OnResize(EventArgs e)
        {
            RefreshLines();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            _startingLine -= e.Delta / FontHeight;
            _startingLine = _startingLine < 0 ? 0 : _startingLine > Lines.Count - 1 ? Lines.Count - 1 : _startingLine;
            RefreshCaretPosition();
            AsyncRefresh();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            CreateCaret(Handle, IntPtr.Zero, 1, FontHeight - 2);
            SetCursorPosition(_cursorX, _cursorY);
            ShowCaret(Handle);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            DestroyCaret();
            base.OnLostFocus(e);
        }

        /// <summary>
        ///     DON'T CALL THIS! Call AsyncRefresh instead.
        /// </summary>
        public override void Refresh()
        {
            if (_backgroundRenderedFrontEnd == null)
                throw new Exception("Called Refresh before creating the frontEnd");
            base.Refresh();
        }

        public void AsyncRefresh()
        {
            Task.Factory.StartNew(async () =>
            {
                Thread.CurrentThread.Name = "RenderingThread";
                while (!IsHandleCreated)
                {
                }
                _backgroundRenderedFrontEnd = await GetNewFrontend();
                Invoke(new Action(Refresh));
            }, TaskCreationOptions.None);
        }

        public async void SyncRefresh()
        {
            while (!IsHandleCreated)
            {
            }
            _backgroundRenderedFrontEnd = await GetNewFrontend();
            Invoke(new Action(Refresh));
        }

        #endregion

        #region HelperFunctions

        private void RefreshLines()
        {
            _refreshingLines = true;
            Task.Factory.StartNew(async () =>
            {
                await Task.Run(() =>
                {
                    if (Text != null)
                        Lines = GetLines();
                    _refreshingLines = false;
                });
                SyncRefresh();
            });
        }

        private bool PerformInput(Keys key, KeyEventArgs keyEventArgs)
        {

            switch (key)
            {
                case Keys.Back:
                    if (CursorIndex > 0)
                    {
                        CursorIndex--;
                        RemoveCharacter(CursorIndex);
                    }
                    return true;
                case Keys.Delete:
                    if (CursorIndex < Text.Length - 1)
                        RemoveCharacter(CursorIndex);
                    return true;
                case Keys.Down:
                    if (_cursorY < Lines.Count - 1)
                        CursorIndex += -_cursorX + Lines[_cursorY].Length + (_cursorX < Lines[_cursorY + 1].Length
                                           ? _cursorX
                                           : Lines[_cursorY + 1].Length - 1
                                       ); //To the beginning -> to the next line -> restore x position
                    return true;
                case Keys.Up:
                    if (_cursorY > 0)
                        CursorIndex += -_cursorX - Lines[_cursorY - 1].Length + (_cursorX < Lines[_cursorY - 1].Length
                                           ? _cursorX
                                           : Lines[_cursorY - 1].Length - 1
                                       ); //To the beginning -> to the previous line -> restore x position
                    return true;
                case Keys.Left:
                    if (CursorIndex > 0)
                        CursorIndex--;
                    return true;
                case Keys.Right:
                    if (CursorIndex < Text.Length - 1)
                        CursorIndex++;
                    return true;
                case Keys.Return:
                    InsertCharacter(CursorIndex, '\n');
                    CursorIndex++;
                    return true;
                case Keys.Tab:
                    InsertText(CursorIndex, string.Join("", Enumerable.Repeat(' ', TabSize)));
                    CursorIndex += TabSize;
                    return true;
                case Keys.OemPeriod: //Dot
                    InsertCharacter(CursorIndex, keyEventArgs.Shift ? ':' : '.');
                    CursorIndex++;
                    return true;
                case Keys.Oemcomma: //Coma
                    InsertCharacter(CursorIndex, keyEventArgs.Shift ? ';' : ',');
                    CursorIndex++;
                    return true;
                case Keys.OemMinus: //Minus
                    InsertCharacter(CursorIndex, keyEventArgs.Shift ? '_' : '-');
                    CursorIndex++;
                    return true;
                case Keys.D0:
                    if (keyEventArgs.Shift) //TODO: Don't do anything if invalid keypress
                        InsertCharacter(CursorIndex, '=');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, '}');
                    else
                        InsertCharacter(CursorIndex, '0');
                    CursorIndex++;
                    return true;
                case Keys.D1:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '!');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, '1');
                    CursorIndex++;
                    return true;
                case Keys.D2:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '"');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, '²');
                    else
                        InsertCharacter(CursorIndex, '2');
                    CursorIndex++;
                    return true;
                case Keys.D3:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '§');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, '³');
                    else
                        InsertCharacter(CursorIndex, '3');
                    CursorIndex++;
                    return true;
                case Keys.D4:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '$');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, '4');
                    CursorIndex++;
                    return true;
                case Keys.D5:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '%');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, '5');
                    CursorIndex++;
                    return true;
                case Keys.D6:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '&');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, '6');
                    CursorIndex++;
                    return true;
                case Keys.D7:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '/');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, '{');
                    else
                        InsertCharacter(CursorIndex, '7');
                    CursorIndex++;
                    return true;
                case Keys.D8:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '(');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, '[');
                    else
                        InsertCharacter(CursorIndex, '8');
                    CursorIndex++;
                    return true;
                case Keys.D9:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, ')');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, ']');
                    else
                        InsertCharacter(CursorIndex, '9');
                    CursorIndex++;
                    return true;
                case Keys.Oemplus:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '*');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, '~');
                    else
                        InsertCharacter(CursorIndex, '+');
                    CursorIndex++;
                    return true;
                case Keys.Oem1: //Ü
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, 'Ü');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, 'ü');
                    CursorIndex++;
                    return true;
                case Keys.Oem2: //Hashtag
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '\'');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, '#');
                    CursorIndex++;
                    return true;
                case Keys.Oem3: //Ö
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, 'Ö');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, 'ö');
                    CursorIndex++;
                    return true;
                case Keys.Oem4: //Sharp s
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '?');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, '\\');
                    else
                        InsertCharacter(CursorIndex, 'ß');
                    CursorIndex++;
                    return true;
                case Keys.Oem5: //Roof
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '°');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, '^');
                    CursorIndex++;
                    return true;
                case Keys.Oem6: //Apostrophe
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '`');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, '´');
                    CursorIndex++;
                    return true;
                case Keys.Oem7:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, 'Ä');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, 'ä');
                    CursorIndex++;
                    return true;
                case Keys.Oem102: //Smaller
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, '>');
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        InsertCharacter(CursorIndex, '|');
                    else
                        InsertCharacter(CursorIndex, '<');
                    CursorIndex++;
                    return true;
                case Keys.End:
                    SetCursorPosition(
                        Lines.Count > 0 ? Lines[_cursorY].Length > 0 ? Lines[_cursorY].Length - 1 : 0 : _cursorX,
                        _cursorY);
                    return true;
                case Keys.Home:
                    SetCursorPosition(0, _cursorY);
                    return true;
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    InsertCharacter(CursorIndex, (key - Keys.NumPad0).ToString()[0]);
                    CursorIndex++;
                    return true;
                case Keys.Divide:
                    InsertCharacter(CursorIndex, '/');
                    CursorIndex++;
                    return true;
                case Keys.Multiply:
                    InsertCharacter(CursorIndex, '*');
                    CursorIndex++;
                    return true;
                case Keys.Subtract:
                    InsertCharacter(CursorIndex, '-');
                    CursorIndex++;
                    return true;
                case Keys.Add:
                    InsertCharacter(CursorIndex, '+');
                    CursorIndex++;
                    return true;
                case Keys.Decimal:
                    InsertCharacter(CursorIndex, ',');
                    CursorIndex++;
                    return true;
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                case Keys.Space:
                    if (keyEventArgs.Shift)
                        InsertCharacter(CursorIndex, char.ToUpper((char)key));
                    else if (keyEventArgs.Control && keyEventArgs.Alt)
                        break;
                    else
                        InsertCharacter(CursorIndex, char.ToLower((char) key));
                    CursorIndex++;
                    return true;
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                    break;
                case Keys.NumLock:
                case Keys.Scroll:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.BrowserBack:
                case Keys.BrowserForward:
                case Keys.BrowserRefresh:
                case Keys.BrowserStop:
                case Keys.BrowserSearch:
                case Keys.BrowserFavorites:
                case Keys.BrowserHome:
                case Keys.VolumeMute:
                case Keys.VolumeDown:
                case Keys.VolumeUp:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                case Keys.MediaPlayPause:
                case Keys.LaunchMail:
                case Keys.SelectMedia:
                case Keys.LaunchApplication1:
                case Keys.LaunchApplication2:
                case Keys.KeyCode:
                case Keys.Modifiers:
                case Keys.None:
                case Keys.LButton:
                case Keys.RButton:
                case Keys.Cancel:
                case Keys.MButton:
                case Keys.XButton1:
                case Keys.XButton2:
                case Keys.LineFeed:
                case Keys.Clear:
                case Keys.Menu:
                case Keys.Pause:
                case Keys.Capital:
                case Keys.KanaMode:
                case Keys.JunjaMode:
                case Keys.FinalMode:
                case Keys.HanjaMode:
                case Keys.Escape:
                case Keys.IMEConvert:
                case Keys.IMENonconvert:
                case Keys.IMEAccept:
                case Keys.IMEModeChange:
                case Keys.Prior:
                case Keys.Next:
                case Keys.Select:
                case Keys.Print:
                case Keys.Execute:
                case Keys.Snapshot:
                case Keys.Insert:
                case Keys.Help:
                case Keys.LWin:
                case Keys.RWin:
                case Keys.Apps:
                case Keys.Sleep:
                case Keys.Separator:
                case Keys.Oem8:
                case Keys.ProcessKey:
                case Keys.Packet:
                case Keys.Attn:
                case Keys.Crsel:
                case Keys.Exsel:
                case Keys.EraseEof:
                case Keys.Play:
                case Keys.Zoom:
                case Keys.NoName:
                case Keys.Pa1:
                case Keys.OemClear:
                    break;
                case Keys.ShiftKey:
                case Keys.ControlKey:
                case Keys.Shift:
                case Keys.Control:
                case Keys.Alt:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                    break;
            }
            return false;
        }

        private void RemoveCharacter(int index)
        {
            Text = Text.Remove(index, 1);
            RefreshLines();
        }

        private void InsertCharacter(int index, char character) => InsertText(index, character.ToString());

        private void InsertText(int index, string text)
        {
            Text = Text.Insert(index, text);
            RefreshLines();
        }

        private void SetCursorPosition(int x, int y)
        {
            _cursorX = x;
            _cursorY = y;
            RefreshCaretPosition();
        }

        private void RefreshCaretPosition() => SetCaretPosition(_cursorX, _cursorY - _startingLine);

        private void SetCaretPosition(int x, int y) => SetCaretPos(
            3 + GetCharacterWidth() * x,
            1 + y * Font.Height);

        private async Task<Bitmap> GetNewFrontend() => await Task.Run(() =>
        {
            var bitmap = new Bitmap(Size.Width, Size.Height);
            var currentPoint = new Point(Location.X, Location.Y);

            Lines = Lines ?? GetLines();
            var lines = new List<string>(Lines);
            lines = _startingLine < lines.Count ? lines.Skip(_startingLine).ToList() : lines;
            var drawableLines = new List<DrawableLine>();
            foreach (var line in lines)
            {
                if (currentPoint.Y - Location.Y > Size.Height)
                    break;
                drawableLines.Add(new DrawableLine {Line = line, Location = currentPoint});
                currentPoint.Y += Font.Height;
            }
            DrawLinesToImage(bitmap, drawableLines, Font, ForeColor, BackColor);

            return bitmap;
        });

        private List<string> GetLines()
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

                    var currentWord = text.Split(' ').First();
                    var hadLineBreak = currentWord.Contains('\n');
                    if (hadLineBreak)
                        currentWord = currentWord.Split('\n').First();
                    if ((current + currentWord).Length * GetCharacterWidth() >= sizeX)
                    {
                        if (wordsAdded != 0)
                            fin.Add(current.ToString());
                        else if ((current + text[0].ToString()).Length * GetCharacterWidth() >= sizeX)
                            return fin;
                        else
                        {
                            current.Append(text[0]);
                            text = text.Substring(1);
                            continue;
                        }
                        break;
                    }
                    current.Append(currentWord + (hadLineBreak ? "\n" : " "));
                    text = currentWord.Length < text.Length ? text.Substring(currentWord.Length + 1) : "";
                    if (hadLineBreak)
                    {
                        fin.Add(current.ToString());
                        break;
                    }
                }
            }
        }

        private Point? GetCursorCoordinates(int cursorIndex)
        {
            var leftCursorPosition = cursorIndex;
            for (var i = 0; i < Lines.Count; i++)
            {
                var newCursorPoint = leftCursorPosition - Lines[i].Length;
                if (newCursorPoint < 0)
                    return new Point(leftCursorPosition, i);
                leftCursorPosition = newCursorPoint;
            }
            return null;
        }

        private int GetCursorIndex(int cursorX, int cursorY)
        {
            var fin = cursorX;
            for (var l = cursorY - 1; l >= 0; l--)
                fin += Lines[l].Length;
            return fin;
        }

        private int GetStringWidth(string s, Graphics g = null)
        {
            var graphics = g ?? CreateGraphics();
            return TextRenderer.MeasureText(graphics, s, Font).Width -
                   (s.Length > 0 ? TextRenderer.MeasureText(graphics, "_", Font).Width / 2 : 0);
        }

        private static void DrawLinesToImage(Image image, List<DrawableLine> lines, Font font,
            Color foreColor, Color backColor = default(Color))
        {
            var memoryHdc = CreateMemoryHdc(IntPtr.Zero, image.Width, image.Height, out IntPtr dib);
            try
            {
                using (var memoryGraphics = Graphics.FromHdc(memoryHdc))
                {
                    memoryGraphics.Clear(backColor);
                    foreach (var drawableLine in lines)
                        TextRenderer.DrawText(memoryGraphics, drawableLine.Line.Replace("\n", ""), font,
                            drawableLine.Location, foreColor,
                            backColor);
                }

                using (var imageGraphics = Graphics.FromImage(image))
                {
                    var imgHdc = imageGraphics.GetHdc();
                    BitBlt(imgHdc, 0, 0, image.Width, image.Height, memoryHdc, 0, 0, 0x00CC0020);
                    imageGraphics.ReleaseHdc(imgHdc);
                }
            }
            finally
            {
                DeleteObject(dib);
                DeleteDC(memoryHdc);
            }
        }

        #endregion

        #region ImportedMethods

        private static IntPtr CreateMemoryHdc(IntPtr hdc, int width, int height, out IntPtr dib)
        {
            var memoryHdc = CreateCompatibleDC(hdc);
            SetBkMode(memoryHdc, 1);

            var info = new BitMapInfo();
            info.biSize = Marshal.SizeOf(info);
            info.biWidth = width;
            info.biHeight = -height;
            info.biPlanes = 1;
            info.biBitCount = 32;
            info.biCompression = 0; // BI_RGB      
            dib = CreateDIBSection(hdc, ref info, 0, out IntPtr _, IntPtr.Zero, 0);
            SelectObject(memoryHdc, dib);

            return memoryHdc;
        }

        [DllImport("gdi32.dll")]
        private static extern int SetBkMode(IntPtr hdc, int mode);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BitMapInfo pbmi, uint iUsage,
            out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        private static extern int SelectObject(IntPtr hdc, IntPtr hgdiObj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc,
            int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hdc);

        [StructLayout(LayoutKind.Sequential)]
        private struct BitMapInfo
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            private readonly int biSizeImage;
            private readonly int biXPelsPerMeter;
            private readonly int biYPelsPerMeter;
            private readonly int biClrUsed;
            private readonly int biClrImportant;
            private readonly byte bmiColors_rgbBlue;
            private readonly byte bmiColors_rgbGreen;
            private readonly byte bmiColors_rgbRed;
            private readonly byte bmiColors_rgbReserved;
        }

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