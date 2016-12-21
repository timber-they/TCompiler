using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class IfBlock : Block
    {
        private Label _endLabel;
        private Condition _condition;

        protected IfBlock(List<Command> content, Label endLabel, Condition condition) : base(content, endLabel)
        {
            _endLabel = endLabel;
            _condition = condition;
        }
    }
}