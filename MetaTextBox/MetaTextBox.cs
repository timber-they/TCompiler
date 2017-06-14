using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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
        public override string Text { get; set; }
        protected override bool DoubleBuffered { get; set; } = true;
        public override Cursor Cursor { get; set; } = Cursors.IBeam;

        private int _startingLine;

        private int? _characterWidth;
        private int _cursorX;
        private int _cursorY;

        private Bitmap _backgroundRenderedFrontEnd;

        public int CursorIndex
        {
            set
            {
                var coordinates = GetCursorCoordinates(value);
                if (!coordinates.HasValue)
                    return;
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

        public List<string> Lines { get; private set; }

        #region MainFunctions
        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (PerformInput(e.KeyCode, e))
                e.Handled = true;
            else
                base.OnKeyUp(e);
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
            SetCursorPosition(0, 0);
            ShowCaret(Handle);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            DestroyCaret();
            base.OnLostFocus(e);
        }

        /// <summary>
        ///     DON'T CALL THIS! Call AsyncRefresh instead
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
                while (!IsHandleCreated) ;
                _backgroundRenderedFrontEnd = await GetNewFrontend();
                Invoke(new Action(Refresh));
            }, TaskCreationOptions.None);
        }

        public async void SyncRefresh()
        {
            while (!IsHandleCreated) ;
            _backgroundRenderedFrontEnd = await GetNewFrontend();
            Invoke(new Action(Refresh));
        }

        #endregion

        #region HelperFunctions

        private void RefreshLines()
        {
            Task.Factory.StartNew(async () =>
            {
                await Task.Run(() =>
                {
                    if (Text != null)
                        Lines = GetLines(CreateGraphics());
                });
                SyncRefresh();
            });
        }

        private bool PerformInput(Keys key, KeyEventArgs keyEventArgs)
        {
            switch (key)
            {
                case Keys.Back:
                    return true;
                default:
                    if (key >= Keys.D0 && key <= Keys.Z ||
                        key >= Keys.NumPad0 && key <= Keys.Divide ||
                        key == Keys.Space ||
                        key == Keys.Enter)
                    {
                        var c = keyEventArgs.Shift ? (char) key : char.ToLower((char) key);
                        InsertCharacter(CursorIndex, c);
                        CursorIndex++;
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void InsertCharacter(int index, char character)
        {
            Text = Text.Insert(index, character.ToString());
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
            2 + GetCharacterWidth() * x,
            1 + y * FontHeight);

        private async Task<Bitmap> GetNewFrontend() => await Task.Run(() =>
        {
            var bitmap = new Bitmap(Size.Width, Size.Height);
            var drawFont = new Font(Font.FontFamily, (int) (Font.Size));
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var currentPoint = new Point(Location.X, Location.Y);

                Lines = Lines ?? GetLines(graphics);
                var lines = new List<string>(Lines);
                lines = _startingLine < lines.Count ? lines.Skip(_startingLine).ToList() : lines;
                var drawableLines = new List<DrawableLine>();
                foreach (var line in lines)
                {
                    if (currentPoint.Y - Location.Y > Size.Height)
                        break;
                    drawableLines.Add(new DrawableLine{Line = line, Location = currentPoint});
                    currentPoint.Y += drawFont.Height;
                }
                DrawLinesToImage(bitmap, drawableLines, drawFont, ForeColor, BackColor);
            }

            return bitmap;
        });

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
                    current.Append(currentWord + " ");
                    text = currentWord.Length < text.Length ? text.Substring(currentWord.Length + 1) : "";
                    if (containsLineBreak)
                        break;
                }
            }
        }

        private Point? GetCursorCoordinates(int cursorIndex)
        {
            var leftCursorPosition = cursorIndex;
            for (var i = 0; i < Lines.Count; i++)
            {
                leftCursorPosition -= Lines[i].Length + 1;
                if (leftCursorPosition <= 0)
                    return new Point(leftCursorPosition + Lines[i].Length + 1, i);
            }
            return null;
        }

        private int GetCursorIndex(int cursorX, int cursorY)
        {
            var fin = cursorX;
            for (var l = cursorY - 1; l >= 0; l--)
                fin += Lines[l].Length + 1;
            return fin;
        }

        private int GetStringWidth(string s, Graphics g = null)
        {
            var graphics = g ?? CreateGraphics();
            return TextRenderer.MeasureText(graphics, s, Font).Width - (s.Length > 0 ? TextRenderer.MeasureText(graphics, "_", Font).Width / 2 : 0);
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
                        TextRenderer.DrawText(memoryGraphics, drawableLine.Line, font, drawableLine.Location, foreColor, backColor);
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
            // Create a memory DC so we can work  off-screen      
            IntPtr memoryHdc = CreateCompatibleDC(hdc);
            SetBkMode(memoryHdc, 1);

            // Create a device-independent bitmap  and select it into our DC      
            var info = new BitMapInfo();
            info.biSize = Marshal.SizeOf(info);
            info.biWidth = width;
            info.biHeight = -height;
            info.biPlanes = 1;
            info.biBitCount = 32;
            info.biCompression = 0; // BI_RGB      
            IntPtr ppvBits;
            dib = CreateDIBSection(hdc, ref info, 0, out ppvBits, IntPtr.Zero, 0);
            SelectObject(memoryHdc, dib);

            return memoryHdc;
        }

        [DllImport("gdi32.dll")]
        public static extern int SetBkMode(IntPtr hdc, int mode);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BitMapInfo pbmi, uint iUsage,
            out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        public static extern int SelectObject(IntPtr hdc, IntPtr hgdiObj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc,
            int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BitMapInfo
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
            public byte bmiColors_rgbBlue;
            public byte bmiColors_rgbGreen;
            public byte bmiColors_rgbRed;
            public byte bmiColors_rgbReserved;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CreateCaret(IntPtr hWnd, IntPtr hBmp, int w, int h);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetCaretPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowCaret(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyCaret();

        //[DllImport("Gdi32.dll", SetLastError = true)]
        //private static extern bool TextOut(IDeviceContext hdc, int nxStart, int nYStart, string text, int length);

        #endregion
    }
}