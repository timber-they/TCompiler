#region

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace TIDE.Coloring.StringFunctions
{
    /// <summary>
    ///     Provides methods for evaluating colors
    /// </summary>
    public static class EvaluateColor
    {
        public static async Task<Color> GetColor(string word, bool asm, string line, int linePos)
        {
            return await Task.Run(() =>
            {
                int foo;
                var semiIndex = line.ToCharArray().ToList().IndexOf(';');

                return (semiIndex >= 0) && (semiIndex <= linePos)
                    ? PublicStuff.CommentColor
                    : ((word.StartsWith("#") && asm) || (word.StartsWith("0x") && !asm) ||
                       int.TryParse(word, NumberStyles.Integer, CultureInfo.InvariantCulture, out foo)
                        ? PublicStuff.NumberColor
                        : (!asm
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
            });
        }
    }
}