#region

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TIDE.Coloring.color;
using TIDE.Coloring.StringFunctions;
using TIDE.Coloring.Types;

#endregion

namespace TIDE.Coloring
{
    /// <summary>
    /// Provides some coloring methods
    /// </summary>
    public static class Coloring
    {
        /// <summary>
        /// Evaluates the range of a string of an array
        /// </summary>
        /// <param name="pos">The pos in the text</param>
        /// <param name="lines">The lines of the text</param>
        /// <returns>The range</returns>
        public static Range GetStringofArray(int pos, IReadOnlyList<string> lines)
        {
            var a = 0;
            var c = 0;
            var lc = pos;

            while ((a <= pos) && (c < lines.Count))
            {
                a += lines[c].Length + 1;
                if (a <= pos)
                    lc -= lines[c].Length + 1;
                c++;
            }
            return new Range(c > 0 ? c - 1 : c, lc);
        }

        /// <summary>
        /// Does the stuff that has to be done with a word
        /// </summary>
        /// <param name="word">The actual word</param>
        /// <param name="textBox">The textBox in which the word is</param>
        /// <param name="asm">Indicates wether the word is from assembler</param>
        public static void WordActions(Word word, RichTextBox textBox, bool asm = false)
        {
            if (string.IsNullOrEmpty(word?.Value) || (textBox.Lines.Length == 0))
                return;

            var lineIndex =
                textBox.GetLineFromCharIndex(word.Position);
            var line =
                textBox.Lines.ToArray()[lineIndex];
            var linePos = word.Position - textBox.GetFirstCharIndexFromLine(lineIndex);

            var color = EvaluateColor.GetColor(word.Value, asm, line, linePos);
            ColorSomething.color_FromTo(
                GetRangeWithWord.GetRangeWithWordSpaces(
                    word,
                    textBox.Text.Split(PublicStuff.Splitters)),
                textBox,
                color);
        }

        /// <summary>
        /// Does the stuff that has to be done with a char
        /// </summary>
        /// <param name="char">The character with which stuff has to be done</param>
        /// <param name="textBox">The textBox in which the character is</param>
        public static void CharActions(Character @char, RichTextBox textBox)
        {
            if (@char?.Value == null)
                return;

            var lineIndex = textBox.GetLineFromCharIndex(@char.Position);

            var line = textBox.Lines.ToArray()[lineIndex];
            var linePos = @char.Position - textBox.GetFirstCharIndexFromLine(lineIndex);

            if (!PublicStuff.Splitters.Contains(@char.Value) || char.IsWhiteSpace(@char.Value))
                return;
            var semiIndex = line.ToCharArray().ToList().IndexOf(';');
            ColorSomething.color_FromTo(
                new Range(@char.Position, @char.Position + 1),
                textBox, (semiIndex >= 0) && (semiIndex <= linePos)
                    ? PublicStuff.CommentColor
                    : PublicStuff.SplitterColor);
        }

        /// <summary>
        /// Colors the current line
        /// </summary>
        /// <param name="textBox">The textBox in which the current line has to be coloured</param>
        public static void ColorCurrentLine(RichTextBox textBox)
        {
            foreach (var c in GetCurrent.GetCurrentLineChars(textBox))
                CharActions(c, textBox);
            foreach (var word in GetCurrent.GetCurrentLineWords(textBox))
                WordActions(word, textBox);
        }
    }
}