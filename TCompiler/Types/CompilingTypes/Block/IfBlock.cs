namespace TCompiler.Types.CompilingTypes.Block
{
    public class IfBlock : Block
    {
        public IfBlock(Label endLabel, Condition condition, ElseBlock @else) : base(endLabel)
        {
            Condition = condition;
            Else = @else;
        }

        public Condition Condition { get; }
        public ElseBlock Else { get; set; }
    }
}