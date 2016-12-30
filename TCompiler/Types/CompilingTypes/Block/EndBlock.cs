namespace TCompiler.Types.CompilingTypes.Block
{
    public class EndBlock : Command
    {
        public EndBlock(Block block)
        {
            Block = block;
        }

        public Block Block { get; }
    }
}