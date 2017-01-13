#region

using System.Collections.Generic;
using System.Drawing;
using TIDE.Colouring.Types;

#endregion

namespace TIDE
{
    public static class PublicStuff
    {
        public static readonly List<StringColor> StringColorsTCode = new List<StringColor>
        {
            new StringColor("int", TypeColor),
            new StringColor("if", BlockColor),
            new StringColor("else", BlockColor),
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
            new StringColor("sleep", OtherKeywordColor),
            new StringColor("isrexternal0", IsrColor),
            new StringColor("isrexternal1", IsrColor),
            new StringColor("endisr", IsrColor)
        };

        public static readonly List<StringColor> StringColorsAssembler = new List<StringColor>
        {
            new StringColor("add", ArithmeticColor),
            new StringColor("addc", ArithmeticColor),
            new StringColor("subb", ArithmeticColor),
            new StringColor("inc", ArithmeticColor),
            new StringColor("dec", ArithmeticColor),
            new StringColor("mul", ArithmeticColor),
            new StringColor("div", ArithmeticColor),
            new StringColor("da", ArithmeticColor),
            new StringColor("anl", LogicalColor),
            new StringColor("orl", LogicalColor),
            new StringColor("xrl", LogicalColor),
            new StringColor("clr", LogicalColor),
            new StringColor("cpl", LogicalColor),
            new StringColor("mov", TransportColor),
            new StringColor("movc", TransportColor),
            new StringColor("movx", TransportColor),
            new StringColor("push", TransportColor),
            new StringColor("pop", TransportColor),
            new StringColor("xch", TransportColor),
            new StringColor("xchd", TransportColor),
            new StringColor("swap", TransportColor),
            new StringColor("nop", NoColor),
            new StringColor("setb", LogicalColor),
            new StringColor("rl", ArithmeticColor),
            new StringColor("rlc", ArithmeticColor),
            new StringColor("rr", ArithmeticColor),
            new StringColor("rrc", ArithmeticColor),
            new StringColor("call", MethodColor),
            new StringColor("ret", MethodColor),
            new StringColor("jmp", JumpColor),
            new StringColor("jz", JumpColor),
            new StringColor("jnz", JumpColor),
            new StringColor("jc", JumpColor),
            new StringColor("jnc", JumpColor),
            new StringColor("jb", JumpColor),
            new StringColor("jnb", JumpColor),
            new StringColor("jbc", JumpColor),
            new StringColor("cjne", JumpColor),
            new StringColor("djnz", JumpColor),
            new StringColor("data", SimulatorSpecialColor),
            new StringColor("bit", SimulatorSpecialColor),
            new StringColor("include", SimulatorSpecialColor)
        };

        public static readonly Color SplitterColor = Color.LimeGreen;
        public static readonly Color StandardColor = Color.White;

        public static readonly char[] Splitters =
        {
            ' ', ',', '.', ';', ':', '!', '?', '/', '\\', '&', '\"', '-', '_',
            '%', '(', ')', '{', '}', '[', ']', '=', '*', '^', '>', '<', '\n', '+',
            '|'
        };

        private static Color SimulatorSpecialColor => Color.LawnGreen;
        private static Color JumpColor => Color.DodgerBlue;
        private static Color NoColor => Color.Gray;
        private static Color TransportColor => Color.GreenYellow;
        private static Color LogicalColor => Color.Cyan;
        private static Color ArithmeticColor => Color.OrangeRed;

        private static Color MethodColor => Color.DarkCyan;
        private static Color OtherKeywordColor => Color.DodgerBlue;
        private static Color BlockColor => Color.OrangeRed;
        private static Color TypeColor => Color.Cyan;
        private static Color IsrColor => Color.DarkOrchid;

        public static Color NumberColor => Color.DeepPink;

        public static Color CommentColor => Color.Gray;
    }
}