namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Method : ReturningCommand
    {
        private Block.Block _content;
        public string Name { get; }

        public Method(Block.Block content, string name)
        {
            _content = content;
            Name = name;
        }
    }
}