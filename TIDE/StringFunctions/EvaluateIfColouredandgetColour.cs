using System;
using System.Drawing;
using System.Linq;
using TIDE.Types;

namespace TIDE.StringFunctions
{
    public static class EvaluateIfColouredAndGetColour
    {
        public static Color IsColouredAndColor(string word)
        {
            if (word.Length <= 0) return Color.Empty;

            return PublicStuff.StringColors.FirstOrDefault(
                    color => string.Equals(color.Thestring, word, StringComparison.CurrentCultureIgnoreCase))?.Thecolor ?? PublicStuff.StandardColor;
        }
    }
}