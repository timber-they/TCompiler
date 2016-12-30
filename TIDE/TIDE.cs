using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TCompiler.Main;
using TIDE.Colour;
using TIDE.Forms;
using TIDE.Properties;
using TIDE.StringFunctions;
using TIDE.Types;

namespace TIDE
{
    public partial class TIDE : Form
    {
        private static IntelliSensePopUp _intelliSensePopUp;
        private string _savePath;

        private bool _unsaved;

        public TIDE()
        {
            _unsaved = true;
            SavePath = null;
            InitializeComponent();

            _intelliSensePopUp = new IntelliSensePopUp(GetUpdatedItems(), GetPosition()) {Visible = false};
            _intelliSensePopUp.Show();
            _intelliSensePopUp.Visible = false;
            _intelliSensePopUp.ItemEntered += (sender, s) => OnItemSelected(s);
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
            var lw = editor.Text.Split(PublicStuff.Splitters).LastOrDefault();

            editor.Text += item.Substring(lw?.Length??0);
        }

        private static void CharActions(stringint cChar, RichTextBox tbox)
        {
            if ((cChar?.Thestring == null) || (cChar.Thestring.Length <= 0)) return;
            if (PublicStuff.Splitters.Contains(cChar.Thestring[0]))
                ColourSth.Colour_FromTo(
                    new intint(cChar.Theint, cChar.Theint + 1),
                    tbox,
                    PublicStuff.SplitterColor);
        }

        private IEnumerable<string> GetUpdatedItems()
            =>
            PublicStuff.StringColorsTCode.Select(color => color.Thestring)
                .Where(s =>
                {
                    var current = GetCurrent.GetCurrentWord(editor.SelectionStart, editor);
                    return current == null || s.StartsWith(current.Thestring);
                });

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

        private void editor_TextChanged(object sender, EventArgs e)
        {
            var word = GetCurrent.GetCurrentWord(editor.SelectionStart, editor);
            WordActions(word, editor);
            var cChar = GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor);
            CharActions(cChar, editor);
            _unsaved = true;
            _intelliSensePopUp.UpdateList(GetUpdatedItems());
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
            _unsaved = false;
            File.WriteAllText(SavePath, editor.Text);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (_unsaved)
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
            editor.Text = File.ReadAllText(SavePath);
            ColourAll(editor);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
            =>
            ColourAll(tabControl.SelectedTab == assemblerPage ? assemblerTextBox : editor,
                tabControl.SelectedTab == assemblerPage);


        private void EditorOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            var pos = GetStringofArray(editor.SelectionStart, editor.Text.Split('\n'));
            PositionLabel.Text = string.Format(Resources.Line_Column, pos.Int1, pos.Int2);
            _intelliSensePopUp.Location = GetPosition();
        }

        private Point GetPosition()
        {
            var pos = editor.PointToScreen(editor.GetPositionFromCharIndex(editor.SelectionStart));
            return new Point(pos.X, pos.Y + Cursor.Size.Height);
        }

        private static intint GetStringofArray(int pos, IReadOnlyList<string> lines)
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
            return new intint(c - 1, lc);
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (_unsaved)
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
            if (!_unsaved) return;
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
            switch (e.KeyCode)
            {
                case Keys.F5:
                    RunButton.PerformClick();
                    break;
                case Keys.Escape:
                    _intelliSensePopUp.Visible = false;
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
                                _intelliSensePopUp.Visible = true;
                                Focus();
                                break;
                        }
                    break;
            }
        }

        private void editor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) => TIDE_PreviewKeyDown(sender, e);

        private void HelpButton_Click(object sender, EventArgs e)
            =>
            MessageBox.Show(
                string.Format(Resources.help_Text,
                    string.Join("\n", PublicStuff.StringColorsTCode.Select(color => color.Thestring))), Resources.Help);
    }
}