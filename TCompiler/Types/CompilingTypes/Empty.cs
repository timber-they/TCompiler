using TCompiler.Types.CompilerTypes;

namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    ///     Represents an empty line in T. Will get removed in the compilation to assembler
    /// </summary>
    public class Empty : Command
    {
        /// <summary>
        ///     Initializes a new empty command
        /// </summary>
        public Empty(CodeLine tCode) : base(false, false, tCode)
        {
        }
    }
}