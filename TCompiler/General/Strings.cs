using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCompiler.General
{
    public static class Strings
    {

        /// <summary>
        ///     Trims a string like the normal "...".Trim() method but works with a string as trimmer
        /// </summary>
        /// <param name="trimmer">The string that splits the other string</param>
        /// <param name="tString">The string that shall get splitted</param>
        /// <returns>The trimmed string</returns>
        public static string Trim(string trimmer, string tString)
        {
            while (tString.EndsWith(trimmer))
                tString = tString.Substring(0, tString.Length - trimmer.Length);
            while (tString.StartsWith(trimmer))
                tString = tString.Substring(trimmer.Length, tString.Length - trimmer.Length);
            return tString;
        }

        /// <summary>
        ///     Splits the given string by a string
        /// </summary>
        /// <param name="toSplit">The string that shall get splitted</param>
        /// <param name="splitter">The string that shall split the other string</param>
        /// <param name="options">The string split options - similiar to the classic split</param>
        /// <returns>A list of the parts of the string as splitted string</returns>
        public static IEnumerable<string> Split(string toSplit, string splitter, StringSplitOptions options = StringSplitOptions.None)
        {
            var fin = new List<string>();
            var sb = new StringBuilder();
            var ts = toSplit.ToCharArray();
            var s = splitter.ToCharArray();

            for (var i = 0; i <= ts.Length - (s.Length - 1); i++)
            {
                if (s.Where((t, j) => t != ts[i + j]).Any())
                {
                    sb.Append(ts[i]);
                    continue;
                }
                fin.Add(sb.ToString());
                sb = new StringBuilder();
                i += splitter.Length - 1;
            }

            if (sb.Length > 0)
                fin.Add(sb.ToString());

            return options == StringSplitOptions.None ? fin : fin.Where(s1 => !string.IsNullOrEmpty(s1));
        }
    }
}