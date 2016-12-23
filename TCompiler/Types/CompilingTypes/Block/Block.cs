using System.Collections.Generic;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.Block
{
    public class Block : Command
    {
        public Label EndLabel { get; set; }

        public readonly List<Variable> Variables;

        public Block(Label endLabel)
        {
            Variables = new List<Variable>();
            EndLabel = endLabel;
        }
    }
}