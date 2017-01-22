#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TIDE.Coloring.Types;

#endregion

namespace TIDE.Coloring.StringFunctions
{
    /// <summary>
    ///     Provides method for getting something current
    /// </summary>
    public static class GetCurrent
    {
        /// <summary>
        ///     Evaluates the current word
        /// </summary>
        /// <param name="pos">The position in the text</param>
        /// <param name="textBox">The textBox in which the word is</param>
        /// <returns>The found word as a word</returns>
        public static Word GetCurrentWord(int pos, RichTextBox textBox)
            => GetWordOfArray(pos, textBox.Text.Split(PublicStuff.Splitters));

        /// <summary>
        ///     Evaluates all words from the textBox
        /// </summary>
        /// <param name="textBox">The textBox in which the words are</param>
        /// <returns>An IEnumerable of all the words</returns>
        public static IEnumerable<Word> GetAllWords(RichTextBox textBox)
        {
            if (textBox.InvokeRequired)
                return (IEnumerable<Word>) textBox.Invoke(new Func<IEnumerable<Word>>(() => GetAllWords(textBox)));
            var count = 0;
            return textBox.Text.Split(PublicStuff.Splitters).Select((s, i) =>
            {
                var retValue = new Word(s, i, count);
                count += s.Length + 1;
                return retValue;
            });
        }

        /// <summary>
        ///     Evaluates all single characters in the textBox
        /// </summary>
        /// <param name="textBox">The textBox in which the characters are</param>
        /// <returns>An IEnumerable of all the characters</returns>
        public static IEnumerable<Character> GetAllChars(RichTextBox textBox) => textBox.InvokeRequired
            ? (IEnumerable<Character>) textBox.Invoke(new Func<IEnumerable<Character>>(() => GetAllChars(textBox)))
            : textBox.Text.Select((c, i) => i >= 0 ? new Character(c, i) : null);

        /// <summary>
        ///     Evaluates all the words of the current line
        /// </summary>
        /// <param name="textBox">The textBox in which the words are</param>
        /// <returns>A list of the words of the current line</returns>
        public static IEnumerable<Word> GetCurrentLineWords(RichTextBox textBox)
        {
            var off = 0;
            var count = 0;
            var ln = textBox.GetLineFromCharIndex(textBox.SelectionStart);
            for (var i = 0; i < ln; i++)
            {
                var line = textBox.Lines[i];
                off += line.Split(PublicStuff.Splitters).Length;
                count += line.Length + 1;
            }
            var fin = new List<Word>();
            foreach (var s in textBox.Lines[ln].Split(PublicStuff.Splitters))
            {
                fin.Add(new Word(s, off, count));
                off++;
                count += s.Length + 1;
            }
            return fin;
        }

        /// <summary>
        ///     Evaluates the word of the array at the position
        /// </summary>
        /// <param name="pos">The position of the word in the actual text</param>
        /// <param name="strings">The text splitted into single words</param>
        /// <returns>The word from the position</returns>
        private static Word GetWordOfArray(int pos, IReadOnlyList<string> strings)
        {
            var positionInText = 0;
            var positionInWordArray = -1;
            var oldPositionInText = 0;
            while ((positionInText <= pos) && (positionInWordArray < strings.Count - 1))
            {
                oldPositionInText = positionInText;
                positionInWordArray++;
                positionInText += strings[positionInWordArray].Length + 1;
            }
            return positionInWordArray >= 0
                ? new Word(strings[positionInWordArray], positionInWordArray, oldPositionInText)
                : null;
        }

        /// <summary>
        ///     Evaluates the current character at the specified position
        /// </summary>
        /// <param name="pos">The position of the character in the text</param>
        /// <param name="textBox">The textBox in which the character is</param>
        /// <returns>The character</returns>
        public static Character GetCurrentCharacter(int pos, RichTextBox textBox)
            => pos > 0 ? new Character(textBox.Text.ToCharArray()[pos - 1], pos - 1) : null;

        /// <summary>
        ///     Returns all characters of the current line
        /// </summary>
        /// <param name="textBox">The textBox in which the line is</param>
        /// <returns>A list of all the characters</returns>
        public static IEnumerable<Character> GetCurrentLineChars(RichTextBox textBox)
        {
            var off = -1;
            var ln = textBox.GetLineFromCharIndex(textBox.SelectionStart);
            for (var i = 0; i < ln; i++)
                off += textBox.Lines[i].Length + 1;
            var fin = new List<Character>();
            foreach (var c in textBox.Lines[ln].ToCharArray())
            {
                off++;
                if (!PublicStuff.Splitters.Contains(c) || char.IsWhiteSpace(c))
                    continue;
                fin.Add(new Character(c, off));
            }
            return fin;
        }
    }
}