using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class Block : Command
    {
        public Label EndLabel { get; set; }

        public Block(Label endLabel)
        {
            EndLabel = endLabel;
        }
    }
}