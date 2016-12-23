namespace TCompiler.Types.CompilingTypes.Block
{
    public class WhileBlock : Block
    {
        private Condition _condition;

        public WhileBlock(Label endLabel, Condition condition) : base(endLabel)
        {
            _condition = condition;
        }
    }
}