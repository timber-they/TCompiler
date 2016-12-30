namespace TCompiler.Types.CompilingTypes.Block
{
    public class WhileBlock : Block
    {
        public WhileBlock(Label endLabel, Condition condition, Label upperLabel) : base(endLabel)
        {
            Condition = condition;
            UpperLabel = upperLabel;
        }

        public Condition Condition { get; }
        public Label UpperLabel { get; }
    }
}