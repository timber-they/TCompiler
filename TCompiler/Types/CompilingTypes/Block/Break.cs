namespace TCompiler.Types.CompilingTypes.Block
{
    public class Break : Command
    {
        public Block CurrentBlock { get; }

        public Break(Block currentBlock)
        {
            CurrentBlock = currentBlock;
        }
    }
}