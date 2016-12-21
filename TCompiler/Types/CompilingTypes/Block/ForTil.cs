using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class ForTil : Block
    {
        private int _limit;

        protected ForTil(List<Command> content, Label endLabel, int limit) : base(content, endLabel)
        {
            _limit = limit;
        }
    }
}