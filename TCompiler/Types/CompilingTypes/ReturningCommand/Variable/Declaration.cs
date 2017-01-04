using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Declaration : Command
    {
        public Declaration(Assignment assignment, Variable variable)
        {
            Assignment = assignment;
            Variable = variable;
        }

        private Variable Variable { get; }
        private Assignment Assignment { get; }

        public override string ToString() => Assignment?.ToString();
    }
}