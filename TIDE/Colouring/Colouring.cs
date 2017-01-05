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

        public static void WordActions(Stringint word, RichTextBox tbox, bool asm = false, bool useEndOfLine = false)
        {
            if (word == null || tbox.Lines.Length == 0) return;

            var line =
                tbox.Lines.ToArray()[
                    tbox.GetLineFromCharIndex(tbox.SelectionStart > 0 ? tbox.SelectionStart - 1 : tbox.SelectionStart)
                ];
            var linePos = useEndOfLine
                ? tbox.GetFirstCharIndexOfCurrentLine() +
                  tbox.Lines[tbox.GetLineFromCharIndex(tbox.SelectionStart)].Length
                : (tbox.GetLineFromCharIndex(tbox.SelectionStart) <= 0 ||
                   tbox.SelectionStart - tbox.GetFirstCharIndexOfCurrentLine() > 0
                    ? tbox.SelectionStart - tbox.GetFirstCharIndexOfCurrentLine()
                    : tbox.Lines[tbox.GetLineFromCharIndex(tbox.SelectionStart - 1)].Length +
                      tbox.GetFirstCharIndexFromLine(tbox.GetLineFromCharIndex(tbox.SelectionStart - 1)));
            linePos = tbox.SelectionStart - tbox.GetFirstCharIndexOfCurrentLine() > 0 && !useEndOfLine ? linePos - 1 : linePos;

            var color = EvaluateIfColouredAndGetColour.IsColouredAndColor(word.Thestring, asm, line, linePos);
            ColourSth.Colour_FromTo(
                GetRangeWithStringInt.GetRangeWithStringIntSpaces(
                    word,
                    tbox.Text.Split(PublicStuff.Splitters)),
                tbox,
                color);
        }

        public static void CharActions(Stringint cChar, RichTextBox tbox, bool useEndOfLine = false)
        {
            if ((cChar?.Thestring == null) || (cChar.Thestring.Length <= 0)) return;

            var line =
                tbox.Lines.ToArray()[
                    tbox.GetLineFromCharIndex(tbox.SelectionStart > 0 ? tbox.SelectionStart - 1 : tbox.SelectionStart)
                ];
            var linePos = useEndOfLine
                ? tbox.GetFirstCharIndexOfCurrentLine() +
                  tbox.Lines[tbox.GetLineFromCharIndex(tbox.SelectionStart)].Length
                : (tbox.GetLineFromCharIndex(tbox.SelectionStart) <= 0 ||
                   tbox.SelectionStart - tbox.GetFirstCharIndexOfCurrentLine() > 0
                    ? tbox.SelectionStart - tbox.GetFirstCharIndexOfCurrentLine()
                    : tbox.Lines[tbox.GetLineFromCharIndex(tbox.SelectionStart - 1)].Length +
                      tbox.GetFirstCharIndexFromLine(tbox.GetLineFromCharIndex(tbox.SelectionStart - 1)));
            linePos = tbox.SelectionStart - tbox.GetFirstCharIndexOfCurrentLine() > 0 && !useEndOfLine ? linePos - 1 : linePos;

            if (!PublicStuff.Splitters.Contains(cChar.Thestring[0]) || char.IsWhiteSpace(cChar.Thestring[0])) return;
            var semiIndex = line.ToCharArray().ToList().IndexOf(';');
            ColourSth.Colour_FromTo(
                new Intint(cChar.Theint, cChar.Theint + 1),
                tbox, semiIndex >= 0 && semiIndex <= linePos
                    ? PublicStuff.CommentColor
                    : PublicStuff.SplitterColor);
        }

        public static void ColourCurrentLine(RichTextBox tbox, bool chars = false)
        {
            if (chars)
                foreach (var c in GetCurrent.GetAllChars(tbox))
                    CharActions(c, tbox, true);
            foreach (var word in GetCurrent.GetCurrentLine(tbox))
                WordActions(word, tbox, false, true);
        }
    }
}