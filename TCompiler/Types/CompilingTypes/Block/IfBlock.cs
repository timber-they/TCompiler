namespace TCompiler.Types.CompilingTypes.Block
{
    public class IfBlock : Block
    {
        public IfBlock(Label endLabel, Condition condition) : base(endLabel)
        {
            Condition = condition;
        }

        public Condition Condition { get; }
    }
}