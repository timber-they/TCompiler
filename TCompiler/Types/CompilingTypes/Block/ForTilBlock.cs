#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.Block
{
    public class ForTilBlock : Block
    {
        public ForTilBlock(Label endLabel, ByteVariableCall limit, Label upperLabel, ByteVariable variable)
            : base(endLabel)
        {
            Limit = limit;
            UpperLabel = upperLabel;
            Variable = variable;
        }

        public ByteVariableCall Limit { get; }
        public ByteVariable Variable { get; }
        public Label UpperLabel { get; }
    }
}