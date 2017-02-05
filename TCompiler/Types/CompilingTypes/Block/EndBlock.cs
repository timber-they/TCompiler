using TCompiler.Types.CompilerTypes;

namespace TCompiler.Types.CompilingTypes.Block
{
    /// <summary>
    ///     Specifies the end of the current block<br />
    ///     Syntax:<br />
    ///     endblock
    /// </summary>
    /// <remarks>Can be a } and a endBlock</remarks>
    public class EndBlock : Command
    {
        /// <summary>
        ///     Initiates a new endBlock
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="block">The block the endBlock is the end of</param>
        /// <param name="cLine">The original T code line</param>
        public EndBlock(Block block, CodeLine cLine) : base(false, false, cLine)
        {
            Block = block;
        }

        /// <summary>
        ///     The block the endBlock is the end of
        /// </summary>
        /// <value>The block as a block or an inheriting type of it</value>
        public Block Block { get; }
    }
}