#region

using System.Drawing;
using System.Windows.Forms;
using TIDE.Coloring.Types;

#endregion

namespace TIDE.Coloring.color
{
    /// <summary>
    ///     A class that provides methods for coloring text from a RichTextBox
    /// </summary>
    public static class ColorSomething
    {
        /// <summary>
        ///     colors the text in the specified area
        /// </summary>
        /// <param name="area">The area in which the text shall get colored</param>
        /// <param name="textBox">The textBox in which the text shall get colored</param>
        /// <param name="color">The new color of the text in the area</param>
        /// <param name="back">Indicates wether the background and not the foreground color shall get changed</param>
        /// <param name="resetCursor">Indicates wether to reset the cursor position to the position it was before</param>
        public static void color_FromTo(Range area, RichTextBox textBox, Color color, bool back = false,
            bool resetCursor = true)
        {
            var pos = textBox.SelectionStart;
            textBox.Select(area.Beginning, area.Ending - area.Beginning);
            if ((!back || (textBox.SelectionBackColor != color))/* && (back || (textBox.SelectionColor != color))*/)
                if (!back)
                    textBox.SelectionColor = color;
                else
                    textBox.SelectionBackColor = color;
            if (resetCursor)
                textBox.Select(pos, 0);
            else if (area.Beginning > 0)
                textBox.Select(area.Beginning - 1, 0);
            else if (textBox.Text.Length > area.Beginning + 1)
                textBox.Select(area.Beginning + 1, 0);
            else
                textBox.Select(0, 0);
            textBox.SelectionColor = textBox.ForeColor;
            if (textBox.SelectionBackColor != textBox.BackColor)
                textBox.SelectionBackColor = textBox.BackColor;
        }

        /// <summary>
        ///     Highlights the specified line
        /// </summary>
        /// <param name="lineIndex">The lineIndex of the line that shall get highlighted</param>
        /// <param name="textBox">The textBox in which the line shall get highlighted</param>
        /// <param name="color">The new background color of the text</param>
        public static void HighlightLine(int lineIndex, RichTextBox textBox, Color color)
        {
            color_FromTo(GetLine(lineIndex, textBox.Text), textBox, color, true, false);
        }

        /// <summary>
        ///     Evaluates the line area of the specified lineIndex
        /// </summary>
        /// <param name="lineIndex">The line index of the line of which the area is wanted</param>
        /// <param name="text">The text of which the area will be</param>
        /// <returns></returns>
        private static Range GetLine(int lineIndex, string text)
        {
            var pos = 0;
            for (var i = 0; i < lineIndex; i++)
                pos += text.Split('\n')[i].Length + 1;
            return new Range(pos, pos + text.Split('\n')[lineIndex].Length);
        }
    }
}