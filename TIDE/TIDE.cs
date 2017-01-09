#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCompiler.Enums;
using TCompiler.Main;
using TCompiler.Settings;
using TIDE.Colouring.Colour;
using TIDE.Colouring.StringFunctions;
using TIDE.Forms;
using TIDE.Properties;

#endregion

namespace TIDE
{
    // ReSharper disable once InconsistentNaming
    public partial class TIDE : Form
    {
        private string _savePath;
        private string _wholeText;

        public TIDE()
        {
            Intellisensing = false;
            Unsaved = false;
            SavePath = null;
            _wholeText = "";
            InitializeComponent();

            IntelliSensePopUp = new IntelliSensePopUp(GetUpdatedItems(), GetIntelliSensePosition()) { Visible = false };
            IntelliSensePopUp.ItemEntered += (sender, s) => IntelliSense_ItemSelected(s);
            Focus();
        }

        private IntelliSensePopUp IntelliSensePopUp { get; }

        private bool Unsaved { get; set; }

        private bool Intellisensing { get; set; }

        private string SavePath
        {
            get { return _savePath; }
            set
            {
                var findForm = FindForm();
                if (findForm != null)
                    findForm.Text = value != null ? $@"TIDE - {value.Split('\\', '/').Last()}" : Resources.TIDE;
                _savePath = value;
            }
        }

        private void Save(bool showDialogue)
        {
            if ((SavePath == null) || showDialogue)
            {
                var dia = new SaveFileDialog
                {
                    AddExtension = true,
                    OverwritePrompt = true,
                    Title = Resources.Save,
                    Filter = Resources.Type_Ending,
                    DefaultExt = "tc"
                };
                if (dia.ShowDialog() != DialogResult.OK)
                    return;
                SavePath = dia.FileName;
            }
            Unsaved = false;
            File.WriteAllText(SavePath, editor.Text);
        }

        private async void Run() => await Task.Run(delegate
        {
            assemblerTextBox.Invoke(new Action(() =>
            {
                Main.Initialize(SavePath, "out.asm", "error.txt");
                var ex = Main.CompileFile();
                if (ex != null)
                {
                    if (ex.Line >= 0)
                        ColourSth.HighlightLine(ex.Line, editor, Color.Red);
                    MessageBox.Show(File.ReadAllText("error.txt"), Resources.Error);
                    if (ex.Line >= 0)
                        ColourSth.HighlightLine(ex.Line, editor, editor.BackColor);
                    return;
                }
                assemblerTextBox.Text = File.ReadAllText("out.asm");
                ColourAll(assemblerTextBox, true);
                tabControl.SelectTab(assemblerPage);
            }));
        });

        private void Open()
        {
            var dia = new OpenFileDialog
            {
                AddExtension = true,
                Title = Resources.Open,
                Filter = Resources.Type_Ending,
                DefaultExt = "tc"
            };
            if (dia.ShowDialog() != DialogResult.OK)
                return;
            SavePath = dia.FileName;
            editor.TextChanged -= editor_TextChanged;
            editor.Text = File.ReadAllText(SavePath);
            ColourAll(editor);
            _wholeText = new string(editor.Text.ToCharArray());
            editor.TextChanged += editor_TextChanged;
        }

        private IEnumerable<string> GetVariableNames()
        {
            var fin = new List<string>(GlobalProperties.StandardVariables.Select(variable => variable.Name));
            VariableType foo;
            fin.AddRange(
                editor.Lines.Where(s => (s.Trim(' ').Split(' ').Length > 1) && Enum.TryParse(s.Trim(' ').Split(' ')[0], true, out foo))
                    .Select(s => s.Trim(' ').Split(' ')[1]));
            return fin;
        }

        private IEnumerable<string> GetMethodNames() => new List<string>(
            editor.Lines.Where(s => s.Trim(' ').Split(' ').Length > 1 && s.Trim(' ').Split(' ').First().Trim(' ') == "method")
                .Select(s => s.Trim(' ').Split(' ')[1].Trim(' ', '[')));

        private async void ColourAll(RichTextBox tbox, bool asm = false)
            => await Task.Run(delegate
            {
                return tbox.Invoke(new Action(() =>
                {
                    BeginUpdate(tbox);
                    foreach (var c in GetCurrent.GetAllChars(tbox))
                        Colouring.Colouring.CharActions(c, tbox);
                    foreach (var word in GetCurrent.GetAllWords(tbox))
                        Colouring.Colouring.WordActions(word, tbox, asm);
                    EndUpdate(tbox);
                }));
            });

        #region IntelliSense

        private Point GetIntelliSensePosition()
        {
            var pos = editor.PointToScreen(editor.GetPositionFromCharIndex(editor.SelectionStart));
            return new Point(pos.X, pos.Y + Cursor.Size.Height);
        }

        private void ShowIntelliSense()
        {
            IntelliSensePopUp.Visible = true;
            IntelliSensePopUp.SelectIndex(0);
            Focus();
        }

        private void HideIntelliSense() => IntelliSensePopUp.Visible = false;

        private void UpdatIntelliSense() => IntelliSensePopUp.UpdateList(GetUpdatedItems());

        private IEnumerable<string> GetUpdatedItems()
        {
            var fin = PublicStuff.StringColorsTCode.Select(color => color.Thestring).Concat(GetVariableNames()).Concat(GetMethodNames())
                .Where(s =>
                {
                    var current =
                        PublicStuff.Splitters.Any(
                            c =>
                                c ==
                                GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor)?.Value)
                            ? ""
                            : GetCurrent.GetCurrentWord(editor.SelectionStart, editor)?.Value;
                    return string.IsNullOrEmpty(current) || s.StartsWith(current, true, CultureInfo.InvariantCulture);
                }).Distinct().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            fin.Sort();
            return fin;
        }

        #endregion

        #region Eventhandling

        #region ButtonHandling

        private void ColourAllButton_Click(object sender, EventArgs e) => ColourAll(editor);

        private void HelpButton_Click(object sender, EventArgs e)
            =>
            MessageBox.Show(
                string.Format(Resources.help_Text,
                    string.Join("\n", PublicStuff.StringColorsTCode.Select(color => color.Thestring))), Resources.Help);

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (Unsaved)
            {
                var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning,
                    MessageBoxButtons.YesNoCancel);
                switch (res)
                {
                    case DialogResult.Yes:
                        SaveButton.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            editor.Text = "";
            SavePath = null;
        }

        private void SaveAsButton_Click(object sender, EventArgs e) => Save(true);

        private void RunButton_Click(object sender, EventArgs e)
        {
            SaveButton.PerformClick();
            if (SavePath == null)
            {
                MessageBox.Show(Resources.You_have_to_save_first, Resources.Error);
                return;
            }
            Run();
        }

        private void SaveButton_Click(object sender, EventArgs e) => Save(false);

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (Unsaved)
            {
                var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning,
                    MessageBoxButtons.YesNoCancel);
                switch (res)
                {
                    case DialogResult.Yes:
                        SaveButton.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            Open();
        }

        #endregion

        private void IntelliSense_ItemSelected(string item)
        {
            HideIntelliSense();
            Intellisensing = true;
            var pos = editor.SelectionStart;
            var lw = GetCurrent.GetCurrentWord(pos, editor)?.Value;
            var s = item.Substring(item.Length >= (lw?.Length ?? 0) ? lw?.Length ?? 0 : 0) + " ";
            Focus();
            SendKeys.Send(s); //Because this is hilarious
        }

        private async void editor_TextChanged(object sender = null, EventArgs e = null)
            => await Task.Run(delegate
            {
                return editor.Invoke(new Action(() =>
                {
                    BeginUpdate(editor);
                    if (StringFunctions.GetRemoved(_wholeText, editor.Text).Contains(';'))
                        Colouring.Colouring.ColourCurrentLine(editor);
                    else
                    {
                        var cChar = GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor);
                        if (!string.IsNullOrEmpty(cChar?.Value.ToString()) && (cChar.Value == ';'))
                            Colouring.Colouring.ColourCurrentLine(editor);
                        else
                        {
                            var word = GetCurrent.GetCurrentWord(editor.SelectionStart, editor);
                            Colouring.Colouring.WordActions(word, editor);
                            Colouring.Colouring.CharActions(cChar, editor);
                        }
                    }
                    Unsaved = true;
                    _wholeText = new string(editor.Text.ToCharArray());
                    UpdatIntelliSense();
                    EndUpdate(editor);
                    if (!Intellisensing)
                        return;
                    IntelliSensePopUp.Disselect();
                    Intellisensing = false;
                }));
            });

        private void TIDE_Load(object sender, EventArgs e)
        {
            IntelliSensePopUp.Show();
            HideIntelliSense();
            editor.Focus();
        }

        private void Editor_SelectionChanged(object sender, EventArgs eventArgs)
        {
            var pos = Colouring.Colouring.GetStringofArray(editor.SelectionStart, editor.Lines);
            PositionLabel.Text = string.Format(Resources.Line_Column, pos.Int1, pos.Int2);
            IntelliSensePopUp.Location = GetIntelliSensePosition();
            UpdatIntelliSense();
        }

        private void TIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Unsaved)
                return;
            var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning,
                MessageBoxButtons.YesNoCancel);
            switch (res)
            {
                case DialogResult.Yes:
                    SaveButton.PerformClick();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }

        private void editor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        private void TIDE_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;
            e.Handled = false;
            switch (e.KeyCode)
            {
                case Keys.F5:
                    RunButton.PerformClick();
                    break;
                case Keys.Escape:
                    HideIntelliSense();
                    break;
                case Keys.Tab:
                case Keys.Enter:
                    if (!IntelliSensePopUp.Visible)
                    {
                        if (e.KeyCode != Keys.Tab)
                            return;
                        SendKeys.Send(new string(' ', 4));
                        break;
                    }
                    IntelliSense_ItemSelected(IntelliSensePopUp.GetSelected());
                    break;
                case Keys.Down:
                    if (!IntelliSensePopUp.Visible)
                        return;
                    IntelliSensePopUp.ScrollDown();
                    break;
                case Keys.Up:
                    if (!IntelliSensePopUp.Visible)
                        return;
                    IntelliSensePopUp.ScrollUp();
                    break;
                default:
                    if (!e.Control)
                        return;
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            if (e.Shift)
                                SaveAsButton.PerformClick();
                            else
                                SaveButton.PerformClick();
                            break;
                        case Keys.O:
                            OpenButton.PerformClick();
                            break;
                        case Keys.N:
                            NewButton.PerformClick();
                            break;
                        case Keys.Space:
                            ShowIntelliSense();
                            break;
                        default:
                            return;
                    }
                    break;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void editor_KeyDown(object sender, KeyEventArgs e) => TIDE_KeyDown(sender, e);

        private void ToolBar_KeyDown(object sender, KeyEventArgs e) => TIDE_KeyDown(sender, e);

        private void tabControl_KeyDown(object sender, KeyEventArgs e) => TIDE_KeyDown(sender, e);

        private void TIDE_ResizeEnd(object sender, EventArgs e)
            => IntelliSensePopUp.Location = GetIntelliSensePosition();

        #endregion

        #region Update

        private const int EmSetEventMask = 0x0400 + 69;
        private const int WmSetredraw = 0x0b;
        private IntPtr _oldEventMask;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private void BeginUpdate(RichTextBox tb)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action<RichTextBox>) BeginUpdate, tb);
                return;
            }
            SendMessage(tb.Handle, WmSetredraw, IntPtr.Zero, IntPtr.Zero);
            _oldEventMask = SendMessage(tb.Handle, EmSetEventMask, IntPtr.Zero, IntPtr.Zero);
        }

        private void EndUpdate(RichTextBox tb)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action<RichTextBox>) EndUpdate, tb);
                return;
            }
            SendMessage(tb.Handle, WmSetredraw, (IntPtr) 1, IntPtr.Zero);
            SendMessage(tb.Handle, EmSetEventMask, IntPtr.Zero, _oldEventMask);
        }

        #endregion
    }
}