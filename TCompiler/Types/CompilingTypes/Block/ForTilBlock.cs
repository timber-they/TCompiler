using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class ForTilBlock : Block
    {
        public VariableCall Limit { get; }
        public Label UpperLabel { get; }

        public ForTilBlock(Label endLabel, VariableCall limit, Label upperLabel) : base(endLabel)
        {
            Limit = limit;
            UpperLabel = upperLabel;
        }
    }
}