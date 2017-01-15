namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// Represents an empty line in T. Will get removed in the compilation to assembler
    /// </summary>
    public class Empty : Command
    {
        /// <summary>
        /// Initializes a new empty command
        /// </summary>
        public Empty() : base(false, false, null)
        {
        }
    }
}