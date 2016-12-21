using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class WhileBlock : Block
    {
        private Condition _condition;
        private bool _isDoWhile;

        protected WhileBlock(List<Command> content, Label endLabel, Condition condition, bool isDoWhile) : base(content, endLabel)
        {
            _condition = condition;
            _isDoWhile = isDoWhile;
        }
    }
}