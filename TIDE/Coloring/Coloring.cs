#region

using System;
using System.Linq;

using MetaTextBoxLibrary;

using TIDE.Coloring.StringFunctions;
using TIDE.Coloring.Types;

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
        public static void WordActions (Word word, MetaTextBox textBox, bool asm = false)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke (new Action (() => WordActions (word, textBox, asm)));
                return;
            }

            if (string.IsNullOrWhiteSpace (word?.Value) || textBox.Lines.Count == 0)
                return;

            var lineIndex =
                textBox.GetLineFromCharIndex (word.Position);

            var range = GetRangeWithWord.GetRangeWithWordSpaces (
                word,
                textBox.Text.ToString ().Split (PublicStuff.Splitters));
            textBox.ColorRange (
                range.Beginning, range.Ending - range.Beginning,
                EvaluateColor.GetColor (word.Value, asm, textBox.Lines [lineIndex].ToString (),
                                        word.Position - textBox.GetFirstCharIndexFromLine (lineIndex)));
        }

        /// <summary>
        ///     Does the stuff that has to be done with a char
        /// </summary>
        /// <param name="char">The character with which stuff has to be done</param>
        /// <param name="textBox">The textBox in which the character is</param>
        public static void CharActions (Character @char, MetaTextBox textBox)
        {
            if (@char?.Value == null ||
                char.IsWhiteSpace (@char.Value) ||
                !PublicStuff.Splitters.Contains (@char.Value) ||
                @char.Value == '_')
                return;

            if (textBox.InvokeRequired)
            {
                textBox.Invoke (new Action (() => CharActions (@char, textBox)));
                return;
            }

            var lineIndex = textBox.GetLineFromCharIndex (@char.Position);
            var semiIndex = textBox.Lines.ToArray () [lineIndex].ToCharArray ().ToList ().IndexOf (';');
            textBox.ColorRange (
                @char.Position, 1,
                semiIndex >= 0 && semiIndex <= @char.Position - textBox.GetFirstCharIndexFromLine (lineIndex)
                    ? PublicStuff.CommentColor
                    : PublicStuff.SplitterColor);
        }
    }
}