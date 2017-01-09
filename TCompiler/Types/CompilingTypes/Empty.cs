namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// Represents an empty line in T. Will get removed in the compilation to assembler
    /// </summary>
    public class Empty : Command
    {
        public Empty() : base(false, false)
        {
        }
    }
}