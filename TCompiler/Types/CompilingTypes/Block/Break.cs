namespace TCompiler.Types.CompilingTypes.Block
{
    public class Break : Command
    {
        public Break(Block currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public Block CurrentBlock { get; }
    }
}