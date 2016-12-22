namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Method : ReturningCommand
    {
        private Block.Block _content;

        public Method(Block.Block content)
        {
            _content = content;
        }
    }
}