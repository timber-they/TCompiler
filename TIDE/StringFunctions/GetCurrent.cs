using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TIDE.Types;

namespace TIDE.StringFunctions
{
    public static class GetCurrent
    {
        public static stringint GetCurrentWord (int pos, RichTextBox text) => GetStringofArray(pos, text.Text.Split(PublicStuff.Splitters));

        public static IEnumerable<stringint> GetAllWords(RichTextBox text) => text.Text.Split(PublicStuff.Splitters).Select((s, i) =>
        {
            i++;
            return i > 0 ? new stringint(s, i - 1) : null;
        });

        public static IEnumerable<stringint> GetAllChars(RichTextBox text)
            => text.Text.ToCharArray().Select((c, i) => i > 0 ? new stringint(c.ToString(), i - 1) : null);

        private static stringint GetStringofArray (int pos, IReadOnlyList<string> strings)
        {
            var cpos = 0;
            var apos = -1;
            while (cpos < pos && apos < strings.Count - 1)
            {
                apos++;
                cpos += strings[apos].Length + 1;
            }
            return apos >= 0 ? new stringint (strings[apos], apos) : null;
        }

        public static stringint GetCurrentCharacter(int pos, RichTextBox text) => pos > 0 ? new stringint(text.Text.ToCharArray()[pos - 1].ToString(), pos - 1) : null;
    }
}