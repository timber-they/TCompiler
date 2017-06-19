using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MetaTextBox
{
    public class MetaTextBox : Control
    {
        public MetaTextBox ()
        {
            InitializeComponent ();
            if (GetStringWidth ("i") != GetStringWidth ("m"))
                throw new Exception ("Only monospace fonts are valid!");
            Text = new ColoredString (new List<ColoredCharacter> ());
        }

        /// <inheritdoc />
        public override Font Font { get; set; } =
            new Font ("Consolas", 9.75F, FontStyle.Regular,
                      GraphicsUnit.Point, 0);

        /// <summary>
        ///     Always ends with \n
        /// </summary>
        public new ColoredString Text
        {
            get => _text;
            set
            {
                _text = value.Remove ('\r');
                _text = _text.LastOrDefault ()?.Character == '\n'
                            ? _text
                            : _text + new ColoredCharacter (ForeColor, BackColor, '\n');
                RefreshLines ();
                _verticalScrollBar.Maximum = Lines.Count - 2 + _verticalScrollBar.LargeChange - 1;
            }
        }

        private bool AutomaticLineFolding { get; } = false;

        protected override bool DoubleBuffered { get; set; } = true;
        public override Cursor Cursor { get; set; } = Cursors.IBeam;
        private Color SelectionColor { get; } = Color.DodgerBlue;

        private int _startingLine;

        private int? _characterWidth;
        private int _cursorX;
        private int _cursorY;

        private Point MousePositionOnMouseDown;

        public int SelectionLength
        {
            get => _selectionLength;
            set
            {
                _selectionLength = value;
                Debug.WriteLine ($"Index: {CursorIndex}; Length: {_selectionLength}");
                ColorSelectionInText ();
            }
        }

        private Bitmap _backgroundRenderedFrontEnd;
        private ColoredString _text;

        private int TabSize { get; } = 4;

        private bool _refreshingLines;
        private readonly List<int> _renderers = new List<int> ();
        private int _rendererCount;
        private bool _rendering;
        private VScrollBar _verticalScrollBar;
        private List<ColoredString> _lines;
        private int _selectionLength;

        public int CursorIndex
        {
            set
            {
                var coordinates = GetCursorCoordinates (value);
                if (!coordinates.HasValue)
                    return;
                _cursorX = coordinates.Value.X;
                _cursorY = coordinates.Value.Y;
                RefreshCaretPosition ();
            }
            get => GetCursorIndex (_cursorX, _cursorY);
        }

        public int GetCharacterWidth ()
        {
            if (_characterWidth == null)
                _characterWidth = GetStringWidth ("_");
            return _characterWidth.Value;
        }

        private List<ColoredString> Lines
        {
            get
            {
                while (_refreshingLines) {}
                return _lines;
            }
            set => _lines = value;
        }


        #region MetaFunctions

        public void ColorCharacter (int index, Color color) => Text.SetForeColor (index, color);

        public void ColorRange (int startingIndex, int count, Color color) =>
            Text.SetForeColorRange (startingIndex, count, color);

        #endregion


        #region MainFunctions

        #region Handler

        protected override void OnKeyDown (KeyEventArgs e)
        {
            if (PerformInput (e.KeyCode, e) || PerformShortcut (e.KeyCode, e))
                e.Handled = true;
            else
                base.OnKeyDown (e);
        }

        protected override void OnPaint (PaintEventArgs e)
        {
            base.OnPaint (e);
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            if (_backgroundRenderedFrontEnd != null)
                e.Graphics.DrawImage (_backgroundRenderedFrontEnd, e.ClipRectangle,
                                      new Rectangle (new Point (0, 0),
                                                     new Size (e.ClipRectangle.Width,
                                                               e.ClipRectangle.Height)),
                                      GraphicsUnit.Pixel);
        }

        protected override void OnResize (EventArgs e)
        {
            if (AutomaticLineFolding)
                RefreshLines ();
            else
                AsyncRefresh ();
            _verticalScrollBar.Location = new Point (Width - _verticalScrollBar.Width, 0);
            _verticalScrollBar.Size = new Size (_verticalScrollBar.Width, Height);
        }

        protected override void OnMouseWheel (MouseEventArgs e)
        {
            base.OnMouseWheel (e);
            if (Lines.Count > 2)
            {
                ScrollTo (_startingLine - e.Delta / Font.Height);
                _verticalScrollBar.Value = _startingLine *
                                           (_verticalScrollBar.Maximum -
                                            (_verticalScrollBar.LargeChange - 1) -
                                            _verticalScrollBar.Minimum) /
                                           (Lines.Count - 2);
            }
        }

        /// <inheritdoc />
        protected override void OnMouseDown (MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MousePositionOnMouseDown = e.Location;
            base.OnMouseDown (e);
        }

        /// <inheritdoc />
        protected override void OnMouseUp (MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SetSelectionFromPosition (MousePositionOnMouseDown, e.Location);
            base.OnMouseUp (e);
        }

        /// <inheritdoc />
        protected override void OnMouseMove (MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SetSelectionFromPosition (MousePositionOnMouseDown, e.Location);
            base.OnMouseMove (e);
        }

        private void VerticalScrollBarOnScroll (object sender, ScrollEventArgs scrollEventArgs) => ScrollTo (
            GetStartingLine (scrollEventArgs.NewValue));

        protected override void OnGotFocus (EventArgs e)
        {
            CreateCaret (Handle, IntPtr.Zero, 1, Font.Height - 2);
            SetCursorPosition (_cursorX, _cursorY);
            ShowCaret (Handle);
            base.OnGotFocus (e);
        }

        protected override void OnLostFocus (EventArgs e)
        {
            DestroyCaret ();
            base.OnLostFocus (e);
        }

        #endregion


        private void ScrollTo (int newStartingLine)
        {
            _startingLine = newStartingLine < 0
                                ? 0
                                : newStartingLine > Lines.Count - 2
                                    ? Lines.Count - 2
                                    : newStartingLine;
            RefreshCaretPosition ();
            if (AutomaticLineFolding)
                RefreshLines ();
            else
                AsyncRefresh ();
        }

        /// <summary>
        ///     DON'T CALL THIS! Call AsyncRefresh instead.
        /// </summary>
        public override void Refresh ()
        {
            if (_backgroundRenderedFrontEnd == null)
                throw new Exception ("Called Refresh before creating the frontEnd");
            base.Refresh ();
        }

        private void AsyncRefresh ()
        {
            Task.Factory.StartNew (async () =>
            {
                var index = _rendererCount;
                _renderers.Add (index);
                _rendererCount++;
                while (_rendering)
                    if (_renderers.Last () != index)
                        return;
                _renderers.Remove (index);
                _rendererCount--;
                _rendering = true;
                Thread.CurrentThread.Name = "RenderingThread";
                while (!IsHandleCreated) {}
                _backgroundRenderedFrontEnd = await GetNewFrontend ();
                Invoke (new Action (Refresh));
                _rendering = false;
            }, TaskCreationOptions.None);
        }

        private async Task SyncRefresh ()
        {
            while (!IsHandleCreated) {}
            var index = _rendererCount;
            _renderers.Add (index);
            _rendererCount++;
            while (_rendering)
                if (_renderers.Last () != index)
                    return;
            _renderers.Remove (index);
            _rendererCount--;
            _rendering = true;
            _backgroundRenderedFrontEnd = await GetNewFrontend ();
            Invoke (new Action (Refresh));
            _rendering = false;
        }

        public void SetSelection (int start, int length)
        {
            CursorIndex = start;
            SelectionLength = length;
            AsyncRefresh ();
        }

        public void SetSelection (Point startCursorLocation, Point endCursorLocation)
        {
            var startIndex = GetCursorIndex (startCursorLocation.X, startCursorLocation.Y);
            var endIndex = GetCursorIndex (endCursorLocation.X, endCursorLocation.Y);
            SetSelection (endIndex, startIndex - endIndex);
        }

        public void SetSelectionFromPosition (Point startPosition, Point endPosition) =>
            SetSelection (GetCursorLocationToPoint (startPosition), GetCursorLocationToPoint (endPosition));

        #endregion


        #region HelperFunctions

        protected override bool IsInputKey (Keys keyData)
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
            return base.IsInputKey (keyData);
        }

        public Point GetCursorLocationToPoint (Point point)
        {
            var x = point.X / GetCharacterWidth ();
            var y = point.Y / Font.Height + _startingLine;
            y = Lines.Count - 1 > y ? y : Lines.Count - 2;
            x = _lines [y].Count () > x ? x : _lines [y].Count () - 1;
            return new Point (x, y);
        }

        public void ColorSelectionInText ()
        {
            var cursorIndex = CursorIndex;
            var selectionLength = SelectionLength;
            var selectionColor = SelectionColor;
            for (var index = 0; index < Text.ColoredCharacters.Count; index++)
                if (cursorIndex <= index && cursorIndex + selectionLength > index ||
                    cursorIndex > index && cursorIndex + selectionLength <= index)
                    Text.ColoredCharacters [index].BackColor =
                        selectionColor;
                else
                    Text.ColoredCharacters [index].BackColor =
                        BackColor; //TODO extra backColor or something, selection splitted from other colors
        }

        private int GetStartingLine (int scrollBarValue) => (int) ((double) scrollBarValue /
                                                                   (_verticalScrollBar.Maximum -
                                                                    (_verticalScrollBar.LargeChange - 1) -
                                                                    _verticalScrollBar.Minimum) *
                                                                   Lines.Count);

        private void RefreshLines ()
        {
            _refreshingLines = true;
            Task.Factory.StartNew (async () =>
            {
                await Task.Run (() =>
                {
                    if (Text != null)
                        Lines = GetLines (_startingLine, Height / Font.Height);
                    _refreshingLines = false;
                });
                await SyncRefresh ();
            });
        }

        private bool PerformShortcut (Keys key, KeyEventArgs keyEventArgs) //TODO tests
        {
            if (!ValidateShortcut (keyEventArgs.Modifiers))
                return false;
            /**
            *
            * Normally valid keyboard shortcuts:
            * • CTRL + the key
            *
            **/

            switch (key)
            {
                case Keys.A:
                    SetSelection (Text.Count () - 1, -(Text.Count () - 1));
                    return true;
            }
            return false;
        }

        public bool PerformInput (Keys key, KeyEventArgs keyEventArgs)
        {
            if (!ValidateInput (keyEventArgs.Modifiers))
                return false;
            /**
             * 
             * Normally valid keyboard inputs:
             * • The key
             * • SHIFT + the key
             * • ALT + CTRL + the key
             * 
             **/

            var keyInput = KeyInput.AllKeyInputs.
                                    Where (input => input.Key == key).
                                    Select (input => input.GetCharacter (keyEventArgs.Shift, keyEventArgs.Control,
                                                                         keyEventArgs.Alt)).
                                    FirstOrDefault (c => c != null);
            if (keyInput != null)
            {
                var oldIndex = CursorIndex;
                if (keyInput.Value == '\t')
                {
                    InsertString (CursorIndex, new ColoredString (ForeColor, BackColor, new string (' ', TabSize)));
                    CursorIndex = oldIndex + 4;
                }
                else
                {
                    InsertCharacter (CursorIndex, new ColoredCharacter (ForeColor, BackColor, keyInput.Value));
                    CursorIndex = oldIndex + 1;
                }
                return true;
            }

            switch (key)
            {
                case Keys.Back: //TODO selection deletion tests
                    if (_selectionLength == 0 && CursorIndex > 0)
                    {
                        var oldIndex = CursorIndex;
                        DeleteCharacter (CursorIndex - 1);
                        CursorIndex = oldIndex - 1;
                    }
                    else if (_selectionLength != 0)
                        DeleteSelection ();
                    else
                        return false;
                    return true;
                case Keys.Delete:
                    if (CursorIndex < Text.Count () - 1 && _selectionLength == 0)
                        DeleteCharacter (CursorIndex);
                    else if (_selectionLength != 0)
                        DeleteSelection ();
                    else
                        return false;
                    return true;
                case Keys.Down:
                    if (_cursorY < Lines.Count - 1)
                    {
                        var startingCursorPosition = CursorIndex;
                        CursorIndex += -_cursorX +
                                       Lines [_cursorY].Count () +
                                       (_cursorX < Lines [_cursorY + 1].Count ()
                                            ? _cursorX
                                            : Lines [_cursorY + 1].Count () - 1
                                       ); //To the beginning -> to the next line -> restore x position
                        if (keyEventArgs.Shift)
                            SetSelection (CursorIndex, SelectionLength + (startingCursorPosition - CursorIndex));
                    }
                    if (!keyEventArgs.Shift)
                        SelectionLength = 0;
                    AsyncRefresh ();
                    return true;
                case Keys.Up:
                    if (_cursorY > 0)
                    {
                        var startingCursorPosition = CursorIndex;
                        CursorIndex += -_cursorX -
                                       Lines [_cursorY - 1].Count () +
                                       (_cursorX < Lines [_cursorY - 1].Count ()
                                            ? _cursorX
                                            : Lines [_cursorY - 1].Count () - 1
                                       ); //To the beginning -> to the previous line -> restore x position
                        if (keyEventArgs.Shift)
                            SetSelection (CursorIndex, SelectionLength + (startingCursorPosition - CursorIndex));
                    }
                    if (!keyEventArgs.Shift)
                        SelectionLength = 0;
                    AsyncRefresh ();
                    return true;
                case Keys.Left:
                    if (CursorIndex > 0)
                    {
                        CursorIndex--;
                        if (keyEventArgs.Shift)
                            SelectionLength++;
                    }
                    if (!keyEventArgs.Shift)
                        SelectionLength = 0;
                    AsyncRefresh ();
                    return true;
                case Keys.Right:
                    if (CursorIndex < Text.Count () - 1)
                    {
                        CursorIndex++;
                        if (keyEventArgs.Shift)
                            SelectionLength--;
                    }
                    if (!keyEventArgs.Shift)
                        SelectionLength = 0;
                    AsyncRefresh ();
                    return true;
                case Keys.End:
                {
                    var oldCursorIndex = CursorIndex;
                    SetCursorPosition (
                        Lines.Count > 0 ? Lines [_cursorY].Count () > 0 ? Lines [_cursorY].Count () - 1 : 0 : _cursorX,
                        _cursorY);
                    if (!keyEventArgs.Shift)
                        SelectionLength = 0;
                    else
                        SelectionLength += oldCursorIndex - CursorIndex;
                    AsyncRefresh ();
                    return true;
                }
                case Keys.Home:
                {
                    var oldCursorIndex = CursorIndex;
                    SetCursorPosition (0, _cursorY);
                    if (!keyEventArgs.Shift)
                        SelectionLength = 0;
                    else
                        SelectionLength += oldCursorIndex - CursorIndex;
                    AsyncRefresh ();
                    return true;
                }
                default:
                    return false;
            }
        }

        private static bool ValidateInput (Keys modifiers) => modifiers == Keys.None ||
                                                              modifiers == Keys.Shift ||
                                                              modifiers == (Keys.Control | Keys.Alt);

        private static bool ValidateShortcut (Keys modifiers) => modifiers == Keys.Control;

        private void DeleteCharacter (int index) => Text = Text.Remove (index, 1);

        private void InsertCharacter (int index, ColoredCharacter coloredCharacter) =>
            Text = Text.Insert (index, coloredCharacter);

        private void InsertString (int index, ColoredString coloredString) =>
            Text = Text.Insert (index, coloredString);

        private void InsertText (int index, ColoredString text) => Text = Text.Insert (index, text);

        private void SetCursorPosition (int x, int y)
        {
            _cursorX = x;
            _cursorY = y;
            RefreshCaretPosition ();
        }

        private void SetCursorPosition (Point position) => SetCursorPosition (position.X, position.Y);

        private void RefreshCaretPosition () => SetCaretPosition (_cursorX, _cursorY - _startingLine);

        private void SetCaretPosition (int x, int y)
        {
            SetCaretPos (
                3 + GetCharacterWidth () * x,
                1 + y * Font.Height);
        }

        private async Task<Bitmap> GetNewFrontend () => await Task.Run (() =>
        {
            var bitmap =
                new Bitmap (Size.Width, Size.Height);
            var currentPoint =
                new Point (Location.X, Location.Y);

            Lines = Lines /*?? GetLines (cursorIndex, beginningLine, lineCount)*/; //TODO valid without commentary?
            var lines = new List<ColoredString> (Lines);

            lines = lines.Skip (_startingLine).
                          ToList ();
            var drawableLines =
                new List<DrawableLine> ();
            foreach (var line in lines)
            {
                if (currentPoint.Y - Location.Y >
                    Size.Height)
                    break;
                drawableLines.Add (new DrawableLine
                {
                    LineRanges = GetLineRanges (line),
                    Location = currentPoint
                });
                currentPoint.Y += Font.Height;
            }
            DrawLinesToImage (
                bitmap, drawableLines, Font, BackColor);

            return bitmap;
        });

        public static List<ColoredString> GetLineRanges (ColoredString line)
        {
            var fin = new List<ColoredString> ();
            var currentForeColor = line.GetFirstOrDefaultForeColor ();
            var currentBackColor = line.GetFirstOrDefaultBackColor ();
            var currentRange = new ColoredString (new List<ColoredCharacter> ());
            foreach (var coloredCharacter in line.ColoredCharacters)
            {
                if (!char.IsWhiteSpace (coloredCharacter.Character) &&
                    (currentForeColor == null ||
                     currentForeColor.Value != coloredCharacter.ForeColor) ||
                    currentBackColor == null ||
                    currentBackColor.Value != coloredCharacter.BackColor)
                {
                    if (currentRange.Count () > 0)
                        fin.Add (currentRange);
                    currentRange = new ColoredString (new List<ColoredCharacter> ());
                    currentBackColor = coloredCharacter.BackColor;
                    currentForeColor = coloredCharacter.ForeColor;
                }
                currentRange += coloredCharacter;
            }
            if (currentRange.Count () > 0)
                fin.Add (currentRange);
            return fin;
        }

        private List<ColoredString> GetLines (int beginningLine, int lineCount)
        {
            if (AutomaticLineFolding)
            {
                var sizeX = Size.Width - _verticalScrollBar.Width - 3;
                var fin = new List<ColoredString> ();
                if (Text == null)
                    return fin;
                var text = new ColoredString (Text);
                var currentLine = 0;
                while (true)
                {
                    var current = new ColoredString (new List<ColoredCharacter> ());
                    for (var wordsAdded = 0;; wordsAdded++)
                    {
                        if (text.Count () == 0)
                        {
                            if (currentLine >= beginningLine)
                                fin.Add (current);
                            return fin;
                        }
                        var splitterColor = text.ColoredCharacters.
                                                 FirstOrDefault (character => character.Character == ' ')?.BackColor;
                        var currentWord = text.Split (' ').First ();
                        var hadLineBreak = currentWord.Contains ('\n');
                        if (hadLineBreak)
                        {
                            splitterColor = currentWord.ColoredCharacters.
                                                        FirstOrDefault (character => character.Character == '\n')?.
                                                        BackColor;
                            currentWord = currentWord.Split ('\n').First ();
                        }
                        if ((current + currentWord).Count () * GetCharacterWidth () >= sizeX)
                        {
                            if (wordsAdded != 0)
                                fin.Add (current);
                            else if ((current + text.Get (0)).Count () * GetCharacterWidth () >= sizeX)
                                return fin;
                            else
                            {
                                current += text.Get (0);
                                text = text.Substring (1);
                                continue;
                            }
                            break;
                        }
                        current += currentWord +
                                   new ColoredCharacter (ForeColor, splitterColor ?? BackColor,
                                                         hadLineBreak ? '\n' : ' ');
                        text = currentWord.Count () < text.Count ()
                                   ? text.Substring (currentWord.Count () + 1)
                                   : new ColoredString (ForeColor, BackColor, "");
                        if (hadLineBreak)
                        {
                            fin.Add (current);
                            break;
                        }
                    }
                    currentLine++;
                    if (currentLine - beginningLine > lineCount)
                        return fin;
                }
            }
            return Text.Split ('\n', true).ToList ();
        }

        private Point? GetCursorCoordinates (int cursorIndex)
        {
            var leftCursorPosition = cursorIndex;
            for (var i = 0; i < Lines.Count; i++)
            {
                var newCursorPoint = leftCursorPosition - Lines [i].Count ();
                if (newCursorPoint < 0)
                    return new Point (leftCursorPosition, i);
                leftCursorPosition = newCursorPoint;
            }
            return null;
        }

        private int GetCursorIndex (int cursorX, int cursorY)
        {
            var fin = cursorX;
            while (_refreshingLines) {}
            for (var l = cursorY - 1; l >= 0; l--)
                fin += Lines [l].Count ();
            return fin;
        }

        public int GetStringWidth (string s, Graphics g = null)
        {
            var graphics = g ?? CreateGraphics ();
            return TextRenderer.MeasureText (graphics, s, Font).Width -
                   (s.Length > 0 ? TextRenderer.MeasureText (graphics, "_", Font).Width / 2 : 0);
        }

        private void DrawLinesToImage (
            Image image, List<DrawableLine> lines, Font font, Color backColor = default (Color))
        {
            var memoryHdc = CreateMemoryHdc (IntPtr.Zero, image.Width, image.Height, out IntPtr dib);
            try
            {
                using (var memoryGraphics = Graphics.FromHdc (memoryHdc))
                {
                    memoryGraphics.Clear (backColor);
                    foreach (var drawableLine in lines)
                    {
                        var currentLocation = new Point (drawableLine.Location.X, drawableLine.Location.Y);
                        foreach (var drawableLineRange in drawableLine.LineRanges)
                        {
                            var foreColor = drawableLineRange.GetFirstOrDefaultForeColor ();
                            var textBackColor = drawableLineRange.GetFirstOrDefaultBackColor ();
                            if (foreColor == null || textBackColor == null)
                                throw new Exception ("Variable shouldn't be null");
                            TextRenderer.DrawText (memoryGraphics, drawableLineRange.Remove ('\n').ToString (),
                                                   font,
                                                   currentLocation,
                                                   foreColor.Value,
                                                   textBackColor.Value);
                            currentLocation.X += drawableLineRange.Count () * GetCharacterWidth ();
                        }
                    }
                }

                using (var imageGraphics = Graphics.FromImage (image))
                {
                    var imgHdc = imageGraphics.GetHdc ();
                    BitBlt (imgHdc, 0, 0, image.Width, image.Height, memoryHdc, 0, 0, 0x00CC0020);
                    imageGraphics.ReleaseHdc (imgHdc);
                }
            }
            finally
            {
                DeleteObject (dib);
                DeleteDC (memoryHdc);
            }
        }

        private void DeleteSelection ()
        {
            var oldCursorIndex = CursorIndex + (_selectionLength > 0 ? 0 : _selectionLength);
            var oldSelectionLength = Math.Sign(_selectionLength) * _selectionLength;
            CursorIndex += _selectionLength > 0 ? 0 : _selectionLength;
            _selectionLength = 0;
            Text = Text.Remove (oldCursorIndex, oldSelectionLength);
        }

        #endregion


        #region ImportedMethods

        private static IntPtr CreateMemoryHdc (IntPtr hdc, int width, int height, out IntPtr dib)
        {
            var memoryHdc = CreateCompatibleDC (hdc);
            SetBkMode (memoryHdc, 1);

            var info = new BitMapInfo ();
            info.biSize = Marshal.SizeOf (info);
            info.biWidth = width;
            info.biHeight = -height;
            info.biPlanes = 1;
            info.biBitCount = 32;
            info.biCompression = 0; // BI_RGB      
            dib = CreateDIBSection (hdc, ref info, 0, out IntPtr _, IntPtr.Zero, 0);
            SelectObject (memoryHdc, dib);

            return memoryHdc;
        }

        [DllImport ("gdi32.dll")]
        private static extern int SetBkMode (IntPtr hdc, int mode);

        [DllImport ("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC (IntPtr hdc);

        [DllImport ("gdi32.dll")]
        private static extern IntPtr CreateDIBSection (
            IntPtr hdc, [In] ref BitMapInfo pbmi, uint iUsage,
            out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport ("gdi32.dll")]
        private static extern int SelectObject (IntPtr hdc, IntPtr hgdiObj);

        [DllImport ("gdi32.dll")]
        [return: MarshalAs (UnmanagedType.Bool)]
        private static extern bool BitBlt (
            IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc,
            int nXSrc, int nYSrc, int dwRop);

        [DllImport ("gdi32.dll")]
        private static extern bool DeleteObject (IntPtr hObject);

        [DllImport ("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool DeleteDC (IntPtr hdc);

        [StructLayout (LayoutKind.Sequential)]
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

        [DllImport ("user32.dll", SetLastError = true)]
        private static extern bool CreateCaret (IntPtr hWnd, IntPtr hBmp, int w, int h);

        [DllImport ("user32.dll", SetLastError = true)]
        private static extern bool SetCaretPos (int x, int y);

        [DllImport ("user32.dll", SetLastError = true)]
        private static extern bool ShowCaret (IntPtr hWnd);

        [DllImport ("user32.dll", SetLastError = true)]
        private static extern bool DestroyCaret ();

        #endregion


        public void InitializeComponent ()
        {
            _verticalScrollBar = new VScrollBar ();
            SuspendLayout ();
            // 
            // _verticalScrollBar
            // 
            _verticalScrollBar.Name = "_verticalScrollBar";
            _verticalScrollBar.TabIndex = 0;
            _verticalScrollBar.Cursor = DefaultCursor;
            _verticalScrollBar.Scroll += VerticalScrollBarOnScroll;
            // 
            // MetaTextBox
            // 
            Controls.Add (_verticalScrollBar);
            ResumeLayout (false);
        }
    }
}