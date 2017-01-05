using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TIDE.Colouring.Types;

namespace TIDE.Colouring.StringFunctions
{
    public static class GetCurrent
    {
        public static Stringint GetCurrentWord(int pos, RichTextBox text)
            => GetStringofArray(pos, text.Text.Split(PublicStuff.Splitters));

        public static IEnumerable<Stringint> GetAllWords(RichTextBox text)
            => text.Text.Split(PublicStuff.Splitters).Select((s, i) =>
            {
                i++;
                return i > 0 ? new Stringint(s, i - 1) : null;
            });

        public static IEnumerable<Stringint> GetAllChars(RichTextBox text)
            => text.Text.ToCharArray().Select((c, i) => i >= 0 ? new Stringint(c.ToString(), i) : null);

        public static IEnumerable<Stringint> GetCurrentLine(RichTextBox text)
        {
            var off = -1;
            var ln = text.GetLineFromCharIndex(text.SelectionStart);
            for (var i = 0; i < ln; i++)
                off += text.Lines[i].Split(PublicStuff.Splitters).Length;
            var fin = new List<Stringint>();
            foreach (var s in text.Lines[ln].Split(PublicStuff.Splitters))
            {
                off++;
                fin.Add(new Stringint(s, off));
            }
            return fin;
        }

        private static Stringint GetStringofArray(int pos, IReadOnlyList<string> strings)
        {
            var cpos = 0;
            var apos = -1;
            while ((cpos < pos) && (apos < strings.Count - 1))
            {
                apos++;
                cpos += strings[apos].Length + 1;
            }
            return apos >= 0 ? new Stringint(strings[apos], apos) : null;
        }

        public static Stringint GetCurrentCharacter(int pos, RichTextBox text)
            => pos > 0 ? new Stringint(text.Text.ToCharArray()[pos - 1].ToString(), pos - 1) : null;
    }
}