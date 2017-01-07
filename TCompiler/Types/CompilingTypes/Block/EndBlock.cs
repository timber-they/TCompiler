namespace TCompiler.Types.CompilingTypes.Block
{
    /// <summary>
    /// Specifies the end of the current block
    /// </summary>
    /// <remarks>Can be a } and a endBlock</remarks>
    public class EndBlock : Command
    {
        /// <summary>
        /// Initiates a new endBlock
        /// </summary>
        /// <returns>Nothing</returns>
        public EndBlock(Block block)
        {
            Block = block;
        }

        /// <summary>
        /// The block the endBlock is the end of
        /// </summary>
        /// <value>The block as a block or an inheriting type of it</value>
        public Block Block { get; }
    }
}