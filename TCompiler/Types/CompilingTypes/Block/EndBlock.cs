namespace TCompiler.Types.CompilingTypes.Block
{
    public class EndBlock : Command
    {
        public Block Block { get; }

        public EndBlock(Block block)
        {
            Block = block;
        }
    }
}