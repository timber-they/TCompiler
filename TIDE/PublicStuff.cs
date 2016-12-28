using System.Collections.Generic;
using System.Drawing;
using TIDE.Types;

namespace TIDE
{
    public static class PublicStuff
    {
        public static List<StringColor> StringColors = new List<StringColor>()
        {
            new StringColor("int", TypeColor),
            new StringColor("if", BlockColor),
            new StringColor("endif", BlockColor),
            new StringColor("bool", TypeColor),
            new StringColor("while", BlockColor),
            new StringColor("endwhile", BlockColor),
            new StringColor("break", OtherKeywordColor),
            new StringColor("block", BlockColor),
            new StringColor("endblock", BlockColor),
            new StringColor("fortil", BlockColor),
            new StringColor("endfortil", BlockColor),
            new StringColor("cint", TypeColor),
            new StringColor("char", TypeColor),
            new StringColor("return", OtherKeywordColor),
            new StringColor("method", MethodColor),
            new StringColor("endmethod", MethodColor),
            new StringColor("sleep", OtherKeywordColor)
        };

        private static Color MethodColor => Color.DarkCyan;
        private static Color OtherKeywordColor => Color.DodgerBlue;
        private static Color BlockColor => Color.OrangeRed;
        private static Color TypeColor => Color.Cyan;
        public static readonly Color SplitterColor = Color.LimeGreen;
        public static readonly Color StandardColor = Color.White;

        public static readonly char[] Splitters = {' ', ',', '.', ';', ':', '!', '?', '/', '\\', '&', '\"', '-', '_', '%', '(', ')', '{', '}', '[', ']', '=', '#', '*', '^', '>', '<', '\n'};
    }
}