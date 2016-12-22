using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class Block : Command
    {
        private Label _endLabel;
        private List<Command> _content;

        protected Block(List<Command> content, Label endLabel)
        {
            _content = content;
            _endLabel = endLabel;
        }
    }
}