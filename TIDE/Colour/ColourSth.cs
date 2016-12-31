using System.Drawing;
using System.Windows.Forms;
using TIDE.Types;

namespace TIDE.Colour
{
    public class ColourSth
    {
        public static void Colour_FromTo(intint area, RichTextBox text, Color colour, bool back = false)
        {
            var pos = text.SelectionStart;
            text.Select(area.Int1, area.Int2 - area.Int1);
            if ((!back || text.SelectionBackColor != colour) && (back || text.SelectionColor != colour))
            {
                if (!back)
                    text.SelectionColor = colour;
                else
                    text.SelectionBackColor = colour;
            }
            text.Select(pos, 0);
            text.SelectionColor = text.ForeColor;
            if (text.SelectionBackColor != text.BackColor)
                text.SelectionBackColor = text.BackColor;
        }

        public static void HighlightLine(int line, RichTextBox text, Color color)
            => Colour_FromTo(GetLine(line, text.Text), text, color, true);

        private static intint GetLine(int line, string text)
        {
            var pos = 0;
            for (var i = 0; i < line; i++)
                pos += text.Split('\n')[i].Length + 1;
            return new intint(pos, pos + text.Split('\n')[line].Length);
        }
    }
}