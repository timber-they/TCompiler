using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace TIDE.Colouring.StringFunctions
{
    public static class EvaluateIfColouredAndGetColour
    {
        public static Color IsColouredAndColor(string word, bool asm, string line, int linePos)
        {
            if (word.Length <= 0) return Color.Empty;
            int foo;
            var semiIndex = line.ToCharArray().ToList().IndexOf(';');

            return semiIndex >= 0 && semiIndex < linePos-1
                ? PublicStuff.CommentColor
                : (word.StartsWith("#") ||
                   int.TryParse(word, NumberStyles.Integer, CultureInfo.InvariantCulture, out foo)
                    ? PublicStuff.NumberColor
                    : (!asm
                        ? PublicStuff.StringColorsTCode.FirstOrDefault(
                                  color => string.Equals(color.Thestring, word, StringComparison.CurrentCultureIgnoreCase))?
                              .Thecolor ?? PublicStuff.StandardColor
                        : PublicStuff.StringColorsAssembler.FirstOrDefault(
                                  color => string.Equals(color.Thestring, word, StringComparison.CurrentCultureIgnoreCase))?
                              .Thecolor ?? PublicStuff.StandardColor));
        }
    }
}