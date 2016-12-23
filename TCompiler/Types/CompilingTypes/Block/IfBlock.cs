namespace TCompiler.Types.CompilingTypes.Block
{
    public class IfBlock : Block
    {
        private Label _endLabel;
        private Condition _condition;

        public IfBlock(Label endLabel, Condition condition) : base(endLabel)
        {
            _endLabel = endLabel;
            _condition = condition;
        }
    }
}