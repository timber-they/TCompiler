#region

using System.Collections.Generic;
using System.Drawing;
using TIDE.Coloring.Types;

#endregion

namespace TIDE
{
    /// <summary>
    ///     Provides some public stuff
    /// </summary>
    public static class PublicStuff
    {
        /// <summary>
        ///     The commands with their colors in T
        /// </summary>
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
            new StringColor("collection", TypeColor),
            new StringColor("return", OtherKeywordColor),
            new StringColor("method", MethodColor),
            new StringColor("endmethod", MethodColor),
            new StringColor("sleep", OtherKeywordColor),
            new StringColor("isrexternal0", IsrColor),
            new StringColor("isrexternal1", IsrColor),
            new StringColor("isrtimer0", IsrColor),
            new StringColor("isrtimer1", IsrColor),
            new StringColor("isrcounter0", IsrColor),
            new StringColor("isrcounter1", IsrColor),
            new StringColor("endisr", IsrColor)
        };

        /// <summary>
        ///     The commands with their colors in assembler
        /// </summary>
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

        /// <summary>
        ///     The standard splitter color
        /// </summary>
        public static readonly Color SplitterColor = Color.LimeGreen;

        /// <summary>
        ///     The standard text color
        /// </summary>
        public static readonly Color StandardColor = Color.White;

        /// <summary>
        ///     A collection of the splitters
        /// </summary>
        public static readonly char[] Splitters =
        {
            ' ', ',', '.', ';', ':', '!', '?', '/', '\\', '&', '\"', '-', '_',
            '%', '(', ')', '{', '}', '[', ']', '=', '*', '^', '>', '<', '\n', '+',
            '|','#'
        };

        /// <summary>
        ///     All the commands that define the end of a block/method
        /// </summary>
        public static readonly List<string> EndCommands = new List<string>
        {
            "endif",
            "endwhile",
            "endblock",
            "endfortil",
            "endmethod",
            "endisr"
        };

        /// <summary>
        ///     A special color for the simulator commands
        /// </summary>
        private static Color SimulatorSpecialColor => Color.LawnGreen;

        /// <summary>
        ///     A color for assembler jump commands
        /// </summary>
        private static Color JumpColor => Color.DodgerBlue;

        /// <summary>
        ///     The color for no operation in assembler
        /// </summary>
        private static Color NoColor => Color.Gray;

        /// <summary>
        ///     The color for transport commands in assembler
        /// </summary>
        private static Color TransportColor => Color.GreenYellow;

        /// <summary>
        ///     The color for logical commands in assembler
        /// </summary>
        private static Color LogicalColor => Color.Cyan;

        /// <summary>
        ///     The color for arithmetic operations in assembler
        /// </summary>
        private static Color ArithmeticColor => Color.OrangeRed;

        /// <summary>
        ///     The color for methods in T
        /// </summary>
        private static Color MethodColor => Color.DarkCyan;

        /// <summary>
        ///     The color for other keywords in T
        /// </summary>
        private static Color OtherKeywordColor => Color.DodgerBlue;

        /// <summary>
        ///     The color for blocks in T
        /// </summary>
        private static Color BlockColor => Color.OrangeRed;

        /// <summary>
        ///     The color for type identifiers in T
        /// </summary>
        private static Color TypeColor => Color.Cyan;

        /// <summary>
        ///     The color for ISR methods in T
        /// </summary>
        private static Color IsrColor => Color.DarkOrchid;

        /// <summary>
        ///     The universal color for numbers
        /// </summary>
        public static Color NumberColor => Color.DeepPink;

        /// <summary>
        ///     The universal color for comments
        /// </summary>
        public static Color CommentColor => Color.Gray;
    }
}