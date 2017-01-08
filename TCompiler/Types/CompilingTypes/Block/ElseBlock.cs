namespace TCompiler.Types.CompilingTypes.Block
{
    public class ElseBlock : Block
    {
        public ElseBlock(Label endLabel, Label elseLabel) : base(endLabel)
        {
            ElseLabel = elseLabel;
        }

        public Label ElseLabel { get; }
    }
}