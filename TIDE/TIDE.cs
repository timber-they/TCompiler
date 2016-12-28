using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIDE.Colour;
using TIDE.StringFunctions;
using TIDE.Types;

namespace TIDE
{
    public partial class TIDE : Form
    {
        public TIDE()
        {
            InitializeComponent();
        }

        private void TIDE_TextChanged(object sender, EventArgs e)
        {
        }

        private static void CharActions(stringint cchar, RichTextBox tbox)
        {
            if (cchar?.Thestring == null || cchar.Thestring.Length <= 0) return;
            if (PublicStuff.Splitters.Contains(cchar.Thestring[0]))
                ColourSth.Colour_FromTo(
                    new intint(cchar.Theint, tbox.SelectionStart),
                    tbox,
                    PublicStuff.SplitterColor);
        }

        public static void WordActions(stringint word, RichTextBox tbox)
        {
            if (word == null) return;
            var color = EvaluateIfColouredAndGetColour.IsColouredAndColor(word.Thestring);
            ColourSth.Colour_FromTo(
                    GetRangeWithstringint.GetRangeWithstringintSpaces(
                        word,
                        tbox.Text.Split(PublicStuff.Splitters)),
                    tbox,
                    color);

        }

        private void editor_TextChanged(object sender, EventArgs e)
        {
            var word = GetCurrent.GetCurrentWord(editor.SelectionStart, editor);
            WordActions(word, editor);
            var cchar = GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor);
            CharActions(cchar, editor);
        }
    }
}
