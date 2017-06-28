#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MetaTextBoxLibrary;

using TIDE.Coloring.StringFunctions;
using TIDE.Forms.Tools;


// ReSharper disable LocalizableElement

#endregion


namespace TIDE.Forms
{
    public class TideTextBox : MetaTextBox
    {
        /// <summary>
        ///     Colors the whole document
        /// </summary>
        /// <param name="asm">Indicates wether assembler code is colored</param>
        public void ColorAll (bool asm = false) => Task.Run (() =>
        {
            foreach (var c in GetCurrent.GetAllChars (this))
                Coloring.Coloring.CharActions (c, this);
            foreach (var word in GetCurrent.GetAllWords (this))
                Coloring.Coloring.WordActions (word, this, asm);
        });

        /// <summary>
        ///     Colors the current line
        /// </summary>
        public void ColorCurrentLine ()
        {
            var clone = (MetaTextBox) Clone ();
            foreach (var c in GetCurrent.GetCurrentLineChars (clone))
                Coloring.Coloring.CharActions (c, clone);
            foreach (var word in GetCurrent.GetCurrentLineWords (clone))
                Coloring.Coloring.WordActions (word, clone);
        }

        /// <summary>
        ///     Formats the whole text of the textBox
        /// </summary>
        public void Format ()
        {
            var currentLine = GetLineFromCharIndex (CursorIndex);
            var trimmedCharIndexOfLine = CursorIndex -
                                         Lines [currentLine].ToCharArray ().TakeWhile (c => c == ' ').Count () -
                                         GetFirstCharIndexFromLine (currentLine);
            SetText (Formatting.FormatText (Text.Count () > 0 ? Text.Substring(0, Text.Count() - 1).ToString() : "") + "\n");
            SetCursorIndex (GetFirstCharIndexFromLine (currentLine) +
                            trimmedCharIndexOfLine +
                            Lines [currentLine].ToString ().TakeWhile (c => c == ' ').Count ());
            ColorAll ();
        }

        /// <summary>
        ///     Formats the specified lines of the TextBox
        /// </summary>
        /// <param name="lines">The lines to format</param>
        public void Format (List<int> lines)
        {
            var currentLine = GetLineFromCharIndex (CursorIndex);
            var trimmedCharIndexOfLine = CursorIndex -
                                         Lines [currentLine].ToCharArray ().TakeWhile (c => c == ' ').Count () -
                                         GetFirstCharIndexFromLine (currentLine);
            SetText (Formatting.FormatLines (Text.ToString (), lines));
            SetCursorIndex (GetFirstCharIndexFromLine (currentLine) +
                            trimmedCharIndexOfLine +
                            Lines [currentLine].ToCharArray ().TakeWhile (c => c == ' ').Count ());
            ColorAll ();
        }
    }
}