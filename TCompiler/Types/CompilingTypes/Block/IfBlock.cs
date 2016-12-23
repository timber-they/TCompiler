namespace TCompiler.Types.CompilingTypes.Block
{
    public class IfBlock : Block
    {
        public Condition Condition { get; }

        public IfBlock(Label endLabel, Condition condition) : base(endLabel)
        {
            Condition = condition;
        }
    }
}