#region

using System;
using System.Linq;
using TIDE.Coloring.StringFunctions;
using TIDE.Coloring.Types;
using TIDE.Forms;

#endregion

namespace TIDE.Coloring
{
    /// <summary>
    ///     Provides some coloring methods
    /// </summary>
    public static class Coloring
    {
        /// <summary>
        ///     Does the stuff that has to be done with a word
        /// </summary>
        /// <param name="word">The actual word</param>
        /// <param name="textBox">The textBox in which the word is</param>
        /// <param name="asm">Indicates wether the word is from assembler</param>
        public static void WordActions(Word word, TideTextBox textBox, bool asm = false)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new Action(() => WordActions(word, textBox, asm)));
                return;
            }

            if (string.IsNullOrWhiteSpace(word?.Value) || textBox.Lines.Length == 0)
                return;

            var lineIndex =
                textBox.GetLineFromCharIndex(word.Position);
            var line =
                textBox.Lines.ToArray()[lineIndex];
            var linePos = word.Position - textBox.GetFirstCharIndexFromLine(lineIndex);

            textBox.Color_FromTo(
                GetRangeWithWord.GetRangeWithWordSpaces(
                    word,
                    textBox.Text.Split(PublicStuff.Splitters)),
                EvaluateColor.GetColor(word.Value, asm, line, linePos));
        }

        /// <summary>
        ///     Does the stuff that has to be done with a char
        /// </summary>
        /// <param name="char">The character with which stuff has to be done</param>
        /// <param name="textBox">The textBox in which the character is</param>
        public static void CharActions(Character @char, TideTextBox textBox)
        {
            if (@char?.Value == null || char.IsWhiteSpace(@char.Value) || !PublicStuff.Splitters.Contains(@char.Value) ||
                @char.Value == '_')
                return;

            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new Action(() => CharActions(@char, textBox)));
                return;
            }

            var lineIndex = textBox.GetLineFromCharIndex(@char.Position);
            var linePos = @char.Position - textBox.GetFirstCharIndexFromLine(lineIndex);
            var semiIndex = textBox.Lines.ToArray()[lineIndex].ToCharArray().ToList().IndexOf(';');
            textBox.Color_FromTo(
                new Range(@char.Position, @char.Position + 1),
                semiIndex >= 0 && semiIndex <= linePos
                    ? PublicStuff.CommentColor
                    : PublicStuff.SplitterColor);
        }
    }
}