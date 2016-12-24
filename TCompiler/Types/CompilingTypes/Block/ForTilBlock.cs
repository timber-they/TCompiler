using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class ForTilBlock : Block
    {
        public ByteVariableCall Limit { get; }
        public ByteVariable Variable { get; }
        public Label UpperLabel { get; }

        public ForTilBlock(Label endLabel, ByteVariableCall limit, Label upperLabel, ByteVariable variable) : base(endLabel)
        {
            Limit = limit;
            UpperLabel = upperLabel;
            Variable = variable;
        }
    }
}