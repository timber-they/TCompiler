using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TIDE.Forms.Tools
{
    public static class Formatting
    {
        /// <summary>
        ///     Formats the text
        /// </summary>
        /// <param name="text">The text to format</param>
        /// <returns>The whole formatted text</returns>
        public static string FormatText (string text)
        {
            var lines = text.Split ('\n');
            var layer = 0;
            var res   = new StringBuilder ();
            foreach (var line in lines)
            {
                if (PublicStuff.EndCommands.Any (s => line.Trim ().StartsWith (s)) && layer > 0)
                    layer--;
                res.AppendLine ($"{string.Join ("", Enumerable.Repeat (' ', layer * 4))}{line.Trim ()}");
                if (PublicStuff.BeginningCommands.Any (s => line.Trim ().StartsWith (s)))
                    layer++;
            }

            if (lines.Length > 0)
                res.Remove (res.Length - 2, 2);
            return res.ToString ();
        }

        /// <summary>
        ///     Formats the line of the text
        /// </summary>
        /// <param name="text">The text of which to format the line from</param>
        /// <param name="line">The line index to format from the text</param>
        /// <returns>The text with the formatted line</returns>
        public static string FormatLine (string text, int line) =>
            string.Join ("\n",
                         text.Split ('\n').
                              Select ((s, i) => i == line
                                                    ? FormatText (text).
                                                        Split ('\n') [i]
                                                    : s));

        /// <summary>
        ///     Formats the lines of the text
        /// </summary>
        /// <param name="text">The text of which to format the lines from</param>
        /// <param name="lines">The lines to format from the text</param>
        /// <returns>The text with the formatted lines</returns>
        public static string FormatLines (string text, List <int> lines) =>
            string.Join ("\n",
                         text.Split ('\n').
                              Select ((s, i) =>
                                          lines.
                                              Contains (i)
                                              ? FormatText (text).
                                                  Split ('\n') [i]
                                              : s));
    }
}