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

        public TIDE()
        {
            _savePath = null;
            InitializeComponent();
        }

        private static void CharActions(stringint cchar, RichTextBox tbox)
        {
            if (cchar?.Thestring == null || cchar.Thestring.Length <= 0) return;
            if (PublicStuff.Splitters.Contains(cchar.Thestring[0]))
                ColourSth.Colour_FromTo(
                    new intint(cchar.Theint, cchar.Theint+1),
                    tbox,
                    PublicStuff.SplitterColor);
        }

        private static void WordActions(stringint word, RichTextBox tbox, bool asm=false)
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

        private static void ColourAll(RichTextBox tbox, bool asm=false)
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
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            SaveButton_Click(null, null);
            Main.Initialize(_savePath, "out.asm", "error.txt");
            var ex = Main.CompileFile();
            if (ex != null)
            {
                ColourSth.HighlightLine(ex.Line, editor, Color.Red);
                MessageBox.Show(File.ReadAllText("error.txt"), Resources.Error);
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_savePath == null)
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
            File.WriteAllText(_savePath, editor.Text);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
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
        

        private void EditorOnSelectionChanged(object sender, EventArgs eventArgs) => PositionLabel.Text = $"Line: {GetStringofArray(editor.SelectionStart, editor.Lines)}";

        public static int GetStringofArray(int pos, IReadOnlyList<string> strings)
        {
            var cpos = 0;
            var apos = 0;
            while (cpos < pos && apos < strings.Count - 1)
            {
                apos++;
                cpos += strings[apos].Length + 1;
            }
            return apos;
        }
    }
}
