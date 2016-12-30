using System.Collections.Generic;

namespace TCompiler.Settings
{
    public static class GlobalSettings
    {
        public static readonly List<string> InvalidNames = new List<string>
        {
            "add",
            "subb",
            "inc",
            "dec",
            "mul",
            "div",
            "da",
            "anl",
            "orl",
            "xrl",
            "clr",
            "cpl",
            "mov",
            "movc",
            "movx",
            "push",
            "pop",
            "xch",
            "xchd",
            "swap",
            "nop",
            "setb",
            "rl",
            "rlc",
            "rr",
            "rrc",
            "call",
            "ret",
            "jmp",
            "jz",
            "jnz",
            "jc",
            "jnc",
            "jb",
            "jnb",
            "jbc",
            "cjne",
            "djnz",
            "data",
            "bit",
            "include",
            "int",
            "if",
            "endif",
            "bool",
            "while",
            "endwhile",
            "break",
            "block",
            "endblock",
            "fortil",
            "endfortil",
            "cint",
            "char",
            "return",
            "method",
            "endmethod",
            "sleep"
        };

        public static string InputPath { get; set; }
        public static string OutputPath { get; set; }
        public static string ErrorPath { get; set; }
    }
}