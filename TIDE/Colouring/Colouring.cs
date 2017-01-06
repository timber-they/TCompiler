using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TIDE.Colouring.Colour;
using TIDE.Colouring.StringFunctions;
using TIDE.Colouring.Types;

namespace TIDE.Colouring
{
    public static class Colouring
    {
        public static Intint GetStringofArray(int pos, IReadOnlyList<string> lines)
        {
            var a = 0;
            var c = 0;
            var lc = pos;

            while (a <= pos && c < lines.Count)
            {
                a += lines[c].Length + 1;
                if (a <= pos)
                    lc -= lines[c].Length + 1;
                c++;
            }
            return new Intint(c > 0 ? c - 1 : c, lc);
        }

        public static void WordActions(Word word, RichTextBox tbox, bool asm = false)
        {
            if (string.IsNullOrEmpty(word?.Value) || tbox.Lines.Length == 0)
                return;

            var lineIndex =
                tbox.GetLineFromCharIndex(word.Position);
            var line =
                tbox.Lines.ToArray()[lineIndex];
            var linePos = word.Position - tbox.GetFirstCharIndexFromLine(lineIndex);

            var color = EvaluateColour.GetColor(word.Value, asm, line, linePos);
            ColourSth.Colour_FromTo(
                GetRangeWithStringInt.GetRangeWithStringIntSpaces(
                    word,
                    tbox.Text.Split(PublicStuff.Splitters)),
                tbox,
                color);
        }

        public static void CharActions(Character cChar, RichTextBox tbox)
        {
            if ((cChar?.Value == null))
                return;

            var lineIndex = tbox.GetLineFromCharIndex(cChar.Position);

            var line = tbox.Lines.ToArray()[lineIndex];
            var linePos = cChar.Position - tbox.GetFirstCharIndexFromLine(lineIndex);

            if (!PublicStuff.Splitters.Contains(cChar.Value) || char.IsWhiteSpace(cChar.Value))
                return;
            var semiIndex = line.ToCharArray().ToList().IndexOf(';');
            ColourSth.Colour_FromTo(
                new Intint(cChar.Position, cChar.Position + 1),
                tbox, semiIndex >= 0 && semiIndex <= linePos
                    ? PublicStuff.CommentColor
                    : PublicStuff.SplitterColor);
        }

        public static void ColourCurrentLine(RichTextBox tbox)
        {
            foreach (var c in GetCurrent.GetCurrentLineChars(tbox))
                CharActions(c, tbox);
            foreach (var word in GetCurrent.GetCurrentLineWords(tbox))
                WordActions(word, tbox);
        }
    }
}