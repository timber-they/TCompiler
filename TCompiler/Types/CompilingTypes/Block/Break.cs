using TCompiler.Types.CompilerTypes;


namespace TCompiler.Types.CompilingTypes.Block
{
    /// <summary>
    ///     The command to jump to the end of the current Block<br />
    ///     Syntax:<br />
    ///     Break
    /// </summary>
    public class Break : Command
    {
        /// <summary>
        ///     Initiates a new break command
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="currentBlock">The current block to break</param>
        /// <param name="cLine">The original T code line</param>
        public Break (Block currentBlock, CodeLine cLine) : base (false, false, cLine) => CurrentBlock = currentBlock;

        /// <summary>
        ///     The block the break has to break
        /// </summary>
        /// <value>The block as a Block or inheriting type of Block</value>
        public Block CurrentBlock { get; }
    }
}