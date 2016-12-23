namespace TCompiler.Types.CompilingTypes.Block
{
    public class WhileBlock : Block
    {
        public Condition Condition { get; }
        public Label UpperLabel { get; }

        public WhileBlock(Label endLabel, Condition condition, Label upperLabel) : base(endLabel)
        {
            Condition = condition;
            UpperLabel = upperLabel;
        }
    }
}