using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TCompiler.Enums;
using TCompiler.Main;
using TCompiler.Settings;
using TIDE.Colour;
using TIDE.Forms;
using TIDE.Properties;
using TIDE.StringFunctions;
using TIDE.Types;

namespace TIDE
{
    // ReSharper disable once InconsistentNaming
    public partial class TIDE : Form
    {
        private IntelliSensePopUp IntelliSensePopUp { get; }
        private string _savePath;

        private bool Unsaved { get; set; }

        private bool _intellisensing;

        public TIDE()
        {
            _intellisensing = false;
            Unsaved = false;
            SavePath = null;
            InitializeComponent();

            IntelliSensePopUp = new IntelliSensePopUp(GetUpdatedItems(), GetPosition()) { Visible = false };
            IntelliSensePopUp.ItemEntered += (sender, s) => OnItemSelected(s);
            Focus();
        }

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

        private void OnItemSelected(string item)
        {
            _intellisensing = true;
            var pos = editor.SelectionStart;
            var lw = GetCurrent.GetCurrentWord(pos, editor).Thestring;
            var s = item.Substring(item.Length >= (lw?.Length ?? 0) ? lw?.Length ?? 0 : 0) + " ";
            Focus();
            SendKeys.Send(s); //Because this is hilarious
        }

        private IEnumerable<string> GetUpdatedItems()
        {
            var fin = PublicStuff.StringColorsTCode.Select(color => color.Thestring).Concat(GetVariableNames())
                .Where(s =>
                {
                    var current =
                        PublicStuff.Splitters.Any(
                            c =>
                                c.ToString() ==
                                GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor)?.Thestring)
                            ? ""
                            : GetCurrent.GetCurrentWord(editor.SelectionStart, editor)?.Thestring;
                    return string.IsNullOrEmpty(current) || s.StartsWith(current, true, CultureInfo.InvariantCulture);
                }).Distinct().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            fin.Sort();
            return fin;
        }

        private void editor_TextChanged(object sender = null, EventArgs e = null)
        {
            BeginUpdate();
            var word = GetCurrent.GetCurrentWord(editor.SelectionStart, editor);
            WordActions(word, editor);
            var cChar = GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor);
            CharActions(cChar, editor);
            Unsaved = true;
            UpdatIntelliSense();
            EndUpdate();
            if (!_intellisensing) return;
            IntelliSensePopUp.Disselect();
            _intellisensing = false;
        }

        private void UpdatIntelliSense()
        {
            IntelliSensePopUp.UpdateList(GetUpdatedItems());
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            SaveButton.PerformClick();
            if (SavePath == null)
            {
                MessageBox.Show(Resources.You_have_to_save_first, Resources.Error);
                return;
            }
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
            tabControl.SelectTab(assemblerPage);
        }

        private void TIDE_Load(object sender, EventArgs e)
        {
            IntelliSensePopUp.Show();
            HideIntelliSense();
            editor.Focus();
        }

        private void SaveButton_Click(object sender, EventArgs e) => Save(false);

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

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (Unsaved)
            {
                var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning, MessageBoxButtons.YesNoCancel);
                switch (res)
                {
                    case DialogResult.Yes:
                        SaveButton.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
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
            editor.TextChanged += editor_TextChanged;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
            =>
            ColourAll(tabControl.SelectedTab == assemblerPage ? assemblerTextBox : editor,
                tabControl.SelectedTab == assemblerPage);


        private void EditorOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            var pos = GetStringofArray(editor.SelectionStart, editor.Text.Split('\n'));
            PositionLabel.Text = string.Format(Resources.Line_Column, pos.Int1, pos.Int2);
            IntelliSensePopUp.Location = GetPosition();
            UpdatIntelliSense();
        }

        private Point GetPosition()
        {
            var pos = editor.PointToScreen(editor.GetPositionFromCharIndex(editor.SelectionStart));
            return new Point(pos.X, pos.Y + Cursor.Size.Height);
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (Unsaved)
            {
                var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning, MessageBoxButtons.YesNoCancel);
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

        private void TIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Unsaved) return;
            var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning, MessageBoxButtons.YesNoCancel);
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

        private void TIDE_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        private void editor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) => TIDE_PreviewKeyDown(sender, e);

        private void HelpButton_Click(object sender, EventArgs e)
            =>
            MessageBox.Show(
                string.Format(Resources.help_Text,
                    string.Join("\n", PublicStuff.StringColorsTCode.Select(color => color.Thestring))), Resources.Help);

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
                        return;
                    OnItemSelected(IntelliSensePopUp.GetSelected());
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
                    if (e.Control)
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
                    else
                        return;
                    break;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void ShowIntelliSense()
        {
            IntelliSensePopUp.Visible = true;
            IntelliSensePopUp.SelectIndex(0);
            Focus();
        }

        private void HideIntelliSense()
        {
            IntelliSensePopUp.Visible = false;
        }

        private void editor_KeyDown(object sender, KeyEventArgs e) => TIDE_KeyDown(sender, e);

        private void TIDE_ResizeEnd(object sender, EventArgs e) => IntelliSensePopUp.Location = GetPosition();

        private IEnumerable<string> GetVariableNames()
        {
            var fin = new List<string>(GlobalSettings.StandardVariables.Select(variable => variable.Name));
            VariableType foo;
            fin.AddRange(editor.Text.Split('\n').Where(s => s.Split(' ').Length > 1 && Enum.TryParse(s.Split(' ')[0], true, out foo)).Select(s => s.Split(' ')[1]));
            return fin;
        }



        #region Imagine this stuff being in another class

        private static Intint GetStringofArray(int pos, IReadOnlyList<string> lines)
        {
            var a = 0;
            var c = 0;
            var lc = pos;

            while (a <= pos)
            {
                a += lines[c].Length + 1;
                if (a <= pos)
                    lc -= lines[c].Length + 1;
                c++;
            }
            return new Intint(c - 1, lc);
        }

        private static void WordActions(stringint word, RichTextBox tbox, bool asm = false)
        {
            if (word == null) return;
            var color = EvaluateIfColouredAndGetColour.IsColouredAndColor(word.Thestring, asm);
            ColourSth.Colour_FromTo(
                GetRangeWithStringInt.GetRangeWithStringIntSpaces(
                    word,
                    tbox.Text.Split(PublicStuff.Splitters)),
                tbox,
                color);
        }

        private static void ColourAll(RichTextBox tbox, bool asm = false)
        {
            foreach (var c in GetCurrent.GetAllChars(tbox))
                CharActions(c, tbox);
            foreach (var word in GetCurrent.GetAllWords(tbox))
                WordActions(word, tbox, asm);
        }

        private static void CharActions(stringint cChar, RichTextBox tbox)
        {
            if ((cChar?.Thestring == null) || (cChar.Thestring.Length <= 0)) return;
            if (PublicStuff.Splitters.Contains(cChar.Thestring[0]) && !char.IsWhiteSpace(cChar.Thestring[0]))
                ColourSth.Colour_FromTo(
                    new Intint(cChar.Theint, cChar.Theint + 1),
                    tbox,
                    PublicStuff.SplitterColor);
        }

        #endregion

        #region Update
        
        private const int EmSetEventMask = 0x0400 + 69;
        private const int WmSetredraw = 0x0b;
        private IntPtr _oldEventMask;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private void BeginUpdate()
        {
            SendMessage(editor.Handle, WmSetredraw, IntPtr.Zero, IntPtr.Zero);
            _oldEventMask = SendMessage(editor.Handle, EmSetEventMask, IntPtr.Zero, IntPtr.Zero);
        }

        private void EndUpdate()
        {
            SendMessage(editor.Handle, WmSetredraw, (IntPtr)1, IntPtr.Zero);
            SendMessage(editor.Handle, EmSetEventMask, IntPtr.Zero, _oldEventMask);
        }
#endregion
    }
}