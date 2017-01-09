#region

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TIDE.Colouring.Types;

#endregion

namespace TIDE.Colouring.StringFunctions
{
    public static class GetCurrent
    {
        public static Word GetCurrentWord(int pos, RichTextBox text)
            => GetStringofArray(pos, text.Text.Split(PublicStuff.Splitters));

        public static IEnumerable<Word> GetAllWords(RichTextBox text)
        {
            var count = 0;
            return text.Text.Split(PublicStuff.Splitters).Select((s, i) =>
            {
                var retValue = new Word(s, i, count);
                count += s.Length + 1;
                return retValue;
            });
        }

        public static IEnumerable<Character> GetAllChars(RichTextBox text) => text.Text.ToCharArray().Select((c, i) => i >= 0 ? new Character(c, i) : null);

        public static IEnumerable<Word> GetCurrentLineWords(RichTextBox text)
        {
            var off = 0;
            var count = 0;
            var ln = text.GetLineFromCharIndex(text.SelectionStart);
            for (var i = 0; i < ln; i++)
            {
                var line = text.Lines[i];
                off += line.Split(PublicStuff.Splitters).Length;
                count += line.Length + 1;
            }
            var fin = new List<Word>();
            foreach (var s in text.Lines[ln].Split(PublicStuff.Splitters))
            {
                fin.Add(new Word(s, off, count));
                off++;
                count += s.Length + 1;
            }
            return fin;
        }

        private static Word GetStringofArray(int pos, IReadOnlyList<string> strings)
        {
            var cpos = 0;
            var apos = -1;
            while ((cpos < pos) && (apos < strings.Count - 1))
            {
                apos++;
                cpos += strings[apos].Length + 1;
            }
            return apos >= 0 ? new Word(strings[apos], apos, pos) : null;
        }

        public static Character GetCurrentCharacter(int pos, RichTextBox text)
            => pos > 0 ? new Character(text.Text.ToCharArray()[pos - 1], pos - 1) : null;

        public static IEnumerable<Character> GetCurrentLineChars(RichTextBox tbox)
        {
            var off = -1;
            var ln = tbox.GetLineFromCharIndex(tbox.SelectionStart);
            for (var i = 0; i < ln; i++)
                off += tbox.Lines[i].Length + 1;
            var fin = new List<Character>();
            foreach (var c in tbox.Lines[ln].ToCharArray())
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