using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class ForTilBlock : Block
    {
        private ByteVariable _limit;

        public ForTilBlock(Label endLabel, ByteVariable limit) : base(endLabel)
        {
            _limit = limit;
        }
    }
}