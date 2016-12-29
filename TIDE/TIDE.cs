using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCompiler.Main;
using TIDE.Colour;
using TIDE.Properties;
using TIDE.StringFunctions;
using TIDE.Types;

namespace TIDE
{
    public partial class TIDE : Form
    {
        private string _savePath;
        private bool _unsaved;

        public TIDE()
        {
            _unsaved = true;
            _savePath = null;
            InitializeComponent();
        }

        private static void CharActions(stringint cchar, RichTextBox tbox)
        {
            if (cchar?.Thestring == null || cchar.Thestring.Length <= 0) return;
            if (PublicStuff.Splitters.Contains(cchar.Thestring[0]))
                ColourSth.Colour_FromTo(
                    new intint(cchar.Theint, cchar.Theint + 1),
                    tbox,
                    PublicStuff.SplitterColor);
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

        private void editor_TextChanged(object sender, EventArgs e)
        {
            var word = GetCurrent.GetCurrentWord(editor.SelectionStart, editor);
            WordActions(word, editor);
            var cchar = GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor);
            CharActions(cchar, editor);
            _unsaved = true;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            SaveButton.PerformClick();
            if (_savePath == null)
            {
                MessageBox.Show("You have to save the file first!", "Error");
                return;
            }
            Main.Initialize(_savePath, "out.asm", "error.txt");
            var ex = Main.CompileFile();
            if (ex != null)
            {
                if(ex.Line >= 0)
                    ColourSth.HighlightLine(ex.Line, editor, Color.Red);
                MessageBox.Show(File.ReadAllText("error.txt"), Resources.Error);
                if(ex.Line >= 0)
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
            if (_savePath == null || showDialogue)
            {
                var dia = new SaveFileDialog
                {
                    AddExtension = true,
                    OverwritePrompt = true,
                    Title = "Save",
                    Filter = "*.tc|*.tc",
                    DefaultExt = "tc"
                };
                if (dia.ShowDialog() != DialogResult.OK)
                    return;
                _savePath = dia.FileName;
            }
            _unsaved = false;
            File.WriteAllText(_savePath, editor.Text);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (_unsaved)
            {
                var res = MessageBox.Show("Do you want to save your changes?", "Warning", MessageBoxButtons.YesNoCancel);
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
                Title = "Open",
                Filter = "*.tc|*.tc",
                DefaultExt = "tc"
            };
            if (dia.ShowDialog() != DialogResult.OK)
                return;
            _savePath = dia.FileName;
            editor.Text = File.ReadAllText(_savePath);
            ColourAll(editor);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e) => ColourAll(tabControl.SelectedTab == assemblerPage ? assemblerTextBox : editor, tabControl.SelectedTab == assemblerPage);


        private void EditorOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            var pos = GetStringofArray(editor.SelectionStart, editor.Text.Split('\n'));
            PositionLabel.Text = $"Line: {pos.Int1}; Column: {pos.Int2}";
        }

        public static intint GetStringofArray(int pos, IReadOnlyList<string> lines)
        {
            var a = 0;
            var c = 0;
            var lc = pos;

            while (a <= pos)
            {
                a += lines[c].Length+1;
                if(a <= pos)
                    lc -= lines[c].Length + 1;
                c++;
            }
            return new intint(c-1, lc);
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (_unsaved)
            {
                var res = MessageBox.Show("Do you want to save your changes?", "Warning", MessageBoxButtons.YesNoCancel);
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
            _savePath = null;
        }

        private void SaveAsButton_Click(object sender, EventArgs e) => Save(true);

        private void TIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_unsaved) return;
            var res = MessageBox.Show("Do you want to save your changes?", "Warning", MessageBoxButtons.YesNoCancel);
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
            if(e.KeyCode == Keys.F5)
                RunButton.PerformClick();
            else if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        if(e.Shift)
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
                }
            }
        }

        private void editor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) => TIDE_PreviewKeyDown(sender, e);
    }
}
