#region

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

#endregion

namespace TIDE.Coloring.StringFunctions
{
    /// <summary>
    ///     Provides methods for evaluating colors
    /// </summary>
    public static class EvaluateColor
    {
        /// <summary>
        ///     Evaluates the color for the given word
        /// </summary>
        /// <param name="word">The word to evaluate the color for</param>
        /// <param name="assembler">Indicates wether the language is assembler and not T</param>
        /// <param name="line">The line in which the word is</param>
        /// <param name="lineIndex">The index of the line in the textBox</param>
        /// <returns>The evaluated color as a Color</returns>
        public static Color GetColor(string word, bool assembler, string line, int lineIndex)
        {
            int foo;
            var semiIndex = line.ToCharArray().ToList().IndexOf(';');

            return semiIndex >= 0 && semiIndex <= lineIndex
                ? PublicStuff.CommentColor
                : ((word.FirstOrDefault() == '#' || char.IsNumber(word.FirstOrDefault())) && assembler ||
                   word.StartsWith("0x") && !assembler ||
                   int.TryParse(word, NumberStyles.Integer, CultureInfo.InvariantCulture, out foo)
                    ? PublicStuff.NumberColor
                    : (!assembler
                        ? PublicStuff.StringColorsTCode.FirstOrDefault(
                                  color =>
                                      string.Equals(color.Thestring, word,
                                          StringComparison.CurrentCultureIgnoreCase))
                              ?
                              .Thecolor ?? PublicStuff.StandardColor
                        : PublicStuff.StringColorsAssembler.FirstOrDefault(
                                  color =>
                                      string.Equals(color.Thestring, word,
                                          StringComparison.CurrentCultureIgnoreCase))
                              ?
                              .Thecolor ?? PublicStuff.StandardColor));
        }
    }
}