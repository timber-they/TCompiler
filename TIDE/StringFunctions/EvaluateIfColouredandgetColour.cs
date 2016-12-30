using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using TIDE.Types;

namespace TIDE.StringFunctions
{
    public static class EvaluateIfColouredAndGetColour
    {
        public static Color IsColouredAndColor(string word, bool asm)
        {
            if (word.Length <= 0) return Color.Empty;
            int foo;

            return word.StartsWith("#") || int.TryParse(word, NumberStyles.Integer, CultureInfo.InvariantCulture, out foo) ? PublicStuff.NumberColor :  (!asm
                ? PublicStuff.StringColorsTCode.FirstOrDefault(
                          color => string.Equals(color.Thestring, word, StringComparison.CurrentCultureIgnoreCase))?
                      .Thecolor ?? PublicStuff.StandardColor
                : PublicStuff.StringColorsAssembler.FirstOrDefault(
                          color => string.Equals(color.Thestring, word, StringComparison.CurrentCultureIgnoreCase))?
                      .Thecolor ?? PublicStuff.StandardColor);
        }
    }
}